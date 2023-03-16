// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Authors: VinTK
using UnityEngine;

public class GameState : ScriptableObject
{
    public string puzzleName = string.Empty;
    public Platform headPlatform = null;
    public int collectedCrystals = 0;
    public int totalCrystals = 0;


    public void Clear()
    {
        puzzleName = string.Empty;
        headPlatform = null;
        collectedCrystals = 0;
        totalCrystals = 0;
    }
}
