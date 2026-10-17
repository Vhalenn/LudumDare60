using UnityEngine;

public class MovingEntity : MonoBehaviour
{
    public Rigidbody rb;
    public Rigidbody2D rb2;

    [Header("Move Alterations")]
    public bool boosted;
    public bool stuned;

    public float boostTime;
    public float stunTime;
}
