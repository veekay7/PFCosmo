// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KanaTable : ScriptableObject
{
    [SerializeField]
    private List<Kana> m_kanaList = new List<Kana>();


    public bool CreateFromSprites(Sprite[] sprites)
    {
        if (sprites != null && sprites.Length > 0)
            return false;

        for (int i = 0; i < sprites.Length; i++)
        {
            Kana kana = new Kana(i, sprites[i]);
            m_kanaList.Add(kana);
        }

        return true;
    }


    public Kana GetUnusedKana()
    {
        Kana found = null;
        foreach (Kana g in m_kanaList)
        {
            if (!g.isUsed)
            {
                found = g;
                break;
            }
        }

        return found;
    }


    public Kana GetKana(int id)
    {
        if (m_kanaList.Count == 0 || id < 0)
            throw new ArgumentException("id");

        Kana found = null;
        foreach (Kana g in m_kanaList)
        {
            if (g.id == id)
            {
                found = g;
                break;
            }
        }

        return found;
    }

    [ContextMenu("Clear States")]
    public void ClearStates()
    {
        for (int i = 0; i < m_kanaList.Count; i++)
        {
            m_kanaList[i].isUsed = false;
        }
    }
}
