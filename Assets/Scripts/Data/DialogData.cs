using UnityEngine;

using Sirenix.OdinInspector;

[CreateAssetMenu]
public class DialogData : ScriptableObject
{
    public DialogBubble[] array;

    public DialogBubble GetBubble(int index, out bool success)
    {
        success = (array != null && index >= 0 && array.Length > index);

        if (success) return array[index];
        else return default;
    }
}

[System.Serializable]
public struct DialogBubble
{
    public string speakerNameFr;
    public string speakerNameEn;
    public string speakerStyle;
    public bool HasStyle => !string.IsNullOrWhiteSpace(speakerStyle);

    [Title("Text")]
    [PreviewField(100, ObjectFieldAlignment.Left), LabelWidth(50)] public Sprite sprite;
    [TextArea] public string dialogFr;
    [TextArea] public string dialogEn;
    public DialogEvent dialEvent;
}

public enum DialogEvent
{
    None,
    ShakeScreen, // Maybe?
    WinGame,
    TpToNextLevel,
}
