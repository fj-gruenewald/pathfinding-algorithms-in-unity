using System;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    //Breite
    //breadth
    public int width = 10;

    //Höhe
    //height
    public int height = 5;

    //txt für die Wand generierung
    //txt for walls generation
    public TextAsset textAsset;

    //RessourcePath zum laden von Karten
    //RessourcePath for loading the map
    public string resourcePath = "Mapdata";

    //Einlesen der wanddaten aus txt
    //get the wall data from txt
    public List<string> GetTextFromFile(TextAsset txtAsset)
    {
        //linien aus textdokument
        //lines from text document
        List<string> lines = new List<string>();

        //string in variable ablegen
        //deposit string in variable
        if (txtAsset != null)
        {
            string textData = txtAsset.text;

            //Textumbruch für neue Zeile
            //new line
            string[] delimiters = { "\r\n", "\n" };

            //einzelne Reihen Splitten / liste zu array konvertieren
            //split single rows / list to array
            lines.AddRange(textData.Split(delimiters, System.StringSplitOptions.None));

            //erste reihe des strings ist erste reihe von unten
            //first line of the string ist first line bottom up
            lines.Reverse();
        }
        else
        {
            //vermeiden von Crash wenn Textdokument nicht vorhanden
            //avoid crash when the text document doesnt exist
            Debug.LogWarning("MapData: Inkorrektes Labyrinth geladen");
        }
        //
        return lines;
    }

    //Überladene GetTextFromFile Methode zur Ausgabe
    //Method for printing
    public List<string> GetTextFromFile()
    {
        if (textAsset == null)
        {
            textAsset = Resources.Load(resourcePath + "/Testing") as TextAsset;
        }
        return GetTextFromFile(textAsset);
    }

    //Größe der karte an Textdokument anpassen
    //Make Map Size txt size
    public void SetDimensions(List<string> textLines)
    {
        //höhe --> zeilen
        //height --> lines
        height = textLines.Count;

        //breite --> längste linie im dokument
        //Breadth --> longest line in document
        foreach (string line in textLines)
        {
            if (line.Length > width)
            {
                width = line.Length;
            }
        }
    }

    //Array / 2D Array als Koordinatensystem
    //2D Array as Coordinate System
    public int[,] MakeMap()
    {
        //jede zeile ein seperater string
        //every line a seperate string
        List<string> lines = new List<string>();
        lines = GetTextFromFile();
        SetDimensions(lines);

        //array initialisieren
        //initialize array
        int[,] map = new int[width, height];

        //durch y gehen
        //go trough y
        for (int y = 0; y < height; y++)
        {
            //durch x gehen
            //go trough x
            for (int x = 0; x < width; x++)
            {
                //beheben des Out of Range Errors
                //avaoid out of range error
                if (lines[y].Length > x)
                {
                    //chars in int konvertieren und einlesen
                    //convert chars to int and read
                    map[x, y] = (int)Char.GetNumericValue(lines[y][x]);
                }
            }
        }

        //map ausgeben
        //print map
        return map;
    }
}