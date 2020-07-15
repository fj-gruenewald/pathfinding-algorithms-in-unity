using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameFrame : MonoBehaviour
{
    //Variablen
    public GameObject optionsMenu;

    private int buttonCounter;

    //Main Menu
    public void LoadMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    //Reset
    public void ResetCurrentScene()
    {
        SceneManager.LoadScene(1);
    }

    //Options
    public void IngameOptionsVisibility()
    {
        //1. Klick öffnen 2ter schließt menü
        buttonCounter++;
        if (buttonCounter % 2 == 1)
        {
            optionsMenu.gameObject.SetActive(false);
        }
        else
        {
            optionsMenu.gameObject.SetActive(true);
        }
    }
}