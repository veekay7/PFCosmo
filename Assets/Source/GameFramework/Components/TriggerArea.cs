// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Authors: VinTK
using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class TriggerArea : MonoBehaviour
{
    [SerializeField]
    private GameObject _instigatorFilter = null;

    public CollisionAreaEvent onTriggerTouched = new CollisionAreaEvent();
    public CollisionAreaEvent onTriggerUntouched = new CollisionAreaEvent();

    public BoxCollider2D colliderComponent { get; private set; }


    private void Awake()
    {
        colliderComponent = GetComponent<BoxCollider2D>();
        colliderComponent.isTrigger = true;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        bool cond1 = _instigatorFilter != null && other.gameObject != _instigatorFilter;
        bool cond2 = transform.parent == other.transform;
        if (cond1 || cond2)
            return;

        if (onTriggerTouched != null)
            onTriggerTouched.Invoke(other);
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        bool cond1 = _instigatorFilter != null && other.gameObject != _instigatorFilter;
        bool cond2 = transform.parent == other.transform;
        if (cond1 || cond2)
            return;

        if (onTriggerUntouched != null)
            onTriggerUntouched.Invoke(other);
    }
}

[Serializable]
public class CollisionAreaEvent : UnityEvent<Collider2D> { }