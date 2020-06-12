using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Hauptscene laden
    public void LoadMainScene()
    {
        SceneManager.LoadScene(1);
    }

    //HowItWorks laden
    public void LoadHowItWorks()
    {
    }

    //ToolSettings laden
    public void LoadToolSettings()
    {
        SceneManager.LoadScene(0);
    }

    //App beenden
    public void QuitApp()
    {
        Debug.Log("Wird beendet!");
        Application.Quit();
    }
}