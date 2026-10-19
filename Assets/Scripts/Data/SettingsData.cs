using UnityEngine;

[CreateAssetMenu]
public class SettingsData : ScriptableObject
{
    public bool disableCRTEffect;

    // Translation
    public bool languageFr;

    private string SHAER_FEATURE = "DISABLE_OFF";
}
