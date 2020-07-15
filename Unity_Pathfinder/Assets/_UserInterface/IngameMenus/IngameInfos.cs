using System;
using TMPro;
using UnityEngine;

public class IngameInfos : MonoBehaviour
{
    //Variablen
    public TextMeshProUGUI timeText;

    public TextMeshProUGUI nodeText;

    //Time in textMeshPro setzten
    public void SetSearchTime(string time)
    {
        Debug.Log("Content of the Time Info: " + time);
        try
        {
            timeText.text = time + " Seconds";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    //Node in textMeshPro setzten
    public void SetSearchNodes(string nodes)
    {
        Debug.Log("Content of the Node Info:" + nodes);
        try
        {
            nodeText.text = nodes + " Nodes";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}