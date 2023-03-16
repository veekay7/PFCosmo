// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;
using UnityEngine.Events;

// Tells entities how to reach this waypoint
public enum EWaypointTravelHint
{
    None,
    Jump,
    Walk
};

[RequireComponent(typeof(CircleCollider2D))]
public class WaypointNode : MonoBehaviour
{
    [Header("Object")]
    [SerializeField]
    private EWaypointTravelHint m_hint = EWaypointTravelHint.None;
    [SerializeField]
    private WaypointNode m_next = null;

    public EWaypointTravelHint hint => m_hint;

    public WaypointNode next
    {
        get { return m_next; }
        set { m_next = value; }
    }


    private void OnDrawGizmos()
    {
        Color oldColor = Gizmos.color;

        Gizmos.color = Color.blue;
        Gizmos.DrawIcon(transform.position, "T_Gizmos_Waypoint.png", true);

        if (next != null)
        {
            Vector3 origin = transform.position;
            Vector3 to = next.transform.position;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origin, to);
        }
        Gizmos.color = oldColor;
    }


    public void SetHint(EWaypointTravelHint hint)
    {
        m_hint = hint;
    }
}

public class WaypointNodeEvent : UnityEvent<GameObject> { }