using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Variable für Übergangseffekt
    //Transition Effects
    public Animator transition;

    //Hauptscene laden
    //load Main Scene
    public void LoadMainScene()
    {
        SceneManager.LoadScene(1);
    }

    //App beenden
    //Close Application
    public void QuitApp()
    {
        Debug.Log("Wird beendet!");
        Application.Quit();
    }

    //Übergange von Scene zu Scene
    //transition scene to scene
    private IEnumerator LoadScene(int levelIndex)
    {
        //play animation
        transition.SetTrigger("Start");

        //wait
        yield return new WaitForSeconds(1);

        //load scene
        SceneManager.LoadScene(1);
    }
}