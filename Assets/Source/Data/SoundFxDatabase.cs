// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SoundFxDbElement
{
    public AudioClip clip;

    public string name
    {
        get
        {
            if (clip == null)
                return "";
            return clip.name;
        }
    }

    public bool isInUse
    {
        get;
        set;
    }

    public SoundFxDbElement()
    {
        isInUse = false;
    }
}

public class SoundFxDatabase : ScriptableObject
{
    [SerializeField]
    private List<SoundFxDbElement> m_elements = new List<SoundFxDbElement>();


    /// <summary>
    /// Adds a new clip to the list
    /// </summary>
    /// <param name="newClip"></param>
    public void Add(AudioClip newClip)
    {
        // Check to see if an audio clip with the same name is inside
        foreach (SoundFxDbElement e  in m_elements)
        {
            if (e.clip == newClip || e.name == newClip.name)
            {
                Debug.LogWarning("A clip with the same name is already in the database.");
                return;
            }
        }

        SoundFxDbElement newElement = new SoundFxDbElement();
        newElement.clip = newClip;
        m_elements.Add(newElement);
    }


    /// <summary>
    /// Returns the first available clip
    /// </summary>
    /// <returns></returns>
    public SoundFxDbElement GetFirstAvailable()
    {
        SoundFxDbElement found = m_elements.First(e => e.isInUse == false);
        return found;
    }


    /// <summary>
    /// Finds a clip by name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public SoundFxDbElement Get(string name)
    {
        SoundFxDbElement found = m_elements.Find(c => c.name == name);
        return found;
    }


    /// <summary>
    /// Removes a string by name
    /// </summary>
    /// <param name="name"></param>
    public void Remove(string name)
    {
        SoundFxDbElement found = m_elements.Find(e => e.name == name);
        if (found == null)
        {
            Debug.LogWarning("Failed to remove clip with the name " + name + ". No such clip.");
            return;
        }

        m_elements.Remove(found);
    }
}
