using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameOptionsMenu : MonoBehaviour
{
    //Variablen für UserInterface
    public Toggle showIterationsToggle;
    public Toggle showColorToggle;
    public Toggle exitOnGoalToggle;

    //variablen für Pathfinder
    public static bool showIterations = true;
    public static bool showColor = true;
    public static bool exitOnGoal = true;

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

    //Iteration Speed
    public void IterationSpeedSlider()
    {
        Debug.Log("Value changed on IterationSpeedSlider");
    }

    //Algorithms Dropdown
    public void AlgorithmsDropdown()
    {

    }
}
