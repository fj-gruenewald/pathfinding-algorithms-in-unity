using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    //Suchvariablen
    public bool isComplete = false;
    int m_iterations = 0;

    //Suche durchführen
    public void Init(Graph graph, GraphView graphView, Node start, Node end)
    {
        //nichts darf fehlen
        if (start == null || endColor == null || graph == null || graphView == null)
        {
            Debug.LogWarning("Irgendetwas stimmt mit Hasi nicht! pathfinder_Init_component-missing");
            return;
        }

        //start und endpunkt dürfen nicht in wänden liegen
        if (start.nodeType == NodeType.Blocked || end.nodeType == NodeType.Blocked)
        {
            Debug.LogWarning("So ein Feuerball Junge! pathfinder_Init_start/end-blocked");
            return;
        }

        //variablen ablegen
        m_graph = graph;
        m_graphView = graphView;
        m_startNode = start;
        m_endNode = end;

        //Färben
        ShowColors(graphView, start, end);

        //initialisierung
        m_frontierNodes = new Queue<Node>();
        m_frontierNodes.Enqueue(start);
        m_exploredNodes = new List<Node>();
        m_pathNodes = new List<Node>();

        //Durch alle Knoten gehen
        for (int x = 0; x < m_graph.Width; x++)
        {
            //Durch alle Knoten gehen
            for (int y = 0; y < m_graph.Height; y++)
            {
                //Zurück zum Anfang 
                m_graph.nodes[x, y].Reset();
            }
        }

        //Informationen über den Verlauf der Suche zurücksetzten
        isComplete = false;
        m_iterations = 0;

    }

    //Überladene ShowColors
    void ShowColors()
    {
        ShowColors(m_graphView, m_startNode, m_endNode);
    }

    //Knoten färben
    void ShowColors(GraphView graphView, Node start, Node end)
    {
        //Crash durch fehlende Werte vermeiden
        if(graphView == null || start == null || end == null)
        {
            return;
        }

        //Färben der Knoten die verarbeitet werden
        if(m_frontierNodes != null)
        {
            graphView.ColorNodes(m_frontierNodes.ToList(), frontierColor);
        }

        //färben der abgearbeiteteten Knoten
        if(m_exploredNodes != null)
        {
            graphView.ColorNodes(m_exploredNodes, exploredColor);
        }

        //Start und Endpunkt Färben
        NodeView startNodeView = graphView.nodeViews[start.xIndex, start.yIndex];

        if (startNodeView != null)
        {
            startNodeView.ColorNode(startColor);
        }

        //Endpunkt färben
        NodeView endNodeView = graphView.nodeViews[end.xIndex, end.yIndex];
        if (endNodeView != null)
        {
            endNodeView.ColorNode(endColor);
        }
    }

    //Suchalgorithmus
    public IEnumerator SearchRoutine(float timeStep = 0.1f)
    {
        yield return null;

        //durchführen der Suche
        while(!isComplete)
        {
            if(m_frontierNodes.Count > 0)
            {
                //Knoten aus "nächste Knoten" liste entfernen
                Node currentNode = m_frontierNodes.Dequeue();
                m_iterations++;

                //Knoten zu "besuchte Knoten" liste hinzufügen
                if(!m_exploredNodes.Contains(currentNode))
                {
                    m_exploredNodes.Add(currentNode);
                }

                //Verarbeitung der Nachbarn
                ExpandFrontier(currentNode);

                //Färben der Knoten
                ShowColors();

                //Anzeigen der Pfeile
                if(m_graphView != null)
                {
                    m_graphView.ShowNodeArrows(m_frontierNodes.ToList());
                }

                //
                yield return new WaitForSeconds(timeStep);

            }
            else
            {
                //Wenn alles durchlaufen wurde
                isComplete = true;

            }
        }
    }

    //Methode zum finden des Weges
    void ExpandFrontier(Node node)
    {
        //Crash vermeiden
        if(node != null)
        {
            //alle nachbarn des Knoten abarbeiten
            for(int i = 0; i < node.neighbors.Count; i++)
            {
                //Nur NEUE Nachbarn verarbeiten nicht bereits verarbeitete Knoten!
                if (!m_exploredNodes.Contains(node.neighbors[i]) && !m_frontierNodes.Contains(node.neighbors[i]))
                {
                    //Informationen über Vorgängerknoten Speichern
                    node.neighbors[i].previous = node;
                    m_frontierNodes.Enqueue(node.neighbors[i]);
                }
            }
        }
    }
}
