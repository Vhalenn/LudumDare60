using UnityEngine;

public interface IDamageReceiver
{
    public Transform damageTransform { get; }

    public void TakeDamages(float value, Vector2 dir)
    {

    }
}
