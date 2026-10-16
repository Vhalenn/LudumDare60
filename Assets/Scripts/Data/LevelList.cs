using UnityEngine;

[CreateAssetMenu]
public class LevelList : ScriptableObject
{
    public Level[] list;

    public void LoadLevel(int index)
    {
        SceneLoader.LoadScene(list[index].sceneName);
    }
}

[System.Serializable]
public struct Level
{
    public string name;
    public string sceneName;
    public Sprite sprite;
    public int difficulty;
}
