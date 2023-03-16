// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using System.Collections.Generic;
using UnityEngine;

public class LLValueManager : ScriptableObject
{
    [ReadOnly]
    public List<int> usableValues = new List<int>();
    [SerializeField, ReadOnly]
    private List<UsedValueData> m_used = new List<UsedValueData>();


    public void Init(int[] usableValueArray)
    {
        if (usableValues.Count > 0)
            usableValues.Clear();

        for (int i = 0; i < usableValueArray.Length; i++)
        {
            usableValues.Add(usableValueArray[i]);
        }
    }


    public int GetValueAndUse()
    {
        int output = 0;
        int idx = 0;
        for (int i = 0; i < usableValues.Count; i++)
        {
            // Find a value that isn't 0
            if (usableValues[i] != 0)
            {
                idx = i;
                output = usableValues[i];
                break;
            }
        }

        if (output != 0)
        {
            m_used.Add(new UsedValueData(output, idx));
            usableValues[idx] = 0;
        }

        return output;
    }


    public int GetValueAndUse(int value)
    {
        int output = 0;
        int idx = 0;
        for (int i = 0; i < usableValues.Count; i++)
        {
            // Find a value that matches it
            if (usableValues[i] == value)
            {
                idx = i;
                output = usableValues[i];
                break;
            }
        }

        if (output != 0)
        {
            m_used.Add(new UsedValueData(output, idx));
            usableValues[idx] = 0;
        }

        return output;
    }


    public int GetValue()
    {
        int output = 0;
        for (int i = 0; i < usableValues.Count; i++)
        {
            // Find a value that isn't 0
            if (usableValues[i] != 0)
            {
                output = usableValues[i];
                break;
            }
        }

        return output;
    }


    public void UseValue(int value)
    {
        for (int i = 0; i < usableValues.Count; i++)
        {
            if (value == usableValues[i])
            {
                m_used.Add(new UsedValueData(value, i));
                usableValues[i] = 0;
                return;
            }
        }

        Debug.Log("The value " + value + " cannot be found in this list.");
    }


    public void UnuseValue(int value)
    {
        UsedValueData found = m_used.Find((x) => x.value == value);
        usableValues[found.index] = found.value;
        m_used.Remove(found);
    }


    public void Clear()
    {
        for (int i = 0; i < m_used.Count; i++)
        {
            int idx = m_used[i].index;
            int value = m_used[i].value;
            usableValues[idx] = value;
        }

        m_used.Clear();
        usableValues.Clear();
    }
}

[Serializable]
public struct UsedValueData
{
    public int value;
    public int index;

    public UsedValueData(int value, int index)
    {
        this.value = value;
        this.index = index;
    }
}
