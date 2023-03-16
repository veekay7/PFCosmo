// Copyright 2019 Nanyang Technological University.
// Purpose: Used to store a reference to a platform
// Author: VinTK
using UnityEngine;

public class PlatformSlot : MonoBehaviour
{
    [ReadOnly]
    public Platform m_platform = null;

    public SpriteRenderer spriteRenderer { get; private set; }
    public bool isEmpty => (m_platform == null);


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void SetPlatform(Platform platform)
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        m_platform = platform;

        if (m_platform != null)
            spriteRenderer.enabled = false;
        else
            spriteRenderer.enabled = true;
    }


    public Platform GetPlatform()
    {
        return m_platform;
    }
}
