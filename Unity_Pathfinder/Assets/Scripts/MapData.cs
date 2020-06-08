using UnityEngine;

public class MapData : MonoBehaviour
{
    //breite
    public int width = 10;

    //höhe
    public int height = 5;

    //Array / 2D Array als Koordinatensystem
    public int[,] MakeMap()
    {
        //array initialisieren
        int[,] map = new int[width, height];
        
        //durch y gehen
        for(int y = 0; y < height; y++)
        {
            //durch x gehen
            for (int x = 0; x < width; x++)
            {
                map[x, y] = 0;
            }
        }

        //wände einfügen
        map[1, 0] = 1;
        map[1, 1] = 1;
        map[1, 2] = 1;

        //map ausgeben
        return map;
    }

}
