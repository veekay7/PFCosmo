// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Authors: VinTK
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollection : ScriptableObject
{
    private List<Collectible> m_collection = new List<Collectible>();


    public int collectedAmount => m_collection.Count;


    public bool AddItem(Collectible item)
    {
        m_collection.Add(item);
        return true;
    }


    public Collectible GetItem(int index)
    {
        if (index < 0 || index > m_collection.Count - 1)
            throw new IndexOutOfRangeException("index");
        return m_collection[index];
    }


    public void RemoveItem(int index)
    {
        if (index < 0 || index > m_collection.Count - 1)
            throw new IndexOutOfRangeException("index");
        m_collection.RemoveAt(index);
    }


    public List<Collectible> GetCollection()
    {
        return m_collection;
    }


    public void Clear()
    {
        m_collection.Clear();
    }
}
