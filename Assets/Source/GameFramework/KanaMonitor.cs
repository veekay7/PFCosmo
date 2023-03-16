// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Purpose: This object always displays the Glyph of the platform it is pointing to.
// Author: VinTK
using UnityEngine;

public class KanaMonitor : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_kanaRenderer = null;


    public void SetReferencingPlatform(Platform platform)
    {
        Debug.Assert(m_kanaRenderer != null, "Glyph Renderer is null");

        if (platform == null)
        {
            m_kanaRenderer.sprite = null;
        }
        else
        {
            if (platform.kana != null)
                m_kanaRenderer.sprite = platform.kana.sprite;
        }
    }
}
