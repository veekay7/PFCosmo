// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;

public abstract class CollectibleEffect : ScriptableObject
{
    protected Collectible m_collectible;


    public virtual void Init(Collectible collectible)
    {
        m_collectible = collectible;
    }

    public abstract void Apply(GameObject instigator);
}
