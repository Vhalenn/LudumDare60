using UnityEngine;
using UnityEngine.Events;

public class UI_LanguageSel : MonoBehaviour
{
    public SettingsData settings;
    [SerializeField] private UnityEvent eventToCall;

    public void SetLanguage(bool french)
    {
        settings.languageFr = french;

        Debug.Log($"Langage setted! fr: {french}");

        // If needs to call
        eventToCall?.Invoke();
    }
}
