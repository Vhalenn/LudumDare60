using UnityEngine;
using Core.Utilities;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private LevelList levelList;
    [SerializeField] private bool mainMenu;
    [SerializeField] private bool inMenu; public bool InMenu { get => inMenu; set => inMenu = value; }

    [Header("Elements")]
    [SerializeField] private WorldManager world; public WorldManager World => world;
    [SerializeField] private CanvasManager canvasManager; public CanvasManager CanvasManager => canvasManager;
    [SerializeField] private ArcadeControls arcadeControls; public ArcadeControls ArcadeControls => arcadeControls;

    public void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void LoadLevel(int value)
    {
        Debug.Log("Load level");
        if(!levelList)
        {
            Debug.LogError("No level-list !!!");
            return;
        }

        SceneLoader.LoadScene(levelList.list[value].sceneName);
    }

    public void LoadMainMenu()
    {
        SceneLoader.LoadScene("MainMenu");
    }

    public void PlayerActionPressed()
    {

    }

    public void OnEscape()
    {
        Debug.Log("Pressed Escape");
        if (mainMenu) return;

        if(canvasManager)
        {
            canvasManager.PauseMenuChangeState();
        }
    }
}
