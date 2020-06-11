using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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

    //Queue abzuarbeitender Knoten / Update: zur PriorityQueue
    PriorityQueue<Node> m_frontierNodes;

    //Bereits besuchte Knoten
    List<Node> m_exploredNodes;

    //Liste des kürzesten weges
    List<Node> m_pathNodes;

    //Farben für den Ablauf info: Color32 für normale RGB Werte Bsp. Color32(216, 216, 216, 255) , Color für float Werte Bsp. Color(0.85f, 0.85f, 0.85f, 1f)
    public Color startColor = Color.green;
    public Color endColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploredColor = Color.gray;
    public Color pathColor = new Color(38f, 114f, 76f, 1f);

    //Visualisierung steuern
    public bool showIterations = true;
    public bool showColor = true;
    public bool exitOnGoal = true;

    //Suchvariablen
    public bool isComplete = false;
    int m_iterations = 0;

    //Suchmodus wählen
    public enum Mode
    {
        BreadthFirstSearch = 0,
        DepthFirstSearch = 1,
        DijkstraAlgorithm = 2,
        GreedyBestFirstSearch = 3,
        GreedyBestFirstSearchManhattan = 4 ,
        AStarSearch = 5,
        AStarSearchManhattan = 6
    }

    //Standard Suchmodus
    public Mode mode = Mode.BreadthFirstSearch;

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
        m_frontierNodes = new PriorityQueue<Node>();
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
        m_startNode.distanceTraveled = 0;

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

        //Färben der Knoten des gefundenen Weges
        if(m_pathNodes != null && m_pathNodes.Count > 0)
        {
            graphView.ColorNodes(m_pathNodes, pathColor);
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
        //Beginn der Zeitmessung für die Anzeige
        float timeStart = Time.time;

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

                //Durchführen der Suchalgorithmen
                if(mode == Mode.BreadthFirstSearch)
                {
                    ExpandFrontierBreadthFirstSearch(currentNode);
                }
                else if(mode == Mode.DijkstraAlgorithm)
                {
                    ExpandFrontierDijkstraAlgorithm(currentNode);
                }
                else if (mode == Mode.GreedyBestFirstSearch)
                {
                    ExpandFrontierGreedyBestFirstSearch(currentNode);
                }
                else if (mode == Mode.AStarSearch)
                {
                    ExpandFrontierAStarSearch(currentNode);
                }

                //Wenn es ein enpunkt gibt
                if (m_frontierNodes.Contains(m_endNode))
                {
                    //Knoten rückwärts in pathnodes übergeben
                    m_pathNodes = GetPathNodes(m_endNode);

                    //Suche beenden wenn Ziel gefunden
                    if(exitOnGoal)
                    {
                        isComplete = true;

                        //Ausgabe: wie lang ist der gefundene Weg
                        Debug.Log("Der Suchalgorithmus: " + mode.ToString() + " fand einen Weg der länge: " + m_endNode.distanceTraveled.ToString());
                    }
                }

                //gesamte Visualisierung nur Anzeigen wenn gewollt
                if (showIterations)
                {
                    //Visualisierungen verwalten
                    ShowVisualization();

                    //
                    yield return new WaitForSeconds(timeStep);
                }

            }
            else
            {
                //Wenn alles durchlaufen wurde
                isComplete = true;

            }
        }

        //Anzeigefehler vermeiden
        ShowVisualization();

        //Ausgabe: Ende der Zeitmessung für die Anzeige
        Debug.Log("Zeit bis zur Terminierung = " + (Time.time - timeStart).ToString() + " Sekunden");

    }

    //Methode zum verwalten der Visualisierungen
    private void ShowVisualization()
    {
        //Färbung nur Anzeigen wenn gewollt
        if (showColor)
        {
            //Färben der Knoten
            ShowColors();
        }
    }

    //Methode zum finden des Weges !!Breitensuche!!
    void ExpandFrontierBreadthFirstSearch(Node node)
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
                    //Distanz von Aktuellem Knoten zu den Nachbarn
                    float distanceToNeighbor = m_graph.GetNodeDistance(node, node.neighbors[i]);
                    float newDistanceTraveled = distanceToNeighbor + node.distanceTraveled;

                    //Menge der durchlaufenen Knoten
                    node.neighbors[i].distanceTraveled = newDistanceTraveled;

                    //Informationen über Vorgängerknoten Speichern
                    node.neighbors[i].previous = node;

                    //Fix für die Breitensuche
                    node.neighbors[i].priority = m_exploredNodes.Count;

                    //Neuen Knoten einsetzten
                    m_frontierNodes.Enqueue(node.neighbors[i]);
                }
            }
        }
    }

    //Methode zum finden des Weges !!Dijkstra!!
    void ExpandFrontierDijkstraAlgorithm(Node node)
    {
        //Crash vermeiden
        if (node != null)
        {
            //alle nachbarn des Knoten abarbeiten
            for (int i = 0; i < node.neighbors.Count; i++)
            {
                //Nur NEUE Nachbarn verarbeiten nicht bereits verarbeitete Knoten!
                if (!m_exploredNodes.Contains(node.neighbors[i]))
                {
                    //Distanz von Aktuellem Knoten zu den Nachbarn
                    float distanceToNeighbor = m_graph.GetNodeDistance(node, node.neighbors[i]);
                    float newDistanceTraveled = distanceToNeighbor + node.distanceTraveled;

                    //Wenn der Weg länger ist als ein bereits gefundener anderer Weg
                    if (float.IsPositiveInfinity(node.neighbors[i].distanceTraveled) || newDistanceTraveled < node.neighbors[i].distanceTraveled)
                    {
                        //Informationen über Vorgängerknoten Speichern
                        node.neighbors[i].previous = node;

                        //kosten speichern
                        node.neighbors[i].distanceTraveled = newDistanceTraveled;
                    }

                    //Informationen über Vorgängerknoten Speichern / Update: Kosten übergeben
                    if (!m_frontierNodes.Contains(node.neighbors[i]))
                    {
                        node.neighbors[i].priority = (int)node.neighbors[i].distanceTraveled;
                        m_frontierNodes.Enqueue(node.neighbors[i]);
                    }
                }
            }
        }
    }

    //Methode zum findes Weges !!Gierige Bestensuche!!
    void ExpandFrontierGreedyBestFirstSearch(Node node)
    {
        //Crash vermeiden
        if (node != null)
        {
            //alle nachbarn des Knoten abarbeiten
            for (int i = 0; i < node.neighbors.Count; i++)
            {
                //Nur NEUE Nachbarn verarbeiten nicht bereits verarbeitete Knoten!
                if (!m_exploredNodes.Contains(node.neighbors[i]) && !m_frontierNodes.Contains(node.neighbors[i]))
                {
                    //Distanz von Aktuellem Knoten zu den Nachbarn
                    float distanceToNeighbor = m_graph.GetNodeDistance(node, node.neighbors[i]);
                    float newDistanceTraveled = distanceToNeighbor + node.distanceTraveled;

                    //Menge der durchlaufenen Knoten
                    node.neighbors[i].distanceTraveled = newDistanceTraveled;

                    //Informationen über Vorgängerknoten Speichern
                    node.neighbors[i].previous = node;

                    //Fix für die Gierige Bestensuche
                    if (m_graph != null)
                    {
                        node.neighbors[i].priority = (int)m_graph.GetNodeDistance(node.neighbors[i], m_endNode);
                    }

                    //Neuen Knoten einsetzten
                    m_frontierNodes.Enqueue(node.neighbors[i]);
                }
            }
        }
    }

    //Methode zum finden des Weges !!A* Suche!!
    void ExpandFrontierAStarSearch(Node node)
    {
        //Crash vermeiden
        if (node != null)
        {
            //alle nachbarn des Knoten abarbeiten
            for (int i = 0; i < node.neighbors.Count; i++)
            {
                //Nur NEUE Nachbarn verarbeiten nicht bereits verarbeitete Knoten!
                if (!m_exploredNodes.Contains(node.neighbors[i]))
                {
                    //Distanz von Aktuellem Knoten zu den Nachbarn
                    float distanceToNeighbor = m_graph.GetNodeDistance(node, node.neighbors[i]);
                    float newDistanceTraveled = distanceToNeighbor + node.distanceTraveled;

                    //Wenn der Weg länger ist als ein bereits gefundener anderer Weg
                    if (float.IsPositiveInfinity(node.neighbors[i].distanceTraveled) || newDistanceTraveled < node.neighbors[i].distanceTraveled)
                    {
                        //Informationen über Vorgängerknoten Speichern
                        node.neighbors[i].previous = node;

                        //kosten speichern
                        node.neighbors[i].distanceTraveled = newDistanceTraveled;
                    }

                    //Berechnung für A* f(n) = g(n) + h(n) --> f(n) = Distanz von Startpunkt + Wahrscheinliche Distanz zum Ziel
                    if (!m_frontierNodes.Contains(node.neighbors[i]) && m_graph != null)
                    {
                        //h(n)
                        int distanceToEnd = (int) m_graph.GetNodeDistance(node.neighbors[i], m_endNode);

                        //f(n)
                        node.neighbors[i].priority = (int)node.neighbors[i].distanceTraveled + distanceToEnd;
                        m_frontierNodes.Enqueue(node.neighbors[i]);
                    }
                }
            }
        }
    }

    //PathNode liste füllen
    List<Node> GetPathNodes(Node exitNode)
    {
        List<Node> path = new List<Node>();

        //Wenn es kein Ende gibt gibt es keinen weg
        if(exitNode == null)
        {
            return path;
        }

        //Rückwärts durch den Graph gehen
        path.Add(exitNode);

        Node currentNode = exitNode.previous;

        while(currentNode != null)
        {
            //Den neuen Knoten immer an den Anfang packen
            path.Insert(0, currentNode);
            currentNode = currentNode.previous;
        }
        return path;

    }


}
