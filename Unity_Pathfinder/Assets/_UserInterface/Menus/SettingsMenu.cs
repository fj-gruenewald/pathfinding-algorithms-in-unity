using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    //Qualitätseinstellungen
    //Quality Settings
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    //Vollbild
    //Fullscreen
    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}