// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;

public class RestartCounter : ScriptableObject
{
    private int m_restartCount;


    public void Reset()
    {
        m_restartCount = 0;
    }


    public void Increment()
    {
        m_restartCount++;
    }


    public int GetCount()
    {
        return m_restartCount;
    }
}
