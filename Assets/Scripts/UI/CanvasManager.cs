using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameManager game;

    [Header("Elements")]
    public Canvas mainCanvas;
    public Canvas pauseCanvas;
    public UI_EndScreen endScreen;
    public UI_DialogManager dialogManager;


    private void Start()
    {
        if (pauseCanvas)
        {
            pauseCanvas.enabled = false;
        }
    }

    public void Play() // Load scene if not in menu
    {
        GameManager.instance.LoadLevel(value:0);
    }

    public void LoadManiMenu() // Load scene if not in menu
    {
        GameManager.instance.LoadMainMenu();
    }

    public void QuitGame()
    {
        CoreUtility.QuitGame();
    }

    public void PauseMenuChangeState()
    {
        if(pauseCanvas)
        {
            pauseCanvas.enabled = !pauseCanvas.enabled;
        }

        if(game)
        {
            game.InMenu = pauseCanvas.enabled;
        }
    }

    public void Options()
    {
        
    }

    #region Dialog

    public void StartDialog(DialogData dialog)
    {
        if (!dialogManager)
        {
            Debug.LogError("No dialog manager referenced!");
            return;
        }

        dialogManager.StartDialog(dialog);

    }

    public void ProgresInDialog()
    {
        if (!dialogManager)
        {
            Debug.LogError("No dialog manager referenced!");
            return;
        }

        dialogManager.ProgresInDialog();
    }

    #endregion Dialog
}
