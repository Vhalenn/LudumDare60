using UnityEngine;
using UnityEngine.InputSystem;

public class ControlManager : MonoBehaviour
{
    //public enum KeyboardType { QWERTY, AZERTY };
    //public KeyboardType keyType;

    //public InputMaster controls;
    [SerializeField] private PlayerInput playerInput;

    [Header("Elements")]
    public GameManager game;
    public CanvasManager canvas;
    public Player player;
    public CameraRotation camRotation;

    [Header("Storage")]
    [SerializeField] private float isGamepad;

    void Start()
    {
        //controls = new InputMaster();
        //controls.Enable();

        playerInput.actions["Jump"].performed += ctx => PlayerJump();
        playerInput.actions["Punch"].performed += ctx => PlayerPunch();

        if(player) // PLAYER
        {
            playerInput.actions["Move"].performed += ctx => player.controlInput = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1f);
            playerInput.actions["Move"].canceled += ctx => player.controlInput = Vector2.zero;

            playerInput.actions["PunchHeavy"].performed += ctx => PlayerHeavyPunch();
        }

        playerInput.actions["Quit"].started += ctx => OnEscape();

        if(camRotation)
        {
            playerInput.actions["Look"].performed += ctx => camRotation.controlInput = ctx.ReadValue<Vector2>();
            playerInput.actions["Look"].canceled += ctx => camRotation.controlInput = Vector2.zero;
        }

        //controls.Menu.Escape.started += _ => TogglePauseMenu();
    }

    private void OnEscape()
    {
        if (game) game.OnEscape();
    }

    private void PlayerJump()
    {
        player.Jump();
        if(game) game.ArcadeControls.BtnPressed(2); // Not clean but works
    }

    private void PlayerPunch()
    {
        player.Attack();

        //if(canvas) canvas.ProgresInDialog(); // Not clean but works
        if(game) game.ArcadeControls.BtnPressed(0); // Not clean but works
    }
    private void PlayerHeavyPunch()
    {
        player.HeavyAttack();

        if (game) game.ArcadeControls.BtnPressed(1); // Not clean but works
    }
}
