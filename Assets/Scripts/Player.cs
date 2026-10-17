using System.Collections.Generic;

using UnityEngine;
using Sirenix.OdinInspector;

public class Player : MovingEntity
{
    [SerializeField] private WorldManager worldManager;
    [SerializeField] private Animator animator;

    [Header("Base Components")]
    public Vector2 controlInput;
    public bool desiredJump;
    [HideInInspector] private Transform camTransform;
    [HideInInspector] private Camera mainCam;

    [Header("Stats")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float speedVMultiplier = 1f;
    [SerializeField,Range(0,1f)] private float brakeForce = 0.9f;

    [Header("SafeZone")]
    [SerializeField] private Vector3 lastSafePos;
    [SerializeField] private List<TriggerSafeZone> safeAreaList;

    [Header("Animation")]
    [SerializeField, ReadOnly()] private AnimatorStateInfo currentAnimState;
    [SerializeField, ReadOnly()] private string currentAnim;
    [SerializeField, ReadOnly()] private bool isAttacking;
    public const string IDLE_ANIM = "idle";
    public const string RUN_ANIM = "walk";
    public const string ATTACK_ANIM = "attack";

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSourceWalking;
    [SerializeField] private AudioSource audioSourceGift;
    [SerializeField] private AudioClip stepSound;
    [SerializeField] private AudioClip takeGiftSound;
    [SerializeField] private AudioClip talkSound;


    [Header("Storage")]
    [SerializeField] private Vector3 desiredVelocity;
    [SerializeField] private Vector3 velocity;
    public bool InMenu => worldManager && worldManager.Game && worldManager.Game.InMenu;


    private void Start()
    {
        Init();
    }

    public void Init()
    {
        mainCam = Camera.main;
    }

    public void Teleport(Vector3 pos)
    {
        rb2.position = pos;
    }

    private void Update()
    {
        if(animator)
        {
            currentAnimState = animator.GetCurrentAnimatorStateInfo(0);


            if (currentAnimState.IsName(IDLE_ANIM) || currentAnimState.IsName(RUN_ANIM))
            {
                currentAnim = "still";
                isAttacking = false;
            }
            else
            {
                isAttacking = true;
                currentAnim = ATTACK_ANIM;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!rb2) return;
        /*
        if(rb2.position.y < -1f) // Kill plane
        {
            Teleport(lastSafePos + Vector3.up * 0.3f);

            return;
        }
        */
        velocity = rb2.linearVelocity;
        bool isNotMoving = controlInput.magnitude < 0.1f || InMenu;

        if (isNotMoving)
        {
            velocity.Scale( new (brakeForce, brakeForce, brakeForce));
        }
        else
        {
            // Should not be addition -> Should be capped
            //velocity = Vector3.Lerp(velocity, FollowingCamAngle(controlInput) * speed, 0.01f);

            desiredVelocity = FollowingCamAngle(controlInput) * speed;
            desiredVelocity.y *= speedVMultiplier;
            velocity = desiredVelocity;

            if(animator && Mathf.Abs(velocity.x) > 0.1f) // If is moving horizontally
            {
                animator.transform.localScale = new Vector3(velocity.x > 0f ? -1f : 1f, 1f, 1f);
            }

            //velocity.y = rb2.linearVelocity.y;
        }

        if(audioSourceWalking)
        {
            audioSourceWalking.volume = Mathf.Lerp(audioSourceWalking.volume, isNotMoving ? 0f : 1f, 0.33f);
        }

        if(animator)
        {
            animator.SetFloat("run", isNotMoving ? 0f : 1f);
        }

        // Apply speed
        rb2.linearVelocity = Vector3.Lerp(rb2.linearVelocity, velocity, 0.3f);
    }

    public Vector3 FollowingCamAngle(Vector2 input)
    {
        return new Vector3(input.x, input.y, 0f);

        if (!mainCam) Init();
        if (camTransform == null) camTransform = mainCam.transform;

        // 2D mode

        Vector3 forward = camTransform.forward;
        Vector3 right = camTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 dir = forward * input.y + right * input.x;
        return dir;
    }


    // jumping
    #region Jumping

    public void Jump()
    {
        animator.SetTrigger("jump");
    }

    #endregion Jumping

    // Attack
    #region Attack

    public void Attack()
    {
        animator.SetTrigger("attack");
        isAttacking = true;
    }

    public void HeavyAttack() // Maybe?
    {

    }

    public void Grab() // Maybe?
    {

    }

    #endregion Attack

    #region SafeZone
    public void AddTriggerZone(TriggerSafeZone safeZone)
    {
        if (safeAreaList == null) safeAreaList = new();

        if (!safeAreaList.Contains(safeZone))
        {
            safeAreaList.Add(safeZone);
        }
    }
    public void RemoveTriggerZone(TriggerSafeZone safeZone)
    {
        if (safeAreaList == null) safeAreaList = new();

        if(safeAreaList.Contains(safeZone))
        {
            safeAreaList.Remove(safeZone);
        }
    }
    #endregion SafeZone

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = OnGround ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
    */
}
