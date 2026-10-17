using UnityEngine;

using Sirenix.OdinInspector;

[CreateAssetMenu]
public class DialogData : ScriptableObject
{
    public DialogBubble[] array;

    public void LoadLevel(int index)
    {
        //SceneLoader.LoadScene(list[index].sceneName);
    }
}

[System.Serializable]
public struct DialogBubble
{
    public string speakerNameFr;
    public string speakerNameEn;
    [TextArea] public string dialogFr;
    [TextArea] public string dialogEn;
    [PreviewField(100, ObjectFieldAlignment.Left), LabelWidth(50)] public Sprite sprite;
}
