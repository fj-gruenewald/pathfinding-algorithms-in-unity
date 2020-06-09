using System.Collections.Generic;
using UnityEngine;

// enum --> kann gelaufen werden oder ist der weg blockiert
public enum NodeType
{
    Open = 0,
    Blocked = 1
}

public class Node
{
    //
    public NodeType nodeType = NodeType.Open;

    //wenn kein index gesetzt wurde
    public int xIndex = -1;

    public int yIndex = -1;

    //Position von Nodes
    public Vector3 position;

    //Nachbarn
    public List<Node> neighbors = new List<Node>();

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