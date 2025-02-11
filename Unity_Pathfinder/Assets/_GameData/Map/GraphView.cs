﻿using System.Collections.Generic;
using UnityEngine;

//Vertauschen verhindern
//Graph und GraphView immer auf gleichem GameOBJ
[RequireComponent(typeof(Graph))]
public class GraphView : MonoBehaviour
{
    //tileprefab für Karte
    //tileprefab for map
    public GameObject nodeViewPrefab;

    //array der nodeviews zum färben
    //array of nodeviews for coloring
    public NodeView[,] nodeViews;

    //Farben der Kartenelemente
    //Colors of the map
    public Color baseColor = Color.grey;

    public Color wallColor = Color.black;

    //Alles OK mit dem Graph?
    //Is the Graph OK?
    public void Init(Graph graph)
    {
        if (graph == null)
        {
            //wenn Graph nicht zusammenhängt
            Debug.LogWarning("KEIN GRAPH GEFUNDEN!");
            return;
        }
        //nodeview array zum färben initialisieren
        nodeViews = new NodeView[graph.Width, graph.Height];

        //textur auf node anzeigen
        foreach (Node n in graph.nodes)
        {
            GameObject instance = Instantiate(nodeViewPrefab, Vector3.zero, Quaternion.identity);
            NodeView nodeView = instance.GetComponent<NodeView>();

            //wenn instance vorhanden starte init
            if (nodeView != null)
            {
                nodeView.Init(n);

                //nodeViews Array füllen
                nodeViews[n.xIndex, n.yIndex] = nodeView;

                //wände mit anderer farbe anzeigen
                if (n.nodeType == NodeType.Blocked)
                {
                    nodeView.ColorNode(wallColor);
                }
                else
                {
                    nodeView.ColorNode(baseColor);
                }
            }
        }
    }

    //Färben von Nodes für den Ablauf
    //Coloring of Nodes for progress
    public void ColorNodes(List<Node> nodes, Color color)
    {
        foreach (Node n in nodes)
        {
            if (n != null)
            {
                //NodeView zu Node finden
                NodeView nodeView = nodeViews[n.xIndex, n.yIndex];

                if (nodeView != null)
                {
                    //Node färben
                    nodeView.ColorNode(color);
                }
            }
        }
    }
}