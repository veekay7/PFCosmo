// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using System.Collections.Generic;
using UnityEngine;

public class Lv02BaseLevel : BaseLinkedListLevel
{
    [Header("Lv02BaseLevel")]
    public Puzzle puzzle01;
    public WaypointNode start01;
    public WaypointNode goal01;
    public Transform camMoveHint01;
    public TriggerArea portalArea;
    public List<Collectible> crystals = new List<Collectible>();


    protected override void Awake()
    {
        base.Awake();
    }


    protected override void Start()
    {
        // Gets the camera and player
        base.Start();

        // Set the main puzzle as puzzle01
        mainPuzzle = puzzle01;
        start = start01;
        goal = goal01;

        portalArea.gameObject.SetActive(false);
        gameState.totalCrystals = crystals.Count;
        for (int i = 0; i < crystals.Count; i++)
        {
            crystals[i].gameObject.SetActive(false);
        }
    }


    public override void AddFirst(Action<Platform> onComplete)
    {
        int value = valueManager.GetValue();
        if (value == 0)
        {
            if (onComplete != null)
                onComplete.Invoke(null);
            return;
        }

        Platform p = mainPuzzle.AddFirst(0);
        if (p == null)
        {
            if (onComplete != null)
                onComplete.Invoke(null);
            return;
        }

        p.SetValue(value);
        valueManager.UseValue(value);

        StartCoroutine(mainPuzzle.Co_ArrangePlatformsToSlotsAnim(p, true, () =>
        {
            isModified = true;
            if (onComplete != null)
                onComplete.Invoke(p);
        }));
    }


    public override void AddLast(Action<Platform> onComplete)
    {
        int value = valueManager.GetValue();
        if (value == 0)
        {
            if (onComplete != null)
                onComplete.Invoke(null);
            return;
        }

        Platform p = mainPuzzle.AddLast(0);
        if (p == null)
        {
            isModified = true;
            if (onComplete != null)
                onComplete.Invoke(null);
            return;
        }

        p.SetValue(value);
        valueManager.UseValue(value);

        // TODO: Temporary place this here, we need to move this elsewhere
        StartCoroutine(mainPuzzle.Co_ArrangePlatformsToSlotsAnim(p, true, () =>
        {
            if (onComplete != null)
                onComplete.Invoke(p);
        }));
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
}
