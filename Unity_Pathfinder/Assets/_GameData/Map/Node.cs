using System;
using System.Collections.Generic;
using UnityEngine;

//kann gelaufen werden oder ist der weg blockiert
public enum NodeType
{
    Open = 0,
    Blocked = 1
}

public class Node: IComparable<Node>
{
    //Zeigt das ein Knoten begehbar ist
    public NodeType nodeType = NodeType.Open;

    //wenn kein index gesetzt wurde
    public int xIndex = -1;
    public int yIndex = -1;

    //Position von Nodes
    public Vector3 position;

    //Liste der Nachbarn
    public List<Node> neighbors = new List<Node>();

    //Die Kosten der gelaufenen Kanten
    public float distanceTraveled = Mathf.Infinity;

    //Vorhergegangenen Knoten auf null setzten
    public Node previous = null;

    //Variable zum Vergleichen von 2 Knoten
    public int priority;

    //
    public Node(int xIndex, int yIndex, NodeType nodeType)
    {
        this.xIndex = xIndex;
        this.yIndex = yIndex;
        this.nodeType = nodeType;
    }

    //Schnittstelle für die PriorityQueue. Info: Min Queue --> kleine Werte wandern an den Anfang
    //node.CompareTo(otherNode); -1, 1, 0
    public int CompareTo(Node other)
    {
        //Wenn priority von Knoten 1 höher als Knoten 2
        if(this.priority < other.priority)
        {
            return -1;
        }
        else if(this.priority > other.priority)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    //Reset für neuen Versuch
    public void Reset()
    {
        previous = null;
    }
}