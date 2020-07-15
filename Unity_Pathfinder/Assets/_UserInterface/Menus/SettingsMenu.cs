using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    //Qualitätseinstellungen
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    //Vollbild
    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}