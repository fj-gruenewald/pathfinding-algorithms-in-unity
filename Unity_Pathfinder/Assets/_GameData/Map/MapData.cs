using System;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    //breite
    public int width = 10;

    //höhe
    public int height = 5;

    //txt für die Wand generierung
    public TextAsset textAsset;

    //RessourcePath zum laden von Karten
    public string resourcePath = "Mapdata";

    //Einlesen der wanddaten aus txt
    public List<string> GetTextFromFile(TextAsset txtAsset)
    {
        //linien aus textdokument
        List<string> lines = new List<string>();

        //string in variable ablegen
        if (txtAsset != null)
        {
            string textData = txtAsset.text;

            //Textumbruch für neue Zeile
            string[] delimiters = { "\r\n", "\n" };

            //einzelne Reihen Splitten / liste zu array konvertieren
            lines.AddRange(textData.Split(delimiters, System.StringSplitOptions.None));

            //erste reihe des strings ist erste reihe von unten
            lines.Reverse();
        }
        else
        {
            //vermeiden von Crash wenn Textdokument nicht vorhanden
            Debug.LogWarning("MapData: Inkorrektes Labyrinth geladen");
        }
        //
        return lines;
    }

    //Überladene GetTextFromFile Methode zur Ausgabe
    public List<string> GetTextFromFile()
    {
        if (textAsset == null)
        {
            textAsset = Resources.Load(resourcePath + "/Testing") as TextAsset;
        }
        return GetTextFromFile(textAsset);
    }

    //Größe der karte an Textdokument anpassen
    public void SetDimensions(List<string> textLines)
    {
        //höhe --> zeilen
        height = textLines.Count;

        //breite --> längste linie im dokument
        foreach (string line in textLines)
        {
            if (line.Length > width)
            {
                width = line.Length;
            }
        }
    }

    //Array / 2D Array als Koordinatensystem
    public int[,] MakeMap()
    {
        //jede zeile ein seperater string
        List<string> lines = new List<string>();
        lines = GetTextFromFile();
        SetDimensions(lines);

        //array initialisieren
        int[,] map = new int[width, height];

        //durch y gehen
        for (int y = 0; y < height; y++)
        {
            //durch x gehen
            for (int x = 0; x < width; x++)
            {
                //beheben des Out of Range Errors
                if (lines[y].Length > x)
                {
                    //chars in int konvertieren und einlesen
                    map[x, y] = (int)Char.GetNumericValue(lines[y][x]);
                }
            }
        }

        //map ausgeben
        return map;
    }
}