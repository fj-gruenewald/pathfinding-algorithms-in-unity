using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    //2d Array der Nodes
    public Node[,] nodes;

    //wände
    public List<Node> walls = new List<Node>();

    //Informationen über die Karte
    private int[,] m_mapData;
    private int m_width;
    private int m_height;

    //Breite und Höhe für die Algorithmen
    public int Width {get { return m_width; } }
    public int Height { get { return m_height; } }

    //richtungsmöglichkeiten für bewegung
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
    public void Init(int[,] mapData)
    {
        m_mapData = mapData;
        m_width = mapData.GetLength(0);
        m_height = mapData.GetLength(1);

        //node array
        nodes = new Node[m_width, m_height];

        //durch x und y laufen
        for (int y = 0; y < m_height; y++)
        {
            for (int x = 0; x < m_width; x++)
            {
                //Kartendaten verarbeiten / wand...weg...
                NodeType type = (NodeType)mapData[x, y];
                Node newNode = new Node(x, y, type);
                nodes[x, y] = newNode;

                //wände in 2d aufzeigen
                newNode.position = new Vector3(x, 0, y);

                //wände in array speichern
                if (type == NodeType.Blocked)
                {
                    walls.Add((newNode));
                }
            }
        }

        //durchlaufen des Arrays zum erstellen von nachbar liste
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
    public bool IsWithinBounds(int x, int y)
    {
        return (x >= 0 && x < m_width && y >= 0 && y < m_height);
    }

    //Nachbarn abrufen
    private List<Node> GetNeighbors(int x, int y, Node[,] nodeArray, Vector2[] directions)
    {
        //Liste der Nachbarn
        List<Node> neighborNodes = new List<Node>();

        //Nachbarn ablaufen
        foreach (Vector2 dir in directions)
        {
            int newX = x + (int)dir.x;
            int newY = y + (int)dir.y;

            //prüfen ob Node kriterien für Nachbar erfüllt
            if (IsWithinBounds(newX, newY) && nodeArray[newX, newY] != null &&
                nodeArray[newX, newY].nodeType != NodeType.Blocked)
            {
                neighborNodes.Add(nodeArray[newX, newY]);
            }
        }
        return neighborNodes;
    }

    //Nachbarn ausgeben
    private List<Node> GetNeighbors(int x, int y)
    {
        return GetNeighbors(x, y, nodes, allDirections);
    }

    //Wegkosten für Dijkstra berechnen
    public float GetNodeDistance(Node source, Node target)
    {
        //Abstand von 2 unabhängigen Punkten über x und y Werte 
        int dx = Mathf.Abs(source.xIndex - target.xIndex);
        int dy = Mathf.Abs(source.yIndex - target.yIndex);

        //Deltas für dx, dy bestimmen
        int min = Mathf.Min(dx, dy);
        int max = Mathf.Max(dx, dy);

        //Berechnen von Diagonal und Gerade
        int diagonalSteps = min;
        int straightSteps = max - min;

        //Gesamte Kantenkosten
        return (1.4f * diagonalSteps + straightSteps);
    }
}