using UnityEngine;
using Sirenix.OdinInspector;

public class Enemy : MovingEntity
{
    [Header("Enemy specific")]
    [SerializeField] protected Player player;
    [SerializeField] private bool active = true; // For now, maybe disable for optimization
    [SerializeField, ReadOnly()] private Vector2 posDiff; // For now, maybe disable for optimization
    [SerializeField, ReadOnly()] private bool targetInRange; // For now, maybe disable for optimization

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float searchRange = 15f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 1.25f;
    [SerializeField] private float preAttackCooldown = 0.5f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField, ReadOnly()] private float preAttackTime;
    [SerializeField, ReadOnly()] private float attackTime;

    [Header("Down Anim")]
    [SerializeField] private AnimationCurve posXCurve;
    [SerializeField] private AnimationCurve posYCurve;
    [SerializeField, ReadOnly()] private bool destroyed;
    [SerializeField, ReadOnly()] private float destroyedTime;
    [SerializeField, ReadOnly()] private bool destroyedEndAnim;
    [SerializeField, ReadOnly()] private float randomForce = 0.5f;
    [SerializeField, ReadOnly()] private Vector3 startPos;
    [SerializeField, ReadOnly()] private Vector3 dir;

    protected override void Init()
    {
        base.Init();

        destroyed = false; // Be sure this is not off
        destroyedEndAnim = false; // Be sure this is not off

        if (worldManager)
        {
            player = worldManager.Player;
        }
    }

    private void FixedUpdate()
    {
        if (!active) return;

        if (animState == AnimState.Hurted)
        {
            // Stunned
            return;
        }

        // Movement stages
        UpdateMovement(Time.fixedDeltaTime);
    }

    protected override void Update()
    {
        base.Update();

        if (!active)
        {
            return;
        }

        // Code copied from props
        if (destroyedEndAnim)
        {
            //animator.SetTrigger(HURTED_TRIGGER);
            animator.SetBool(DOWN_TRIGGER, true);
            return;
        }

        if (destroyed)
        {
            // override anim
            animator.SetBool(DOWN_TRIGGER, destroyedTime > 0.9f);
            if(destroyedTime < 0.9f) animator.SetTrigger(HURTED_TRIGGER);

            destroyedTime += Time.deltaTime;
            rb2.position = startPos + (Vector3.up * posYCurve.Evaluate(destroyedTime) * randomForce) + (Vector3.right * dir.x * posXCurve.Evaluate(destroyedTime) * randomForce);

            if (destroyedTime > 1.0f)
            {
                //if (col) col.enabled = false;
                destroyedEndAnim = true; // END
            }

            return;
        }
        // END - Code copied from props

        if (animator.gameObject.TryGetComponent(out SpriteRenderer sp))
        {
            sp.color = animState == AnimState.Hurted ? Color.white * 1.3f : Color.white;
            animator.transform.localPosition = animState == AnimState.Hurted ? Vector3.right * Random.value * 0.15f : Vector3.zero;
        }

        if(UnableToMove)
        {
            // Stunned
            return;
        }

        posDiff = transform.position - player.transform.position;
        targetInRange = Mathf.Abs(posDiff.x) < attackRange && Mathf.Abs(posDiff.y) < HitBox.VERTICAL_RANGE * 0.9f;

        // Face player
        if (animator && !Attacking)
        {
            animator.transform.localScale = new Vector3(posDiff.x < 0f ? -1f : 1f, 1f, 1f);
        }

        // Attack stages
        UpdateAttack(Time.deltaTime);
    }

    private void UpdateMovement(float deltaTime)
    {
        bool isMoving = false;

        if (!rb2) return;


        if (GameManager.instance && GameManager.instance.InMenu)
        {
            // Wait, player in menu
        }
        else if (animState != AnimState.Still)
        {
            // Not moving
        }
        else if (targetInRange)
        {
            // No need to move
        }
        else if(Mathf.Abs(posDiff.x) > searchRange)
        {
            // Wait
        }
        else // Moving
        {
            // Move to player
            rb2.linearVelocity = posDiff.normalized * -moveSpeed * deltaTime;

            isMoving = true;
        }

        if(isMoving) rb2.linearVelocity = posDiff.normalized * -moveSpeed * deltaTime;
        else rb2.linearVelocity = Vector2.zero; ;

        if (audioSourceWalking)
        {
            audioSourceWalking.volume = Mathf.Lerp(audioSourceWalking.volume, !isMoving ? 0f : 1f, 0.33f);
        }

        if (animator)
        {
            animator.SetFloat("run", isMoving ? 1f : 0f);
        }
    }

    private void UpdateAttack(float deltaTime)
    {
        if (GameManager.instance && GameManager.instance.InMenu)
        {
            // Wait, player in menu
            return;
        }

        // Attack stages
        if (attackTime < attackCooldown) // Cooldown between attacks
        {
            attackTime += deltaTime;
        }
        else if (targetInRange)
        {
            // Attack
            preAttackTime += deltaTime;

            if (preAttackTime > preAttackCooldown)
            {
                bool canAttack = Attack();

                if (canAttack)
                {
                    preAttackTime = 0f;
                    attackTime = 0f;
                }
            }
        }
    }

    public override void TakeDamages(float value, Vector2 dir)
    {
        base.TakeDamages(value, dir);

        if(attackTime > 0f)
        {
            attackTime -= 0.25f; // Prevent attack just after
        }

        // To store it for down anim
        dir.x = dir.x > 0f ? 1f : -1f;
        dir.y = 0f;
        this.dir = dir;
    }

    public override void Down()
    {
        //base.Down();

        // Not yet
        animator.SetTrigger(HURTED_TRIGGER);
        animator.SetBool(DOWN_TRIGGER, false);

        startPos = rb2.position;
        destroyed = true;
    }
}
