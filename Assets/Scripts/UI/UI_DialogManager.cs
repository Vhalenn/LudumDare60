using UnityEngine;
using UnityEngine.UI;

using TMPro;
using Sirenix.OdinInspector;

public class UI_DialogManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("Elements")]
    [SerializeField] private TextAnimatorControl textAnimator;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private Image speakerSprite;

    [Header("Storage")]
    [SerializeField] private DialogData currentDial;
    [SerializeField] private int currentIndex;

    void Start()
    {
        HideDialog();
    }

    public void StartDialog(DialogData dialog)
    {
        currentDial = dialog;

        if (dialog != null) PlayDialog();
    }

    public void ProgresInDialog()
    {
        if (_canvasGroup && _canvasGroup.alpha < 0.5f) return; // Hidden

        currentIndex++;

        DisplayBubble();
    }

    [Button, ButtonGroup("Dialog")]
    public void PlayDialog()
    {
        _canvasGroup.alpha = 1f;
        currentIndex = 0;

        DisplayBubble();
    }

    private void DisplayBubble()
    {
        if (currentDial.array.Length <= currentIndex)
        {
            HideDialog();
            return;
        }

        var currentBubble = currentDial.array[currentIndex];

        // <size=125%><smallcaps>Teacher:</smallcaps></size> How can you do this to our childs?! Arrêtez !!!

        string text = string.Empty;
        text += $"     <size=125%><smallcaps>{currentBubble.speakerNameFr}</smallcaps></size>: ";
        text += currentBubble.dialogFr;

        speakerSprite.enabled = currentBubble.sprite != null;
        speakerSprite.sprite = currentBubble.sprite;

        textAnimator.ShowText(text);
    }

    [Button, ButtonGroup("Dialog")]
    public void HideDialog()
    {
        _canvasGroup.alpha = 0f;
        currentIndex = 0;
    }

}
