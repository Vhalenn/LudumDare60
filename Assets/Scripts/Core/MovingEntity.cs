using UnityEngine;

using DG.Tweening;
using Sirenix.OdinInspector;

public class MovingEntity : MonoBehaviour, IDamageReceiver
{
    [SerializeField] protected WorldManager worldManager;
    [SerializeField] protected CapsuleCollider2D col;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Rigidbody2D rb2;

    [Header("Life")]
    [SerializeField] protected float maxLife;
    [SerializeField] protected float life;
    Tween tweenDamage;

    [Header("Move Alterations")]
    protected bool boosted;
    protected bool stuned;

    protected float boostTime;
    protected float stunMeter;
    protected float stunTime;

    private string lastAttackSoundClip;

    [Header("Animation")]
    [SerializeField] protected Animator animator;
    [SerializeField, ReadOnly()] private AnimatorStateInfo currentAnimState;
    [SerializeField, ReadOnly()] protected AnimState animState;
    public bool UnableToMove => animState == AnimState.Hurted || animState == AnimState.Down;
    public bool Attacking => animState == AnimState.Attack;

    public const string IDLE_ANIM = "idle";
    public const string RUN_ANIM = "walk";
    public const string ATTACK_STATE = "attack";

    public const string HURTED_TRIGGER = "hurted";
    public const string DOWN_TRIGGER = "down";

    [Header("Audio")]
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioSource audioSourceWalking;
    [SerializeField] private AudioClip audioDown;
    [SerializeField] private AudioClip[] audioHurted;
    [SerializeField] private AudioClip[] audioAttack;
    [SerializeField] private AudioClip[] audioHeavyAttack;
    [SerializeField] private AudioClip[] audioAttackHitted;


    protected virtual void Start()
    {
        Init();

        audioSourceWalking.pitch = Random.Range(0.8f, 1.2f);
    }

    protected virtual void Init() // Called on start
    {
        life = maxLife; // Regen
    }

    protected virtual void Update()
    {
        if (animator)
        {
            currentAnimState = animator.GetCurrentAnimatorStateInfo(0);

            if (currentAnimState.IsName(DOWN_TRIGGER))
            {
                animState = AnimState.Down;
                lastAttackSoundClip = "down";
            }
            else if (currentAnimState.IsName(HURTED_TRIGGER))
            {
                animState = AnimState.Hurted;
                lastAttackSoundClip = "hurted";
            }
            else if (currentAnimState.IsName(IDLE_ANIM) || currentAnimState.IsName(RUN_ANIM))
            {
                animState = AnimState.Still;
                lastAttackSoundClip = "still";
            }
            else
            {
                animState = AnimState.Attack;
            }
        }
    }

    public virtual void Down()
    {
        animator.SetBool(DOWN_TRIGGER, true);
    }

    public Transform damageTransform => transform;

    public virtual void TakeDamages(float value, Vector2 dir)
    {
        if (life <= 0f)
        {
            // DEAD confirmation to be sure
            life = -555f;
            Down();
            return;
        }

        life -= value;
        Debug.Log($"{transform.name} took damages: {value} -> life: {life}");

        PlaySound(audioHurted, Random.Range(0.7f, 1.2f));

        if (animator)
        {
            animator.SetTrigger(HURTED_TRIGGER);

            bool isDown = life <= 0f;
            if (isDown)
            {
                Down();
                Debug.Log($"{transform.name} is DOWN");

                var seq = DOTween.Sequence(); //new Sequence()
                seq.AppendInterval(0.2f);
                seq.AppendCallback(() => PlaySound(audioDown, Random.Range(0.25f, 0.75f)));
            }

            animator.SetBool(DOWN_TRIGGER, isDown);
            if (col) col.enabled = !isDown;

            tweenDamage?.Complete();
            tweenDamage = animator.transform.DOShakePosition(0.35f, new Vector3(0.07f, 0f, 0f), vibrato: 2000, randomness:0, fadeOut:false);
        }
    }

    public void PlaySound(AudioClip[] soundArray, float volume = 1f)
    {
        if (audioSource && soundArray != null)
        {
            AudioClip sound = soundArray[Random.Range(0, soundArray.Length - 1)];

            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(sound, volume);
        }
    }

    public void PlaySound(AudioClip sound, float volume = 1f)
    {
        if (audioSource && sound)
        {
            audioSource.PlayOneShot(sound, volume);
        }
    }

    public void PlayHitSound() // Called from Hitbox
    {
        //Debug.Log($"{transform.name} hit landed");
        PlaySound(audioAttackHitted, Random.Range(0.25f, 0.5f));
    }

    public bool Attack()
    {
        if(GameManager.instance && GameManager.instance.InMenu)
        {
            return false;
        }

        if(animState == AnimState.Hurted)
        {
            // Cannot attack
            return false;
        }

        bool canPlaySound = true;
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        foreach(AnimatorClipInfo clip in clipInfo)
        {
            if(clip.clip)
            {
                canPlaySound = lastAttackSoundClip != clip.clip.name;
                if(canPlaySound) Debug.Log($"Got clip: {clip.clip.name} - w: {clip.weight} -> Before: {lastAttackSoundClip}");

                lastAttackSoundClip = clip.clip.name;
                break;
            }
        }

        if(canPlaySound) PlaySound(audioAttack, Random.Range(0.3f, 0.7f));

        if (animator)
        {
            animator.SetTrigger("attack");
        }

        return true;
    }

    public bool HeavyAttack() // Maybe?
    {
        if (animState == AnimState.Hurted)
        {
            // Cannot attack
            return false;
        }

        Debug.Log("Heavy attack!");

        bool canPlaySound = true;
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        foreach (AnimatorClipInfo clip in clipInfo)
        {
            if (clip.clip)
            {
                canPlaySound = lastAttackSoundClip != clip.clip.name;
                if (canPlaySound) Debug.Log($"Got clip: {clip.clip.name} - w: {clip.weight} -> Before: {lastAttackSoundClip}");

                lastAttackSoundClip = clip.clip.name;
                break;
            }
        }

        if (canPlaySound) PlaySound(audioAttack, Random.Range(0.6f, 1.0f));

        if (animator)
        {
            animator.SetTrigger("attackHeavy");
        }

        return true;

    }
}
public enum AnimState
{
    Still,
    Attack,
    Hurted,
    Down,
}
