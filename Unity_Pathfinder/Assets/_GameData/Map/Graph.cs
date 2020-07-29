using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    //2d Array der Nodes
    //2d Array of Nodes
    public Node[,] nodes;

    //Wände
    //Walls
    public List<Node> walls = new List<Node>();

    //Informationen über die Karte
    //Informations about the map
    private int[,] m_mapData;

    private int m_width;
    private int m_height;

    //Breite und Höhe für die Algorithmen
    //Breadth and height for the Algorithms
    public int Width { get { return m_width; } }

    public int Height { get { return m_height; } }

    //Richtungsmöglichkeiten für bewegung
    //Angle of Movement
    public static readonly Vector2[] allDirections =
    {
        new Vector2(0f,1f),
        new Vector2(1f,1f),
        new Vector2(1f,0f),
        new Vector2(1f,-1f),
        new Vector2(0f,-1f),
        new Vector2(-1f,-1f),
        new Vector2(-1f,0f),
        new Vector2(-1f,1f)
    };

    //Initialisieren der Karte
    //Initialize the Map
    public void Init(int[,] mapData)
    {
        m_mapData = mapData;
        m_width = mapData.GetLength(0);
        m_height = mapData.GetLength(1);

        //node array
        nodes = new Node[m_width, m_height];

        //durch x und y laufen
        //go trough x and y
        for (int y = 0; y < m_height; y++)
        {
            for (int x = 0; x < m_width; x++)
            {
                //Kartendaten verarbeiten / wand...weg...
                //process map data
                NodeType type = (NodeType)mapData[x, y];
                Node newNode = new Node(x, y, type);
                nodes[x, y] = newNode;

                //wände in 2d aufzeigen
                //paint walls in 2d
                newNode.position = new Vector3(x, 0, y);

                //wände in array speichern
                //save walls in array
                if (type == NodeType.Blocked)
                {
                    walls.Add((newNode));
                }
            }
        }

        //durchlaufen des Arrays zum erstellen von nachbar liste
        //go trough the array to generate neigbour list
        for (int y = 0; y < m_height; y++)
        {
            for (int x = 0; x < m_width; x++)
            {
                if (nodes[x, y].nodeType != NodeType.Blocked)
                {
                    nodes[x, y].neighbors = GetNeighbors(x, y);
                }
            }
        }
    }

    //vermeiden des "out of Bounds" fehlers
    //avoid out of Bounds error
    public bool IsWithinBounds(int x, int y)
    {
        return (x >= 0 && x < m_width && y >= 0 && y < m_height);
    }

    //Nachbarn abrufen
    //get neighbours
    private List<Node> GetNeighbors(int x, int y, Node[,] nodeArray, Vector2[] directions)
    {
        //Liste der Nachbarn
        //list of neighbours
        List<Node> neighborNodes = new List<Node>();

        //Nachbarn ablaufen
        //go trough neigbours
        foreach (Vector2 dir in directions)
        {
            int newX = x + (int)dir.x;
            int newY = y + (int)dir.y;

            //prüfen ob Node kriterien für Nachbar erfüllt
            //check if a node is a neighbour
            if (IsWithinBounds(newX, newY) && nodeArray[newX, newY] != null &&
                nodeArray[newX, newY].nodeType != NodeType.Blocked)
            {
                neighborNodes.Add(nodeArray[newX, newY]);
            }
        }
        return neighborNodes;
    }

    //Nachbarn ausgeben
    //write neighbours
    private List<Node> GetNeighbors(int x, int y)
    {
        return GetNeighbors(x, y, nodes, allDirections);
    }

    //Wegkosten für Dijkstra berechnen
    //costs for dijkstra
    public float GetNodeDistance(Node source, Node target)
    {
        //Abstand von 2 unabhängigen Punkten über x und y Werte
        //distance between 2 independent points
        int dx = Mathf.Abs(source.xIndex - target.xIndex);
        int dy = Mathf.Abs(source.yIndex - target.yIndex);

        //Deltas für dx, dy bestimmen
        //set deltas for dx and dy 
        int min = Mathf.Min(dx, dy);
        int max = Mathf.Max(dx, dy);

        //Berechnen von Diagonal und Gerade
        //calculate diagonal and straight
        int diagonalSteps = min;
        int straightSteps = max - min;

        //Gesamte Kantenkosten
        //all node costs
        return (1.4f * diagonalSteps + straightSteps);
    }

    //Wegkosten nach Manhattan Heuristik
    //costs for manhattan heuristic
    public int GetManhattanDistance(Node source, Node target)
    {
        //Abstand von 2 unabhängigen Punkten über x und y Werte
        //Get Distance from 2 independent Points over x and y
        int dx = Mathf.Abs(source.xIndex - target.xIndex);
        int dy = Mathf.Abs(source.yIndex - target.yIndex);
        return (dx + dy);
    }
}