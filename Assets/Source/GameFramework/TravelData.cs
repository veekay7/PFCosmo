// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Authors: VinTK
using UnityEngine;

public struct TravelData
{
    public BaseLinkedListLevel level;
    public WaypointNode start;
    public WaypointNode end;
    public Platform head;
    public Platform tail;
    public WaypointNode current;

    public void Clear()
    {
        level = null;
        start = null;
        end = null;
    }
}
