using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropdownScript : MonoBehaviour
{
    //Output Text
    public TextMeshProUGUI output = new TextMeshProUGUI();

    private bool showIterations;
    private bool showColors;
    private bool exitOnGoal;

    // Start is called before the first frame update
    public void HandleInputData(int value)
    {
        //Breadth First Search
        if (value == 0)
        {
            
        }

        //Dijkstra
        if (value == 1)
        {

        }

        //Greedy Best First
        if (value == 2)
        {

        }

        //Greedy Best First Manhattan
        if (value == 3)
        {

        }

        //A* Search
        if (value == 4)
        {

        }

        //A* Search Manhattan
        if (value == 5)
        {

        }
    }
}
