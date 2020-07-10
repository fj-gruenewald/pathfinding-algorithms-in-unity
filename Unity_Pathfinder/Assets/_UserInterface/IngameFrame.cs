using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class IngameFrame : MonoBehaviour
{
    //Main Menu Scene
    public void LoadMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    //Reset
    public void ResetCurrentScene()
    {
        SceneManager.LoadScene(1);
    }
}
