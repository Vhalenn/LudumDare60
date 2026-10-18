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

    public enum AnimState
    {
        Still,
        Attack,
        Hurted,
        Down,
    }

    protected virtual void Start()
    {
        Init();
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
            }
            else if (currentAnimState.IsName(HURTED_TRIGGER))
            {
                animState = AnimState.Hurted;
            }
            else if (currentAnimState.IsName(IDLE_ANIM) || currentAnimState.IsName(RUN_ANIM))
            {
                animState = AnimState.Still;
            }
            else
            {
                animState = AnimState.Attack;
            }
        }
    }

    public virtual void Dead()
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
            Dead();
            return;
        }

        life -= value;
        Debug.Log($"{transform.name} took damages: {value} -> life: {life}");

        if(animator)
        {
            animator.SetTrigger(HURTED_TRIGGER);

            bool dead = life <= 0f;
            if (dead)
            {
                Dead();
                Debug.Log($"{transform.name} is DEAD");
            }

            animator.SetBool(DOWN_TRIGGER, dead);
            if (col) col.enabled = !dead;

            tweenDamage?.Complete();
            tweenDamage = animator.transform.DOShakePosition(0.35f, new Vector3(0.07f, 0f, 0f), vibrato: 2000, randomness:0, fadeOut:false);
        }
    }

    public bool Attack()
    {
        if(animState == AnimState.Hurted)
        {
            // Cannot attack
            return false;
        }

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

        if (animator)
        {
            animator.SetTrigger("attackHeavy");
        }

        return true;

    }
}
