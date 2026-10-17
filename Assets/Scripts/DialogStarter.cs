using UnityEngine;

public class DialogStarter : MonoBehaviour
{
    [SerializeField] private bool used;
    [SerializeField] private WorldManager worldManager;

    [Header("Data")]
    [SerializeField] private DialogData dialog;

    public void StartDialog()
    {
        if (used) return;

        used = true;

        Debug.Log("Start dialog");

        if(worldManager)
        {
            worldManager.Game.CanvasManager.StartDialog(dialog);
        }

    }
}
