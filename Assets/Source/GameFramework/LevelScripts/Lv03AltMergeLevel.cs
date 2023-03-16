// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using System.Collections.Generic;
using UnityEngine;

public class Lv03AltMergeLevel : BaseLinkedListLevel
{
    [Header("Lv03AltMerge")]
    public Puzzle puzzle01;
    public Puzzle puzzle02;
    public WaypointNode start01;
    public WaypointNode goal01;
    public WaypointNode start02;
    public WaypointNode goal02;
    public Transform camMoveHint01;
    public TriggerArea portalArea;
    public List<Collectible> crystals = new List<Collectible>();

    private List<int> m_puzzleWinValues01 = new List<int>();
    private List<int> m_puzzleWinValues02 = new List<int>();


    protected override void Awake()
    {
        base.Awake();
    }


    protected override void Start()
    {
        // Gets the camera and player
        base.Start();

        // Configure the level
        int[] puzzle01InitialValues = { 1, 2, 3 };
        for (int i = 0; i < puzzle01InitialValues.Length; i++)
        {
            PlatformSlot s = puzzle01.slotsCtrl.Get(i);
            Platform p = s.GetPlatform();

            Kana kana = kanaTable.GetUnusedKana();
            p.SetKana(kana);

            int value = 0;
            value = valueManager.GetValueAndUse(puzzle01InitialValues[i]);
            p.SetValue(value);
        }

        int[] puzzle02InitialValues = { 4, 5, 6, 7 };
        for (int i = 0; i < puzzle02InitialValues.Length; i++)
        {
            PlatformSlot s = puzzle02.slotsCtrl.Get(i);
            Platform p = s.GetPlatform();

            Kana kana = kanaTable.GetUnusedKana();
            p.SetKana(kana);

            int value = 0;
            value = valueManager.GetValueAndUse(puzzle02InitialValues[i]);
            p.SetValue(value);
        }

        portalArea.gameObject.SetActive(false);
        gameState.totalCrystals = crystals.Count;
        for (int i = 0; i < crystals.Count; i++)
        {
            crystals[i].gameObject.SetActive(false);
        }

        // Create the portal and crystal unlock condition
        int puzzle02Idx = 0;
        for (int i = 0; i < puzzle01InitialValues.Length; i++)
        {
            int value = puzzle01InitialValues[i];
            int altValue = puzzle02InitialValues[puzzle02Idx];
            m_puzzleWinValues01.Add(value);
            m_puzzleWinValues01.Add(altValue);
            puzzle02Idx++;
        }

        if (puzzle02Idx < puzzle02InitialValues.Length)
        {
            for (int i = puzzle02Idx; i < puzzle02InitialValues.Length; i++)
            {
                int value = puzzle02InitialValues[i];
                m_puzzleWinValues02.Add(value);
            }
        }

        mainPuzzle = puzzle01;
        start = start01;
        goal = goal01;
    }


    protected override void Update()
    {
        base.Update();
    }


    public override void AddAt(int index, Action<Platform> onComplete)
    {
        Platform p = mainPuzzle.AddAt(index, 0);
        if (p == null)
        {
            if (onComplete != null)
                onComplete.Invoke(null);
            return;
        }

        StartCoroutine(puzzle01.Co_ArrangePlatformsToSlotsAnim(p, true, () =>
        {
            isModified = true;
            if (onComplete != null)
                onComplete.Invoke(p);
        }));
    }


    public override void AddFirst(Action<Platform> onComplete)
    {
        // Try get value from player inventory
        if (player.inventory.Get() == 0)
        {
            if (onComplete != null)
                onComplete.Invoke(null);
        }

        Platform p = mainPuzzle.AddFirst(0);
        if (p == null)
        {
            if (onComplete != null)
                onComplete.Invoke(null);
            return;
        }

        int value = player.inventory.Get();
        p.SetValue(value);
        player.inventory.Clear();
        //valueManager.UseValue(value);

        StartCoroutine(mainPuzzle.Co_ArrangePlatformsToSlotsAnim(p, true, () =>
        {
            isModified = true;
            if (onComplete != null)
                onComplete.Invoke(p);
        }));
    }


    public override void AddLast(Action<Platform> onComplete)
    {
        // Try get value from player inventory
        if (player.inventory.Get() == 0)
        {
            if (onComplete != null)
                onComplete.Invoke(null);
        }

        Platform p = mainPuzzle.AddLast(0);
        if (p == null)
        {
            if (onComplete != null)
                onComplete.Invoke(null);
            return;
        }

        int value = player.inventory.Get();
        p.SetValue(value);
        player.inventory.Clear();
        //valueManager.UseValue(value);

        // TODO: Temporary place this here, we need to move this elsewhere
        StartCoroutine(mainPuzzle.Co_ArrangePlatformsToSlotsAnim(p, true, () =>
        {
            isModified = true;
            if (onComplete != null)
                onComplete.Invoke(p);
        }));
    }


    public override void RemoveAt(int index, Action<Platform> onComplete)
    {
        Platform p = mainPuzzle.RemoveAt(index);
        if (p == null)
        {
            if (onComplete != null)
                onComplete.Invoke(null);
            return;
        }

        // After remove, we move the node that we are removing back to origin
        StartCoroutine(mainPuzzle.Co_PlayHidePlatformAnim(p, () =>
        {
            StartCoroutine(mainPuzzle.Co_ArrangePlatformsToSlotsAnim(null, false, () =>
            {
                isModified = true;
                if (onComplete != null)
                    onComplete.Invoke(p);
            }));
        }));
    }


    public override void CheckSolution()
    {
        base.CheckSolution();

        //// Get a list of all active platforms in the main puzzle and check the number of active platforms available
        //List<Platform> puzzle01ActivePlatforms = puzzle01.m_activePlatforms;
        //int currentActiveCount = puzzle01ActivePlatforms.Count;

        //// CONDITION: If the number of active platforms in the level is less than total amount of slots, solution not complete.
        //if (currentActiveCount < mainPuzzle.slotsCtrl.Count)
        //{
        //    isSolved = false;
        //    return;
        //}

        // Check the puzzle01 list first
        if (puzzle01.count == m_puzzleWinValues01.Count)
        {
            int idx = 0;
            Platform current = puzzle01.head;
            while (current != null)
            {
                if (current.value != m_puzzleWinValues01[idx])
                {
                    isSolved = false;
                    return;
                }

                current = current.next;
                idx++;
            }
        }
        else
        {
            isSolved = false;
            return;
        }

        // Check puzzle02 list
        if (puzzle02.count == m_puzzleWinValues02.Count)
        {
            int idx = 0;
            Platform current = puzzle02.head;
            while (current != null)
            {
                if (current.value != m_puzzleWinValues02[idx])
                {
                    isSolved = false;
                    return;
                }

                current = current.next;
                idx++;
            }
        }
        else
        {
            isSolved = false;
            return;
        }

        isSolved = true;

        portalArea.gameObject.SetActive(true);
        gameState.totalCrystals = crystals.Count;
        for (int i = 0; i < crystals.Count; i++)
        {
            crystals[i].gameObject.SetActive(true);
        }

        PlayVictoryChime();
    }
}
