using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    //Start und Endpunkt
    Node m_startNode;
    Node m_endNode;

    //Referenzen zu Darstellenden Objekten
    Graph m_graph;
    GraphView m_graphView;

    //Queue abzuarbeitender Knoten
    Queue<Node> m_frontierNodes;

    //Bereits besuchte Knoten
    List<Node> m_exploredNodes;

    //Liste des kürzesten weges
    List<Node> m_pathNodes;

    //Farben für den Ablauf
    public Color startColor = Color.green;
    public Color endColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploredColor = Color.gray;
    public Color pathColor = Color.cyan;

    //Suche durchführen
    public void Init(Graph graph, GraphView graphView, Node start, Node end)
    {
        //nichts darf fehlen
        if(start == null || endColor == null || graph == null || graphView == null)
        {
            Debug.LogWarning("Irgendetwas stimmt mit Hasi nicht! pathfinder_Init_component-missing");
            return;
        }

        //start und endpunkt dürfen nicht in wänden liegen
        if(start.nodeType == NodeType.Blocked || end.nodeType == NodeType.Blocked)
        {
            Debug.LogWarning("So ein Feuerball Junge! pathfinder_Init_start/end-blocked");
            return;
        }

        //variablen ablegen
        m_graph = graph;
        m_graphView = graphView;
        m_startNode = start;
        m_endNode = end;

        //Start und Endpunkt Färben
        NodeView startNodeView = graphView.nodeViews[start.xIndex, start.yIndex];

        if(startNodeView != null)
        {
            startNodeView.ColorNode(startColor);
        }

        //Endpunkt färben
        NodeView endNodeView = graphView.nodeViews[end.xIndex, end.yIndex];
        if(endNodeView != null)
        {
            endNodeView.ColorNode(endColor);
        }

        //initialisierung
        m_frontierNodes = new Queue<Node>();
        m_frontierNodes.Enqueue(start);
        m_exploredNodes = new List<Node>();
        m_pathNodes = new List<Node>();

        //Durch alle Knoten gehen
        for(int x = 0; x < m_graph.Width; x++)
        {
            //Durch alle Knoten gehen
            for(int y = 0; y < m_graph.Height; y++)
            {
                //Zurück zum Anfang 
                m_graph.nodes[x, y].Reset();
            }
        }

    }
}
