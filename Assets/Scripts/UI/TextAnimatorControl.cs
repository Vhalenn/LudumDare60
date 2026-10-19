using UnityEngine;

using TMPro;
using Febucci.TextAnimatorForUnity;
using Febucci.TextAnimatorForUnity.TextMeshPro; // <- import Text Animator's namespace

using Sirenix.OdinInspector;

public class TextAnimatorControl : MonoBehaviour
{
    [SerializeField] TMP_Text textMeshPro;
    [SerializeField] TextAnimator_TMP textAnimator;
    [SerializeField] TypewriterComponent typeWritter;

    void OnEnable()
    {
        ShowText();
    }

    private void OnDisable()
    {
        HideText();
    }

    // States

    public void ShowText(string text)
    {
        if (textAnimator) textAnimator.SetText(text);

        HideText();
        ShowText();
    }

    [Button,ButtonGroup("State")]
    public void ShowText()
    {
        //textAnimator.SetText("<wave>hello");

        if (typeWritter)
        {
            typeWritter.StartShowingText();
        }
    }

    [Button, ButtonGroup("State")]
    public void HideText()
    {
        if (typeWritter)
        {
            typeWritter.StartDisappearingText();
            typeWritter.SkipTypewriter();
        }
    }

    // Utility
    public void EndTyping()
    {
        if (typeWritter)
        {
            typeWritter.SkipTypewriter();
        }
    }

    [Button]
    private void IsTypingDebug()
    {
        Debug.Log($"IsTyping() -> {IsTyping()}");
    }

    public bool IsTyping()
    {
        if (typeWritter)
        {
            return typeWritter.IsShowingText;
        }

        return false;
    }
}
