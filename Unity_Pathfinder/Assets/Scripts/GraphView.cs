using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Vertauschen verhindern
//Graph und GraphView immer auf gleichem GameOBJ
[RequireComponent(typeof(Graph))]
public class GraphView : MonoBehaviour
{
    //tileprefab für Karte
    public GameObject nodeViewPrefab;

    //Infofarben 
    public Color baseColor = Color.white;
    public Color wallColor = Color.black;

    //Alles OK mit dem Graph?
    public void Init(Graph graph)
    {
        if(graph == null)
        {
            Debug.LogWarning("KEIN GRAPH GEFUNDEN!");
            return;
        }

        //
        foreach(Node n in graph.nodes)
        {
            GameObject instance Instantiate(nodeViewPrefab,Vector3.zero,)
        }
    }
}
