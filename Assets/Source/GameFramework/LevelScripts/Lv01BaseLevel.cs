// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using UnityEngine;

public class Lv01BaseLevel : BaseLinkedListLevel
{
    [Header("Lv01BaseLevel")]
    public Puzzle puzzle01;
    public WaypointNode start01;
    public WaypointNode goal01;
    public Transform camMoveHint01;


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

    public void AddLastLv4(Action<Platform> onComplete)
    {
        Platform p = mainPuzzle.AddLast(0);
        if (p == null)
        {
            isModified = true;
            if (onComplete != null)
                onComplete.Invoke(null);
            return;
        }
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
