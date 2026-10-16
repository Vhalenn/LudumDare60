using UnityEngine;

public class SceneMenuSelector : MonoBehaviour
{
    public LevelList levelList;

    public void LoadLevel(int index)
    {
        if(levelList)
        {
            levelList.LoadLevel(index);
        }
        else
        {
            SceneLoader.LoadScene(index);
        }
    }

    public void QuitGame()
    {
        CoreUtility.QuitGame();
    }
}
