using System;
using System.Collections.Generic;

public class PriorityQueue<T> where T : IComparable<T>
{
    //Queue Daten
    //Queue Data
    private List<T> data;

    //Anzahl an Elementen in Queue
    //Count of Elements in Queue
    public int Count
    {
        get { return data.Count; }
    }

    //Konstruktor für PriorityQueue
    //Constructor
    public PriorityQueue()
    {
        this.data = new List<T>();
    }

    //Einreihen von neuen Werten
    //Add new Values
    public void Enqueue(T item)
    {
        //Zum Binary Heap hinzufügen
        //add to the binary heap
        data.Add(item);

        //Index des Childs
        //index of childs
        int childIndex = data.Count - 1;

        while (childIndex > 0)
        {
            //Index des Parents
            //Index of Parents
            int parentIndex = (childIndex - 1) / 2;

            //Wenn Child Priorität größer als Parent Priorität
            //If Child Priority higher then Parent Priority
            if (data[childIndex].CompareTo(data[parentIndex]) >= 0)
            {
                break;
            }

            //tauschen des Parents und Childs wenn Priorität falsch
            //nicht die beste Sortiervariante aber schneller als vollständige Sortierung
            //Exchange Child and Parents until Priorities match
            T tmp = data[childIndex];
            data[childIndex] = data[parentIndex];
            data[parentIndex] = tmp;

            childIndex = parentIndex;
        }
    }

    //Obersten Wert aus der Queue nehmen
    //Get First Elements of the Queue
    public T Dequeue()
    {
        //letztes Element der Liste
        //
        int lastIndex = data.Count - 1;

        //Element vom Anfang der Liste nehmen
        //
        T frontItem = data[0];

        //letztes Element der Liste an den Anfang verschieben um leeren Platz zu füllen
        //
        data[0] = data[lastIndex];

        //letztes Element der Liste löschen
        //
        data.RemoveAt(lastIndex);
        lastIndex--;

        //Neu Ordnen des Binary Heap
        //
        int parentIndex = 0;

        while (true)
        {
            //Child finden (Links)
            //find Child Left
            int childIndex = parentIndex * 2 + 1;

            //Wenn Child ist außerhalb des Heaps
            //If Child is out of Heap
            if (childIndex > lastIndex)
            {
                break;
            }

            //Child finden (Rechts)
            //find Child right
            int rightChild = childIndex + 1;

            //Rechts und Links vergleichen wenn Rechts im Heap ist --> Vergleich (Unter 0 --> rechts höhere priorität)
            //compare left and right side
            if (rightChild <= lastIndex && data[rightChild].CompareTo(data[childIndex]) < 0)
            {
                childIndex = rightChild;
            }

            //Sind Parent und Child in richtiger Reihenfolge
            //are parents and childs in the right order?
            if (data[parentIndex].CompareTo(data[childIndex]) <= 0)
            {
                break;
            }

            //Tauschen von Child und Parent wenn Reihenfolge falsch
            //exchange child and prent if priorities wrong
            T tmp = data[parentIndex];
            data[parentIndex] = data[childIndex];
            data[childIndex] = tmp;

            parentIndex = childIndex;
        }
        return frontItem;
    }

    //Oberstes Element der Queue Anzeigen
    //Show First Element of the Queue
    public T Peek()
    {
        T frontItem = data[0];
        return frontItem;
    }

    //Beinhaltet die Queue ein bestimmtes Element
    //Is there a specific element in the Queue ?
    public bool Contains(T item)
    {
        return data.Contains(item);
    }

    //Alle Elemente in Liste ablegen
    //All Elements to List
    public List<T> ToList()
    {
        return data;
    }
}