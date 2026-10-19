using System.Collections.Generic;

using UnityEngine;
using Sirenix.OdinInspector;

public class Player : MovingEntity
{

    [Header("Base Components")]
    public Vector2 controlInput;
    public bool desiredJump;
    [HideInInspector] private Transform camTransform;

    [Header("Stats")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float speedVMultiplier = 1f;
    [SerializeField,Range(0,1f)] private float brakeForce = 0.9f;

    [Header("SafeZone")]
    [SerializeField] private Vector3 lastSafePos;
    [SerializeField] private List<TriggerSafeZone> safeAreaList;

    [Header("Storage")]
    [SerializeField] private Vector3 desiredVelocity;
    [SerializeField] private Vector3 velocity;
    public bool InMenu => worldManager && worldManager.Game && worldManager.Game.InMenu;

    public void Teleport(Vector3 pos)
    {
        rb2.position = pos;
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
        bool isNotMoving = controlInput.magnitude < 0.1f || InMenu || Attacking;

        if (isNotMoving)
        {
            velocity.Scale( new (brakeForce, brakeForce, brakeForce));
        }
        else
        {
            // Should not be addition -> Should be capped
            //velocity = Vector3.Lerp(velocity, FollowingCamAngle(controlInput) * speed, 0.01f);

            desiredVelocity = FollowingCamAngle(controlInput) * speed * Time.fixedDeltaTime;
            desiredVelocity.y *= speedVMultiplier;
            velocity = desiredVelocity;

            if(animator && Mathf.Abs(velocity.x) > 0.1f && !Attacking) // If is moving horizontally
            {
                // Flip
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

    /*
    // In child class
    public void Attack()
    {
        animator.SetTrigger("attack");
        isAttacking = true;
    }
    */


    public void Grab() // Maybe?
    {

    }

    #endregion Attack

    public override void Down()
    {
        worldManager.LooseGame();
    }

    public float GetLifePercentage()
    {
        return life / maxLife;
    }

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
