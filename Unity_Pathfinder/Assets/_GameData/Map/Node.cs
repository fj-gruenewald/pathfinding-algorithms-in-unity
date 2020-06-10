using System.Collections.Generic;
using UnityEngine;

//kann gelaufen werden oder ist der weg blockiert
public enum NodeType
{
    Open = 0,
    Blocked = 1
}

public class Node
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

    //
    public Node previous = null;

    //
    public Node(int xIndex, int yIndex, NodeType nodeType)
    {
        this.xIndex = xIndex;
        this.yIndex = yIndex;
        this.nodeType = nodeType;
    }

    //Reset für neuen Versuch
    public void Reset()
    {
        previous = null;
    }
}