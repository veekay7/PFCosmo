// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System.Collections.Generic;
using UnityEngine;
using PF;

public class ActionBehaviour : MonoBehaviour
{
    [HideInInspector]
    public List<ActionData> actions = new List<ActionData>();

    public bool isActivated { get; private set; } = false;
    public GameObject owner { get; private set; }


    protected virtual void Awake() { }
    protected virtual void Start() { }


    protected void Update()
    {
        if (owner == null)
            return;
        Tick();
    }


    public virtual void SetOwner(GameObject owner)
    {
        Debug.Assert(owner != null, "owner is null");
        this.owner = owner;
    }


    /// <summary>
    /// The update loop tick for this action once its activated
    /// </summary>
    protected virtual void Tick() { }


    /// <summary>
    /// Stops any pending input which blocks progression
    /// </summary>
    public virtual void StopPendingInputs() { }


    protected void ResetStatistics()
    {
        for (int i = 0; i < actions.Count; i++)
        {
            actions[i].useCount = 0;
        }
    }
}
