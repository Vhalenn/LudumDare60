using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;

using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;
using static UnityEngine.InputSystem.InputAction;
using Febucci.TextAnimatorCore.Typing;

public class UI_DialogManager : MonoBehaviour
{
    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private SettingsData settingsData; // Ref for translation
    [SerializeField] private InputActionReference action;

    [Header("Elements")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextAnimatorControl textAnimator;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private Image speakerSprite;

    [Header("Sounds")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioSource sourceTyping;
    [SerializeField] private AudioClip progresDialogSound;
    [SerializeField] private AudioClip characterDisplayedSound;

    [Header("Evnets")]
    [SerializeField] private UnityEvent endDialogEvent;


    [Header("Storage")]
    [SerializeField] private DialogData currentDial;
    [SerializeField] private DialogBubble currentBubble;
    [SerializeField] private int currentIndex;

    void Start()
    {
        HideDialog();
    }

    private void Update()
    {

        if(action)
        {
            bool performed = action.action.WasPressedThisFrame();

            if (performed) ProgresInDialog();
        }
    }

    public void StartDialog(DialogData dialog)
    {
        Debug.Log("Start dialog!");

        currentDial = dialog;

        if (dialog != null) PlayDialog();
    }

    public void ProgresInDialogFromEvent(CallbackContext context)
    {
        Debug.Log($"Progres In Dialog From Event-> {context}");

        if (context.performed) ProgresInDialog();
    }

    [Button]
    public void ProgresInDialog()
    {
        //Debug.Log("Progres In Dialog");

        if (_canvasGroup && _canvasGroup.alpha < 0.5f) return; // Hidden

        // Check if is typing
        if(textAnimator)
        {
            if(textAnimator.IsTyping())
            {
                // End typing
                textAnimator.EndTyping();
                SetTypingState(false); // End typing sound

                return;
            }
        }

        PlaySound(progresDialogSound);

        // Check if current bubble has an Event
        if (currentBubble.dialEvent != DialogEvent.None)
        {
            // Do event
            if(currentBubble.dialEvent == DialogEvent.WinGame)
            {
                Debug.Log("Event - WIN Game");
                if(canvasManager) canvasManager.game.World.WinGame();
            }
        }

        currentIndex++;

        DisplayBubble();
    }

    [Button, ButtonGroup("Dialog")]
    public void PlayDialog()
    {
        Debug.Log("Play Dialog!");

        _canvasGroup.alpha = 1f;
        currentIndex = 0;

        DisplayBubble();
    }

    private void DisplayBubble()
    {
        if(currentDial == null)
        {
            HideDialog();
            return;
        }

        currentBubble = currentDial.GetBubble(currentIndex, out bool success);

        if (!success)
        {
            HideDialog();
            return;
        }

        GameManager.instance.InMenu = true;
        SetTypingState(true);

        // <size=125%><smallcaps>Teacher:</smallcaps></size> How can you do this to our childs?! Arrêtez !!!

        string text = string.Empty;

        string speakerName = currentBubble.speakerNameEn;
        string dialogTxt = currentBubble.dialogEn;

        if (settingsData && settingsData.languageFr) // Translation
        {
            speakerName = currentBubble.speakerNameFr;
            dialogTxt = currentBubble.dialogFr;
        }

        text += $"     ";
        if (!string.IsNullOrWhiteSpace(speakerName))
        {
            bool hasStyle = currentBubble.HasStyle;
            if (hasStyle) text += $"<style={currentBubble.speakerStyle}>";
            text += $"<size=125%><smallcaps>{speakerName}</smallcaps></size>:\n";
            if (hasStyle) text += "</style>";
        }
        text += dialogTxt;
        text += "<?eventID=end>";

        speakerSprite.enabled = currentBubble.sprite != null;
        speakerSprite.sprite = currentBubble.sprite;

        textAnimator.ShowText(text);
    }

    // HIDE

    [Button, ButtonGroup("Dialog")]
    public void HideDialog()
    {
        if(currentIndex > 0)
        {
            Debug.Log("Hide Dialog, a dialog was in progress!");
            endDialogEvent?.Invoke();
        }
        else Debug.Log("Hide Dialog!");

        _canvasGroup.alpha = 0f;
        currentIndex = 0;
        currentBubble = default;

        SetTypingState(false); // For security

        if (GameManager.instance)
        {
            GameManager.instance.InMenu = false;
        }
    }

    public void StartSceenRevealSequence(CanvasGroup startScreenCanvas)
    {
        if (startScreenCanvas == null) return;

        startScreenCanvas.alpha = 0f;
        startScreenCanvas.gameObject.SetActive(true);

        startScreenCanvas.DOFade(1f, 5f);
    }

    public void SetTypingState(bool state)
    {
        //float levelLoad = Time.timeSinceLevelLoad;
        //if (levelLoad < 0.5f) return; // First unity click;

        //Debug.Log($"Set typing state: {state} - {levelLoad} ms");

        if(sourceTyping)
        {
            if (state)
            {
                sourceTyping.Play();
                sourceTyping.mute = false;
                sourceTyping.volume = 0.8f;
            }
            else
            {
                //sourceTyping.Stop();
                sourceTyping.volume = 0f;
            }
        }
    }

    public void OnMessage(EventMarker eM)
    {
        Debug.Log($"Received message: {eM.name}");

        SetTypingState(false);
    }

    public void CharacterDisplayed()
    {
        PlaySound(characterDisplayedSound);
    }

    public void PlaySound(AudioClip clip)
    {
        if(source && clip)
        {
            source.PlayOneShot(clip);
        }
    }

}
