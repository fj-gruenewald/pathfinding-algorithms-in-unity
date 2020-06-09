using UnityEngine;

public class GameController : MonoBehaviour
{
    //
    public MapData mapData;

    //
    public Graph graph;

    private void Start()
    {
        //haben mapdata und graph daten
        if (mapData != null && graph != null)
        {
            //tiles auf karte bringen
            int[,] mapInstance = mapData.MakeMap();
            graph.Init(mapInstance);

            //
            GraphView graphView = graph.gameObject.GetComponent<GraphView>();

            //ausführen
            if (graphView != null)
            {
                graphView.Init(graph);
            }
        }
    }
}