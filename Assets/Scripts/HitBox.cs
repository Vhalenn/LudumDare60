using UnityEngine;
using UnityEngine.Events;

using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class HitBox : SerializedMonoBehaviour
{
    [OdinSerialize] private IDamageReceiver receiver;
    public static float VERTICAL_RANGE = 0.4f;

    [Header("Parameters")]
    [SerializeField] private bool hurtBox; // Takes damage
    [SerializeField, ShowIf(nameof(hurtBox))] private UnityEvent onHit;

    [SerializeField] private bool hitBox; // Does damage
    [SerializeField, ShowIf(nameof(hitBox))] private float damageInflicted;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log($"{transform.name} -> Triggered by {other.name}");

        if (hurtBox)
        {
            // Nothing to do?
            // Heal only?

            return;
        }

        if (hitBox) // Inflict damages
        {
            if(other.TryGetComponent(out HitBox hitBox))
            {
                if (!hitBox.hurtBox) return; // Nothing to do

                if(hitBox.onHit != null)
                {
                    hitBox.onHit?.Invoke();
                }

                if (receiver == null)
                {
                    Debug.LogError($"{transform.name} parent is NULL!");
                    return;
                }

                if(hitBox.receiver == null)
                {
                    Debug.LogError($"{hitBox.transform.name} parent is NULL!");
                    return;
                }

                if(hitBox.hurtBox)
                {
                    // Compare Y pos

                    Vector2 dir = hitBox.receiver.damageTransform.position - receiver.damageTransform.position;
                    dir.y = Mathf.Abs(dir.y);

                    //Debug.Log($"{parent.name} > {hitBox.parent.name} - YDist: {dir.y}");

                    if(dir.y > VERTICAL_RANGE)
                    {
                        // Too far
                        return;
                    }

                    // Do damage
                    hitBox.receiver.TakeDamages(damageInflicted, dir);
                }

            }
        }
    }
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"{transform.name} -> Colided by {collision.transform.name}");
    }
    */
}
