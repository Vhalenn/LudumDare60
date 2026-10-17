using UnityEngine;
using Sirenix.OdinInspector;

public class Enemy : MovingEntity
{
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

    protected override void Init()
    {
        base.Init();

        if(worldManager)
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

        if (!active) return;

        if(animator.gameObject.TryGetComponent(out SpriteRenderer sp))
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

        if (animState != AnimState.Still)
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

        if (animator)
        {
            animator.SetFloat("run", isMoving ? 1f : 0f);
        }
    }

    private void UpdateAttack(float deltaTime)
    {
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
    }
}
