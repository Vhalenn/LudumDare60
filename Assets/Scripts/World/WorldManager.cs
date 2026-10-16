using System.Collections.Generic;

using UnityEngine;

using Sirenix.OdinInspector;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private GameManager game; public GameManager Game => game;
    [SerializeField] private float currentTime;
#if UNITY_EDITOR
    [SerializeField] private bool accelerateTimeForDebug;
#endif

    [Header("Main Elements")]
    [SerializeField] private Player player; public Player Player => player;

    //[Header("Content")]
    //[SerializeField] private WorldWaypoint[] waypointArray;
    //[SerializeField] private WorldRoad[] roadArray;


    //[Header("Storage")]

    private void Start()
    {

    }

    private void Update()
    {
        
    }

    public void WinGame() // VICTORY !!!
    {
        Debug.Log("YOU WIN THE GAME!!!");
        game.CanvasManager.endScreen.ShowWinScreen();
    }

    public void LooseGame() // Loose
    {
        Debug.Log($"You Lost the game");
        game.CanvasManager.endScreen.ShowLooseScreen();
    }

    /*
    // Editor
    [Button]
    private void GetAllWaypoints()
    {
        waypointArray = GetComponentsInChildren<WorldWaypoint>();

        int length = waypointArray.Length;
        for (int i = 0; i < length; i++)
        {
            waypointArray[i].World = this;
            waypointArray[i].WaypointId = i;
        }

        // Road
        roadArray = GetComponentsInChildren<WorldRoad>();

        length = roadArray.Length;
        for (int i = 0; i < length; i++)
        {
            roadArray[i].World = this;
        }
    }
    */

}
