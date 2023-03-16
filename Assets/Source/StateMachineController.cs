using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Extensions;

/// <summary>
/// State Machine Controller Class
/// </summary>
public class StateMachineController
{
    private MonoBehaviour m_owner;
    private List<string> m_states;

    public StateMachineEvent onStateBegin = new StateMachineEvent();
    public StateMachineEvent onStateUpdate = new StateMachineEvent();
    public StateMachineEvent onStateEnd = new StateMachineEvent();

    public bool IsTransitioning { get; private set; }
    public bool TickState { get; set; }
    public string CurrentState { get; private set; }


    public StateMachineController(MonoBehaviour owner)
    {
        m_owner = owner;
        m_states = new List<string>();
        IsTransitioning = false;
    }


    public void Update()
    {
        if (!(CurrentState.IsNullOrEmpty() || CurrentState.IsNullOrWhiteSpace()) && onStateUpdate != null)
        {
            if (TickState)
                onStateUpdate.Invoke(CurrentState);
        }
    }


    public void AddState(string newStateName)
    {
        Debug.Assert(m_states != null);
        if (!m_states.Contains(newStateName))
            m_states.Add(newStateName);
    }


    public void AddState(int index, string newStateName)
    {
        Debug.Assert(m_states != null);

        if (!m_states.Contains(newStateName))
            m_states.Insert(index, newStateName);
    }


    public void GoToState(string newState)
    {
        if (CurrentState != newState)
        {
            IsTransitioning = true;

            if (!CurrentState.IsNullOrEmpty() || !CurrentState.IsNullOrWhiteSpace())
            {
                if (onStateEnd != null)
                    onStateEnd.Invoke(CurrentState);
            }

            CurrentState = newState;

            if (onStateBegin != null)
                onStateBegin.Invoke(CurrentState);

            IsTransitioning = false;
        }
    }


    public MonoBehaviour GetOwner()
    {
        return m_owner;
    }
}

[Serializable]
public class StateMachineEvent : UnityEvent<string> { }

/// <summary>
/// State Class
/// </summary>
public class State
{
    private StateMachineController m_controller;


    public State(StateMachineController controller)
    {
        m_controller = controller;
    }
}
