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

    [Button,ButtonGroup("State")]
    private void ShowText()
    {
        //textAnimator.SetText("<wave>hello");

        if (typeWritter)
        {
            typeWritter.StartShowingText();
        }
    }

    [Button, ButtonGroup("State")]
    private void HideText()
    {
        if (typeWritter)
        {
            typeWritter.StartDisappearingText();
            typeWritter.SkipTypewriter();
        }
    }
}
