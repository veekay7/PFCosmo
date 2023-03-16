// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;

public static class Globals
{
    public static readonly int MaxActionCount = 16;
    public static readonly int MaxNodesPerPuzzle = 10;
    public static readonly float KINDA_SMALL_NUMBER = 0.00001f;

    public static bool alertBoxIsShown { get; set; } = false;
    public static bool isEditMode => (Application.isEditor && !Application.isPlaying);
}
