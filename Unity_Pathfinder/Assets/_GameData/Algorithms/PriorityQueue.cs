using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PriorityQueue<T> where T: IComparable<T>
{
    //Queue Daten
    List<T> data;

    //Anzahl an Elementen in Queue
    public int Count
    {
        get { return data.Count; }
    }

    //Konstruktor für PriorityQueue
    public PriorityQueue()
    {
        this.data = new List<T>();
    }

    //Einreihen von neuen Werten
    public void Enqueue(T item)
    {
        //Zum Binary Heap hinzufügen
        data.Add(item);

        //Index des Childs
        int childIndex = data.Count - 1;

        //
        while (childIndex > 0)
        {
            //Index des Parents
            int parentIndex = (childIndex - 1) / 2;

            //Wenn Child Priorität größer als Parent Priorität
            if (data[childIndex].CompareTo(data[parentIndex]) >= 0)
            {
                break;
            }

            //tauschen des Parents und Childs wenn Priorität falsch
            //nicht die beste Sortiervariante aber schneller als vollständige Sortierung
            T tmp = data[childIndex];
            data[childIndex] = data[parentIndex];
            data[parentIndex] = tmp;

            childIndex = parentIndex;
        }
    }

    //Obersten Wert aus der Queue nehmen
    public T Dequeue()
    {
        //letztes Element der Liste
        int lastIndex = data.Count - 1;

        //Element vom Anfang der Liste nehmen
        T frontItem = data[0];

        //letztes Element der Liste an den Anfang verschieben um leeren Platz zu füllen
        data[0] = data[lastIndex];

        //letztes Element der Liste löschen
        data.RemoveAt(lastIndex);
        lastIndex--;

        //Neu Ordnen des Binary Heap
        int parentIndex = 0;

        while (true)
        {
            //Child finden (Links)
            int childIndex = parentIndex * 2 + 1;

            //Wenn Child ist außerhalb des Heaps
            if (childIndex > lastIndex)
            {
                break;
            }

            //Child finden (Rechts)
            int rightChild = childIndex + 1;

            //Rechts und Links vergleichen wenn Rechts im Heap ist --> Vergleich (Unter 0 --> rechts höhere priorität)
            if (rightChild <= lastIndex && data[rightChild].CompareTo(data[childIndex]) < 0)
            {
                childIndex = rightChild;
            }

            //Sind Parent und Child in richtiger Reihenfolge
            if (data[parentIndex].CompareTo(data[childIndex]) <= 0)
            {
                break;
            }

            //Tauschen von Child und Parent wenn Reihenfolge falsch
            T tmp = data[parentIndex];
            data[parentIndex] = data[childIndex];
            data[childIndex] = tmp;

            parentIndex = childIndex;

        }
        return frontItem;
    }

    //Oberstes Element der Queue Anzeigen
    public T Peek()
    {
        T frontItem = data[0];
        return frontItem;
    }

    //Beinhaltet die Queue ein bestimmtes Element
    public bool Contains(T item)
    {
        return data.Contains(item);
    }

    //Alle Elemente in Liste ablegen
    public List<T> ToList()
    {
        return data;
    }
}
