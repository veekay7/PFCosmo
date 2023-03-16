// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;

public class PlayerValueInventory : ScriptableObject
{
    [SerializeField, ReadOnly]
    private int m_value = 0;


    public int Get()
    {
        return m_value;
    }


    public void Store(int value)
    {
        m_value = value;
    }


    [ContextMenu("Clear")]
    public void Clear()
    {
        m_value = 0;
    }
}
