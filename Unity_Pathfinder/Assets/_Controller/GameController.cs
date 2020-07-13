using UnityEngine;

public class GameController : MonoBehaviour
{
    //Referenz zur MapData
    public MapData mapData;

    //Referenz zum Graph
    public Graph graph;

    //Referenz zum Pathfinder
    public Pathfinder pathfinder;

    //Start Koordinaten
    public int startX = 0;

    public int StartY = 0;

    //End Koordinaten
    public int endX = 20;

    public int endY = 10;

    //Zeit die ein Schritt benötigt
    public float timeStep = 0.1f;

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

            //Ausführen des Kartenbaus
            if (graphView != null)
            {
                graphView.Init(graph);
            }

            //Ausführen des Pathfinders
            if (graph.IsWithinBounds(startX, StartY) && graph.IsWithinBounds(endX, endY) && pathfinder != null)
            {
                Node startNode = graph.nodes[startX, StartY];
                Node endNode = graph.nodes[endX, endY];
                pathfinder.Init(graph, graphView, startNode, endNode);
            }
        }
    }

    //Button Start
    public void StartPathfinding()
    {
        StartCoroutine(pathfinder.SearchRoutine(timeStep));
    }
}