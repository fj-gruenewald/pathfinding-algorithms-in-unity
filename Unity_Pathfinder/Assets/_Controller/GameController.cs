using UnityEngine;

public class GameController : MonoBehaviour
{
    //Referenz zur MapData
    //reference mapData
    public MapData mapData;

    //Referenz zum IngameMenu
    //reference IngameMenu
    public IngameOptionsMenu ingameOptionsMenu;

    //Referenz zum Graph
    //reference Graph
    public Graph graph;

    //Referenz zum Pathfinder
    //reference Pathfinder
    public Pathfinder pathfinder;

    //Start Koordinaten
    //start coordinates
    public int startX = 0;

    public int StartY = 0;

    //End Koordinaten
    //end coordinates
    public int endX = 20;

    public int endY = 10;

    private void Start()
    {
        //haben mapdata und graph daten
        //have mapdata and graph data
        if (mapData != null && graph != null)
        {
            //tiles auf karte bringen
            //print tiles on map
            int[,] mapInstance = mapData.MakeMap();
            graph.Init(mapInstance);

            GraphView graphView = graph.gameObject.GetComponent<GraphView>();

            //Ausführen des Kartenbaus
            //execute map building
            if (graphView != null)
            {
                graphView.Init(graph);
            }

            //Ausführen des Pathfinders
            //execute Pathfindder
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
        StartCoroutine(pathfinder.SearchRoutine(ingameOptionsMenu.GetTimeStep()));
    }
}