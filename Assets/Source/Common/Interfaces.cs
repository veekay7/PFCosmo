// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using UnityEngine;

public interface IRayCollidable
{
    void OnEnterRayCollision(GameObject instigator);
    void OnExitRayCollision(GameObject instigator);
}
