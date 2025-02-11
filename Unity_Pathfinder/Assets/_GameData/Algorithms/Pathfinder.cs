﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    //Initialisierung von Fremdklassen
    //Initialize other Classes
    public IngameInfos ingameInfos;
    public IngameOptionsMenu ingameOptionsMenu;

    //Start und Endpunkt
    //start and endpoints
    private Node m_startNode;

    private Node m_endNode;

    //Referenzen zu Darstellenden Objekten
    //reference objects to show
    private Graph m_graph;

    private GraphView m_graphView;

    //Queue abzuarbeitender Knoten / Update: zur PriorityQueue
    //enqueued nodes
    private PriorityQueue<Node> m_frontierNodes;

    //Bereits besuchte Knoten
    //already visited nodes
    private List<Node> m_exploredNodes;

    //Liste des kürzesten weges
    //list of the shortest Path
    private List<Node> m_pathNodes;

    //Farben für den Ablauf info: Color32 für normale RGB Werte Bsp. Color32(216, 216, 216, 255) , Color für float Werte Bsp. Color(0.85f, 0.85f, 0.85f, 1f)
    //Colors for execution
    public Color startColor = Color.green;

    public Color endColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploredColor = Color.gray;
    public Color pathColor = Color.cyan;

    //Suchvariablen
    //search variables
    public bool isComplete = false;

    private int m_iterations = 0;

    //Suche durchführen
    //execute search
    public void Init(Graph graph, GraphView graphView, Node start, Node end)
    {
        //nichts darf fehlen
        //nothing missing
        if (start == null || endColor == null || graph == null || graphView == null)
        {
            Debug.LogWarning("There are Mice in the Computer #42! pathfinder_Init_component-missing");
            return;
        }

        //start und endpunkt dürfen nicht in wänden liegen
        //start and endpoints out of walls
        if (start.nodeType == NodeType.Blocked || end.nodeType == NodeType.Blocked)
        {
            Debug.LogWarning("There are Mice in the Computer #42! pathfinder_Init_start/end-blocked");
            return;
        }

        //variablen ablegen
        //variables
        m_graph = graph;
        m_graphView = graphView;
        m_startNode = start;
        m_endNode = end;

        //Färben
        //Coloring
        ShowColors(graphView, start, end);

        //initialisierung
        //initialize
        m_frontierNodes = new PriorityQueue<Node>();
        m_frontierNodes.Enqueue(start);
        m_exploredNodes = new List<Node>();
        m_pathNodes = new List<Node>();

        //Durch alle Knoten gehen
        //move trough all nodes
        for (int x = 0; x < m_graph.Width; x++)
        {
            //Durch alle Knoten gehen
            //move though all nodes
            for (int y = 0; y < m_graph.Height; y++)
            {
                //Zurück zum Anfang
                //back to start
                m_graph.nodes[x, y].Reset();
            }
        }

        //Informationen über den Verlauf der Suche zurücksetzten
        //informations about the search algorithm
        isComplete = false;
        m_iterations = 0;
        m_startNode.distanceTraveled = 0;
    }

    //Überladene ShowColors
    //overloaded showColors
    private void ShowColors()
    {
        ShowColors(m_graphView, m_startNode, m_endNode);
    }

    //Knoten färben
    //
    private void ShowColors(GraphView graphView, Node start, Node end)
    {
        //Crash durch fehlende Werte vermeiden
        //avoid crash because of missing values
        if (graphView == null || start == null || end == null)
        {
            return;
        }

        //Färben der Knoten die verarbeitet werden
        //color the nodes
        if (m_frontierNodes != null)
        {
            graphView.ColorNodes(m_frontierNodes.ToList(), frontierColor);
        }

        //färben der abgearbeiteteten Knoten
        //color visited nodes
        if (m_exploredNodes != null)
        {
            graphView.ColorNodes(m_exploredNodes, exploredColor);
        }

        //Färben der Knoten des gefundenen Weges
        //color path nodes
        if (m_pathNodes != null && m_pathNodes.Count > 0)
        {
            graphView.ColorNodes(m_pathNodes, pathColor);
        }

        //Start und Endpunkt Färben
        //color start and endpoint
        NodeView startNodeView = graphView.nodeViews[start.xIndex, start.yIndex];

        if (startNodeView != null)
        {
            startNodeView.ColorNode(startColor);
        }

        //Endpunkt färben
        //color endpoint
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
        while (!isComplete)
        {
            if (m_frontierNodes.Count > 0)
            {
                //Knoten aus "nächste Knoten" liste entfernen
                Node currentNode = m_frontierNodes.Dequeue();
                m_iterations++;

                //Knoten zu "besuchte Knoten" liste hinzufügen
                if (!m_exploredNodes.Contains(currentNode))
                {
                    m_exploredNodes.Add(currentNode);
                }

                //Durchführen der Suchalgorithmen
                if (ingameOptionsMenu.GetSearchMode() == IngameOptionsMenu.Mode.BreadthFirstSearch)
                {
                    ExpandFrontierBreadthFirstSearch(currentNode);
                }
                else if (ingameOptionsMenu.GetSearchMode() == IngameOptionsMenu.Mode.DijkstraAlgorithm)
                {
                    ExpandFrontierDijkstraAlgorithm(currentNode);
                }
                else if (ingameOptionsMenu.GetSearchMode() == IngameOptionsMenu.Mode.GreedyBestFirstSearch)
                {
                    ExpandFrontierGreedyBestFirstSearch(currentNode);
                }
                else if (ingameOptionsMenu.GetSearchMode() == IngameOptionsMenu.Mode.AStarSearch)
                {
                    ExpandFrontierAStarSearch(currentNode);
                }

                //Wenn es ein endpunkt gibt
                if (m_frontierNodes.Contains(m_endNode))
                {
                    //Knoten rückwärts in pathnodes übergeben
                    m_pathNodes = GetPathNodes(m_endNode);

                    //Suche beenden wenn Ziel gefunden
                    if (ingameOptionsMenu.GetExitOnGoal())
                    {
                        isComplete = true;

                        //Ausgabe: UserInterface
                        ingameInfos.SetSearchNodes(m_endNode.distanceTraveled.ToString());

                        //Ausgabe: wie lang ist der gefundene Weg
                        Debug.Log("Der Suchalgorithmus: " + ingameOptionsMenu.GetSearchMode().ToString() + " fand einen Weg der länge: " + m_endNode.distanceTraveled.ToString());
                    }
                }

                //gesamte Visualisierung nur Anzeigen wenn gewollt
                if (ingameOptionsMenu.GetShowIterations())
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

        //Ausgabe: UserInterface
        ingameInfos.SetSearchTime((Time.time - timeStart).ToString("0.##"));

        //Ausgabe: Ende der Zeitmessung für die Anzeige
        Debug.Log("Zeit bis zur Terminierung = " + (Time.time - timeStart).ToString() + " Sekunden");
    }

    //Methode zum verwalten der Visualisierungen
    private void ShowVisualization()
    {
        //Färbung nur Anzeigen wenn gewollt
        if (ingameOptionsMenu.GetShowColor())
        {
            //Färben der Knoten
            ShowColors();
        }
    }

    //Methode zum finden des Weges !!Breitensuche!!
    private void ExpandFrontierBreadthFirstSearch(Node node)
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

                    //Fix für die Breitensuche
                    node.neighbors[i].priority = m_exploredNodes.Count;

                    //Neuen Knoten einsetzten
                    m_frontierNodes.Enqueue(node.neighbors[i]);
                }
            }
        }
    }

    //Methode zum finden des Weges !!Dijkstra!!
    private void ExpandFrontierDijkstraAlgorithm(Node node)
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
    private void ExpandFrontierGreedyBestFirstSearch(Node node)
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
    private void ExpandFrontierAStarSearch(Node node)
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
                        int distanceToEnd = (int)m_graph.GetNodeDistance(node.neighbors[i], m_endNode);

                        //f(n)
                        node.neighbors[i].priority = (int)node.neighbors[i].distanceTraveled + distanceToEnd;
                        m_frontierNodes.Enqueue(node.neighbors[i]);
                    }
                }
            }
        }
    }

    //PathNode liste füllen
    private List<Node> GetPathNodes(Node exitNode)
    {
        List<Node> path = new List<Node>();

        //Wenn es kein Ende gibt gibt es keinen weg
        if (exitNode == null)
        {
            return path;
        }

        //Rückwärts durch den Graph gehen
        path.Add(exitNode);

        Node currentNode = exitNode.previous;

        while (currentNode != null)
        {
            //Den neuen Knoten immer an den Anfang packen
            path.Insert(0, currentNode);
            currentNode = currentNode.previous;
        }
        return path;
    }
}