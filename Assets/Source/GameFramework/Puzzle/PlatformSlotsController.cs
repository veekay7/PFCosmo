// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System.Collections.Generic;
using UnityEngine;

public class PlatformSlotsController : MonoBehaviour
{
    [SerializeField]
    private PlatformSlot m_slotPrefab = null;
    [SerializeField]
    private List<PlatformSlot> m_slots = new List<PlatformSlot>();

    public Puzzle owner { get; set; }

    public int Count
    {
        get { return m_slots.Count; }
    }


    [ContextMenu("Create")]
    public PlatformSlot CreateSlot()
    {
        Debug.Assert(m_slotPrefab, "Slot Prefab is null");

        if (m_slots.Count > Globals.MaxNodesPerPuzzle)
        {
            Debug.LogWarning("Maximum Slots Reached!");
            return null;
        }

        PlatformSlot newSlot = Instantiate(m_slotPrefab);
        newSlot.transform.SetParent(transform, false);
        newSlot.name = "Slot-" + m_slots.Count.ToString();
        m_slots.Add(newSlot);

        return newSlot;
    }


    [ContextMenu("Delete Last")]
    public void DeleteLastSlot()
    {
        if (m_slots.Count == 0)
        {

            Debug.LogWarning("There are no more slots to delete.");
            return;
        }

        int lastIdx = m_slots.Count - 1;
        PlatformSlot slot = m_slots[lastIdx];
        if (Globals.isEditMode)
            DestroyImmediate(slot.gameObject);
        else
            Destroy(slot.gameObject);
        m_slots.Remove(slot);
    }


    [ContextMenu("Delete All")]
    public void DeleteAll()
    {
        for (int i = 0; i < m_slots.Count; i++)
        {
            if (Globals.isEditMode)
                DestroyImmediate(m_slots[i].gameObject);
            else
                Destroy(m_slots[i].gameObject);
        }

        m_slots.Clear();
    }


    public void InsertPlatformAt(int index, Platform platform)
    {
        Debug.Assert(index >= 0 && index < Count, "Index out of range");

        if (index >= 0 && index < Count - 1)
        {
            // If inserting at the front or somewhere in the middle ( ͡◉ ͜ʖ ͡◉)
            // first record the index where we are starting from and create a temporary list
            // to store the Platform Slots that we want to move.
            int startIdx = index;
            List<Platform> temp = new List<Platform>();
            for (int i = startIdx; i < Count; i++)
            {
                Platform p = m_slots[i].GetPlatform();
                if (p != null)
                {
                    m_slots[i].SetPlatform(null);
                    temp.Add(p);
                }
            }

            // Add the new platform to the desired position
            m_slots[startIdx].SetPlatform(platform);

            startIdx++;
            for (int i = 0; i < temp.Count; i++)
            {
                if (startIdx > Count - 1)
                    break;
                m_slots[startIdx].SetPlatform(temp[i]);
                startIdx++;
            }
        }
        else if (index == Count - 1)
        {
            // Since we are inserting in the back ( ͡° ͜ʖ ͡°),
            // we must find out the actual last index
            int lastIdx = 0;
            for (int i = 0; i < m_slots.Count; i++)
            {
                if (m_slots[i].GetPlatform() != null)
                    lastIdx++;
            }

            // Once we found the last index, increase it by one since that's where we're gonna put the thing
            if (lastIdx > Count - 1)
            {
                Debug.LogWarning("Already beyond last index, can't push platform into slot.");
                return;
            }

            if (m_slots[lastIdx].GetPlatform() != null)
            {
                Debug.LogWarning("There is a platform at index: " + index + ". Cannot add platform to back slot.");
                return;
            }

            m_slots[lastIdx].SetPlatform(platform);
        }

        RefreshPositions();
    }


    public void RemovePlatformFromSlot(Platform platform)
    {
        Debug.Assert(platform != null, "Parameter is null");

        // Find the slot with this platform, and remove the platform the found slot. 
        List<Platform> temp = new List<Platform>();
        for (int i = 0; i < m_slots.Count; i++)
        {
            PlatformSlot slot = m_slots[i];
            if (slot.GetPlatform() == platform)
            {
                slot.SetPlatform(null);
            }
            else
            {
                temp.Add(slot.GetPlatform());
                slot.SetPlatform(null);
            }
        }

        // Then rearrange all the platforms in the remaining slots
        int idx = 0;
        for(int i = 0; i < temp.Count; i++)
        {
            m_slots[idx].SetPlatform(temp[i]);
            idx++;
        }

        RefreshPositions();
    }


    public void RefreshPositions()
    {
        if (Globals.isEditMode)
        {
            foreach (PlatformSlot s in m_slots)
            {
                Platform p = s.GetPlatform();
                if (p == null)
                    continue;
                p.transform.position = s.transform.position;
            }

            return;
        }
    }


    public PlatformSlot Get(int index)
    {
        return m_slots[index];
    }


    public PlatformSlot Get(Platform platform)
    {
        PlatformSlot output = null;
        for (int i = 0; i < Count; i++)
        {
            if (m_slots[i].GetPlatform() == platform)
            {
                output = m_slots[i];
                break;
            }
        }

        return output;
    }


    public PlatformSlot GetEmptySlot()
    {
        PlatformSlot output = null;
        for (int i = 0; i < Count; i++)
        {
            if (m_slots[i].GetPlatform() == null)
            {
                output = m_slots[i];
                break;
            }
        }

        return output;
    }


    public void InvalidateSlots()
    {
        for (int i = 0; i < Count; i++)
        {
            m_slots[i].SetPlatform(null);
        }
    }


    public List<PlatformSlot> GetList()
    {
        return m_slots;
    }
}
