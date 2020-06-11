using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PriorityQueue<T> where T: IComparable<T>
{
    //Queue Daten
    List<T> data;

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
}
