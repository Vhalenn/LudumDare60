using UnityEngine;

public class WorldProps : MonoBehaviour, IDamageReceiver
{
    public Transform damageTransform => transform;
    [SerializeField] private Collider2D col;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer sR;
    [SerializeField] private Sprite damagedSprite;
    [SerializeField] private AnimationCurve posXCurve;
    [SerializeField] private AnimationCurve posYCurve;

    [Header("Storage")]
    [SerializeField] private bool destroyed;
    [SerializeField] private float destroyedTime;
    [SerializeField] private bool destroyedEndAnim;
    [SerializeField] private float randomForce = 0.5f;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 dir;
    [SerializeField] Rigidbody2D rb;

    private void Update()
    {
        if (destroyedEndAnim) return;

        if(destroyed)
        {
            destroyedTime += Time.deltaTime;
            rb.position = startPos + (Vector3.up * posYCurve.Evaluate(destroyedTime) * randomForce) + (Vector3.right * dir.x * posXCurve.Evaluate(destroyedTime) * randomForce);

            if (destroyedTime > 1.5f)
            {
                if (col) col.enabled = false;
                destroyedEndAnim = true; // END
            }
        }
    }

    public virtual void TakeDamages(float value, Vector2 dir)
    {
        destroyed = true;

        if (sR && damagedSprite)
        {
            sR.sprite = damagedSprite;
        }

        randomForce = Random.Range(0.25f, 1.25f);

        if(!rb) rb = gameObject.AddComponent<Rigidbody2D>();

        if(rb)
        {
            startPos = rb.position;
            rb.mass = 1000;
            rb.gravityScale = 0f;

            rb.angularDamping = 5f;
            rb.linearDamping = 0.5f;

            //dir = dir.normalized;
            dir.x = dir.x > 0f ? 1f : -1f;
            dir.y = 0f;
            this.dir = dir;


            //rb.linearVelocity = dir * 150f;
            rb.angularVelocity = Random.value * -360f * dir.x * randomForce;
        }
    }
}
