using UnityEngine;

public class ArcadeControls : MonoBehaviour
{
    [Header("Compnents")]
    [SerializeField] private Player player;

    [Header("Elements")]
    [SerializeField] private Transform joystick;
    [SerializeField] private Transform btnA;
    [SerializeField] private Transform btnB;
    [SerializeField] private Transform btnC;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
        Vector2 control = player.controlInput;

        Vector3 joyDir = Vector3.zero;
        joyDir.x = control.y * 40;
        joyDir.z = control.x * -40;

        joystick.localRotation = Quaternion.Lerp(joystick.localRotation, Quaternion.Euler(joyDir), 0.2f);

        RelaxBtn(btnA);
        RelaxBtn(btnB);
        RelaxBtn(btnC);
    }

    public void BtnPressed(int index)
    {
        Transform target;
        if (index == 0) target = btnA;
        else if (index == 1) target = btnB;
        else target = btnC;

        if (target == null) return;

        Vector3 locPos = target.localPosition;
        target.localPosition = new Vector3(locPos.x, -0.5f, locPos.z);

    }

    private void RelaxBtn(Transform target)
    {
        Vector3 locPos = target.localPosition;
        target.localPosition = Vector3.Lerp(locPos, new Vector3(locPos.x, 0f, locPos.z), 0.2f);
    }
}
