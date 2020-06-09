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
        if (graph == null)
        {
            //wenn Graph nicht zusammenhängt
            Debug.LogWarning("KEIN GRAPH GEFUNDEN!");
            return;
        }

        //textur auf node anzeigen
        foreach (Node n in graph.nodes)
        {
            GameObject instance = Instantiate(nodeViewPrefab, Vector3.zero, Quaternion.identity);
            NodeView nodeView = instance.GetComponent<NodeView>();

            //wenn instance vorhanden starte init
            if (nodeView != null)
            {
                nodeView.Init(n);

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
}