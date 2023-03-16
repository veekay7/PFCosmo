// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System.Collections.Generic;
using UnityEngine;

public class PlatformsManager : MonoBehaviour
{
    public KanaTable kanaTable = null;
    [SerializeField]
    private GameObject m_platformPrefab = null;
    [SerializeField]
    private List<Platform> m_pool = new List<Platform>();

    public Puzzle owner { get; set; }
    public int Count => m_pool.Count;


    public void Init()
    {
        for(int i = 0; i < m_pool.Count; i++)
        {
            m_pool[i].owner = owner;
        }
    }


    public Platform[] GetArray()
    {
        return m_pool.ToArray();
    }


    public Platform GetNewPlatform()
    {
        // Get an unused platform from the pool and initialize a glyph and value
        Platform output = m_pool.Find((p) => !p.gameObject.activeInHierarchy);
        output.gameObject.SetActive(true);

        Kana newKana = kanaTable.GetUnusedKana();
        output.SetKana(newKana);

        return output;
    }


    [ContextMenu("Add New Platform to Pool")]
    public Platform CreatePlatform()
    {
        GameObject newPlatformObject = Instantiate(m_platformPrefab);
        Platform inst = newPlatformObject.GetComponent<Platform>();
        inst.transform.SetParent(transform, false);
        m_pool.Add(inst);
        return inst;
    }


    [ContextMenu("Delete Last Platform From Pool")]
    public void DelLastPlatform()
    {
        if (m_pool.Count == 0)
        {
            Debug.LogWarning("There are no more platforms in the pool to delete.");
            return;
        }

        int lastIdx = m_pool.Count - 1;
        Platform p = m_pool[lastIdx];
        if (Globals.isEditMode)
            DestroyImmediate(p.gameObject);
        else
            Destroy(p.gameObject);
        m_pool.Remove(p);
    }


    [ContextMenu("Invalidate Platforms")]
    public void InvalidatePlatforms()
    {
        for (int i = 0; i < m_pool.Count; i++)
        {
            Platform platform = m_pool[i];
            platform.Invalidate();
            platform.gameObject.SetActive(false);
            platform.transform.position = transform.position;
        }
    }


    [ContextMenu("Delete Pool")]
    public void DeletePool()
    {
        for (int i = 0; i < m_pool.Count; i++)
        {
            Platform platform = m_pool[i];
            if (Globals.isEditMode)
                DestroyImmediate(platform.gameObject);
            else
                Destroy(platform.gameObject);
        }

        m_pool.Clear();
    }
}
