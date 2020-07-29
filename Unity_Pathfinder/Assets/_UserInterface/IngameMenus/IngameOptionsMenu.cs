using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameOptionsMenu : MonoBehaviour
{
    //Variablen für UserInterface (Toggles)
    //variables for the user interface toggles
    public Toggle showIterationsToggle;

    public Toggle showColorToggle;
    public Toggle exitOnGoalToggle;

    //Variablen für UserInterface (Slider)
    //variables for the user interface slider
    public Slider iterationSpeedSlider;

    //Variablen für UserInterface (Dropdown)
    //variables for the user interface dropdown
    public TMP_Dropdown algorithmDropdown;

    public static Mode searchMode = Mode.BreadthFirstSearch;

    public enum Mode
    {
        BreadthFirstSearch = 0,
        DijkstraAlgorithm = 1,
        GreedyBestFirstSearch = 2,
        AStarSearch = 3,
    }

    //Variablen für Pathfinder (Visualisierung)
    //variables for Pathginder visualization
    public static bool showIterations = true;

    public static bool showColor = true;
    public static bool exitOnGoal = true;

    //Variablen für GameController (Speed)
    //variables for GameController 
    public static float timeStep;

    public bool GetShowIterations()
    {
        return showIterations;
    }

    public bool GetShowColor()
    {
        return showColor;
    }

    public bool GetExitOnGoal()
    {
        return exitOnGoal;
    }

    public float GetTimeStep()
    {
        return timeStep;
    }

    public Mode GetSearchMode()
    {
        return searchMode;
    }

    //ShowIterations
    public void ShowIterationToggle()
    {
        if (showIterationsToggle.isOn)
        {
            Debug.Log("ShowIterationToggle enabled");
            showIterations = true;
        }
        else
        {
            Debug.Log("ShowIterationToggle disabled");
            showIterations = false;
        }
        Debug.Log("Value changed on ShowIterationsToggle");
    }

    //ShowColors
    public void ShowColorsToggle()
    {
        if (showColorToggle.isOn)
        {
            Debug.Log("ShowColorsToggle enabled");
            showColor = true;
        }
        else
        {
            Debug.Log("ShowColorsToggle disabled");
            showColor = false;
        }
        Debug.Log("Value changed on ShowColorsToggle");
    }

    //exitOnGoal
    public void ExitOnGoalToggle()
    {
        if (exitOnGoalToggle.isOn)
        {
            Debug.Log("ExitOnGoalToggle enabled");
            exitOnGoal = true;
        }
        else
        {
            Debug.Log("ExitOnGoalToggle disabled");
            exitOnGoal = false;
        }
        Debug.Log("Value changed on ExitOnGoalToggle");
    }

    //Update Methode zum prüfen von Slider
    //update method for slider
    private void Update()
    {
        //Timestep für Geschwindigkeit von Iterationsschritten
        //timestep for the speed of the search algorithms
        timeStep = iterationSpeedSlider.value;
        Debug.Log(timeStep);

        //Prüfen des Dropdown für Mode des Algorithmus
        //check dropdown for search mode
        if (algorithmDropdown.value == 0)
        {
            Debug.Log("Mode 0");
            searchMode = Mode.BreadthFirstSearch;
        }
        if (algorithmDropdown.value == 1)
        {
            Debug.Log("Mode 1");
            searchMode = Mode.GreedyBestFirstSearch;
        }
        if (algorithmDropdown.value == 2)
        {
            Debug.Log("Mode 2");
            searchMode = Mode.DijkstraAlgorithm;
        }
        if (algorithmDropdown.value == 3)
        {
            Debug.Log("Mode 3");
            searchMode = Mode.AStarSearch;
        }
    }
}