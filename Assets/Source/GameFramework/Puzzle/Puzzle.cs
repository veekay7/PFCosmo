// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Referenced from:
// https://crunchify.com/a-simple-singly-linked-list-implementation-in-java/s
// https://codingfreak.blogspot.com/2012/12/deleting-node-from-singly-linked-list.html
// https://www.codesdope.com/blog/article/inserting-a-new-node-to-a-linked-list-in-c/
// Author: VinTK
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public PlatformSlotsController slotsCtrl = null;
    public PlatformsManager platformMgmt = null;
    public LLValueManager valueManager = null;

    [ReadOnly]
    public Platform head = null;
    [ReadOnly]
    public int count = 0;
    [ReadOnly]
    public List<Platform> m_activePlatforms = new List<Platform>();  // All active platforms for this puzzle (?)
    [ReadOnly]
    public bool isBusy;

    public int maxCount => slotsCtrl.Count;


    private void OnEnable()
    {
        slotsCtrl.owner = this;
        platformMgmt.owner = this;
        platformMgmt.Init();
    }


    private void Start()
    {
        isBusy = false;
    }


    public Platform AddFirst(int value)
    {
        if (count == maxCount)
            return null;

        Platform newPlatform = platformMgmt.GetNewPlatform();
        newPlatform.SetValue(value);

        newPlatform.next = head;
        head = newPlatform;
        count++;

        newPlatform.name = "Platform-" + count.ToString();

        // Insert the platform in the correct slot
        slotsCtrl.InsertPlatformAt(0, newPlatform);
        m_activePlatforms.Insert(0, newPlatform);

        return newPlatform;
    }


    public Platform AddLast(int value)
    {
        if (count == maxCount)
            return null;

        Platform newPlatform = platformMgmt.GetNewPlatform();
        newPlatform.SetValue(value);
        newPlatform.next = null;

        if (head == null)
            head = newPlatform;

        Platform nodePrev = Find(count - 1);
        if (nodePrev != null)
            nodePrev.next = newPlatform;

        count++;

        newPlatform.name = "Platform-" + count.ToString();

        // Insert the platform in the correct slot
        slotsCtrl.InsertPlatformAt(slotsCtrl.Count - 1, newPlatform);
        m_activePlatforms.Add(newPlatform);

        return newPlatform;
    }


    public Platform AddAt(int index, int value)
    {
        Debug.Assert(index > -1, "Index cannot be less than 0");

        if (count == maxCount)
            return null;

        Platform newPlatform = null;
        Platform current = null;

        // Check the index of the location we are adding to determine how we should shift the platforms
        if (index == 0)
        {
            return null;
        }
        else
        {
            current = Find(index);

            Platform prevCurrent = GetPreviousOf(current);

            // Get a new platform from the pool
            newPlatform = platformMgmt.GetNewPlatform();
            newPlatform.SetValue(value);

            if (prevCurrent != null)
                prevCurrent.next = newPlatform;
            newPlatform.next = current;

            count++;
            newPlatform.name = "Platform-" + count.ToString();

            // Insert the platform in the correct slot
            slotsCtrl.InsertPlatformAt(index, newPlatform);
            m_activePlatforms.Insert(index, newPlatform);
        }

        return newPlatform;
    }


    public Platform RemoveFirst()
    {
        if (count == 0)
            return null;

        // Save the one to return
        Platform temp = head;

        // Do reference manipulation
        head = head.next;
        temp.next = null;
        count--;

        temp.name = "Platform";
        m_activePlatforms.Remove(temp);

        return temp;
    }


    public Platform RemoveLast()
    {
        if (count == 0)
            return null;

        Platform temp = null;
        count--;

        temp.name = "Platform";
        m_activePlatforms.Remove(temp);

        return temp;
    }


    public Platform RemoveAt(int index)
    {
        // Find the platform at the specified index
        Platform node = Find(index);
        if (node == null)
            return null;

        // Get the value from the platform, remove the value and return it to store
        int value = node.value;
        if (value != 0)
            valueManager.UnuseValue(value);

        // Get the previous and next of the platform
        Platform nodePrev = GetPreviousOf(node);
        Platform nodeNext = node.next;

        // If we are removing head, we need to update head
        if (head == node)
        {
            head = node.next;
        }
        else if (nodePrev != null)
        {
            if (nodeNext == null)
            {
                nodePrev.next = null;
            }
            else
            {
                nodePrev.next = nodeNext;
            }
        }

        slotsCtrl.RemovePlatformFromSlot(node);

        // Remove from list of active nodes
        m_activePlatforms.Remove(node);
        count--;

        return node;
    }


    public Platform Find(int index)
    {
        int currentIdx = -1;
        Platform output = head;
        while (output != null)
        {
            currentIdx++;
            if (currentIdx == index)
                break;
            output = output.next;
        }

        return output;
    }


    public Platform Find(Platform p)
    {
        Platform output = head;
        while (output != null)
        {
            if (output == p)
                break;
            output = output.next;
        }

        return output;
    }


    public Platform GetPreviousOf(Platform p)
    {
        Platform previous = null;
        Platform current = head;
        while (current != null)
        {
            if (current == p)
                break;
            previous = current;
            current = current.next;
        }

        return previous;
    }


    public int IndexOf(Platform p)
    {
        int idx = -1;
        Platform current = head;
        while (current != null)
        {
            idx++;
            if (current == p)
                break;
            current = current.next;
        }

        return idx;
    }


    [ContextMenu("Clear")]
    public void Clear()
    {
        if (Globals.isEditMode)
        {
            // Clear all slots
            slotsCtrl.InvalidateSlots();

            // Disable all platforms in the Platform Manager
            // and moves them to the origin of the Platform Manager.
            platformMgmt.InvalidatePlatforms();

            // Remove all active platforms from the Puzzle
            m_activePlatforms.Clear();

            head = null;
            count = 0;
        }
        else
        {
            StartCoroutine(Co_Clear());
        }
    }


    public IEnumerator Co_ArrangePlatformsToSlotsAnim(Platform newPlatform, bool reverse = true, Action onComplete = null)
    {
        if (reverse)
            yield return Co_InternalArrangePlatformsReverse(newPlatform);
        else
            yield return Co_InternalArrangePlatformsForward(newPlatform);

        if (onComplete != null)
            onComplete.Invoke();
    }


    public IEnumerator Co_PlayHidePlatformAnim(Platform platform, Action onComplete = null)
    {
        Vector3 finalPos = platform.position;
        finalPos.y = platformMgmt.transform.position.y;

        yield return platform.Co_MoveBerp(finalPos);

        platform.gameObject.SetActive(false);

        if (onComplete != null)
            onComplete.Invoke();
    }


    private IEnumerator Co_InternalArrangePlatformsReverse(Platform newPlatform)
    {
        for (int i = slotsCtrl.Count - 1; i >= 0; i--)
        {
            PlatformSlot s = slotsCtrl.Get(i);
            Platform p = s.GetPlatform();
            if (p != null && s != null)
            {
                if (p.transform.position == s.transform.position)
                    continue;

                // もし現在のプラットフォームは新しいのは、プラットフォームの生きるポイントからスロットのポジションまで移動する。
                // あたらしプラットフォームのtransform.position.x = slot.position.x
                if (p == newPlatform)
                {
                    Vector3 newPlatformPos = p.transform.position;
                    newPlatformPos.x = s.transform.position.x;
                    p.transform.position = newPlatformPos;
                    yield return p.Co_MoveBerp(s.transform.position, null);
                }
                else
                {
                    yield return p.Co_MoveSinerp(s.transform.position, null);
                }
            }
        }
    }


    private IEnumerator Co_InternalArrangePlatformsForward(Platform newPlatform)
    {
        for (int i = 0; i < slotsCtrl.Count - 1; i++)
        {
            PlatformSlot s = slotsCtrl.Get(i);
            Platform p = s.GetPlatform();
            if (p != null && s != null)
            {
                if (p.transform.position == s.transform.position)
                    continue;

                // もし現在のプラットフォームは新しいのは、プラットフォームの生きるポイントからスロットのポジションまで移動する。
                // あたらしプラットフォームのtransform.position.x = slot.position.x
                if (p == newPlatform)
                {
                    Vector3 newPlatformPos = p.transform.position;
                    newPlatformPos.x = s.transform.position.x;
                    p.transform.position = newPlatformPos;
                    yield return p.Co_MoveBerp(s.transform.position, null);
                }
                else
                {
                    yield return p.Co_MoveSinerp(s.transform.position, null);
                }
            }
        }
    }


    private IEnumerator Co_Clear()
    {
        // Iterate through all active platforms and clear their internal state
        foreach (PlatformSlot s in slotsCtrl.GetList())
        {
            // Get the platform itself from the slot
            Platform p = s.GetPlatform();
            if (p != null)
            {
                // First, we must unuse the value from the platform
                valueManager.UnuseValue(p.value);

                // Then we can clear out the state
                p.Invalidate();

                // Modify the target position.x to be the same as the platform we are moving.
                // Since we want them to look like they're sinking into the ground.
                Vector3 movePos = platformMgmt.transform.position;
                movePos.x = p.transform.position.x;

                yield return p.Co_MoveSinerp(movePos, null);

                p.gameObject.SetActive(false);
                s.SetPlatform(null);

                // After that, we can remove this shit from the list of active platforms
                m_activePlatforms.Remove(p);
            }
        }

        // Clear all slots
        slotsCtrl.InvalidateSlots();

        // Disable all platforms in the Platform Manager
        // and moves them to the origin of the Platform Manager.
        platformMgmt.InvalidatePlatforms();

        head = null;
        count = 0;
    }
}
