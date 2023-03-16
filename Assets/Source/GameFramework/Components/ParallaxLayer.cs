// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    // NOTE: Move this to the camera or make it some global variable
    [SerializeField]
    private bool m_moveParallax = true;
    [SerializeField]
    private float m_scrollSpeed = 0.0f;
    [SerializeField]
    private Camera m_eventCamera = null;
    [SerializeField]
    private bool m_moveInOppositeDirection = false;
    [Tooltip("Open the Layers menu in the Unity Editor and reference the Sorting Layers section for names.")]
    public string sortingLayerName = "Default";

    private Transform m_camTransform;
    private Vector3 m_oldCamPos;
    private bool m_oldMoveParallaxOption;


    private void OnEnable()
    {
        // Since we have the sorting 

        if (m_eventCamera == null)
            m_eventCamera = Camera.main;

        m_camTransform = m_eventCamera.transform;
        m_oldCamPos = m_camTransform.position;
    }


    private void OnValidate()
    {
        RefreshRenderers();
    }


    private void Update()
    {
        if (m_eventCamera == null)
            return;

        if (m_moveParallax && !m_oldMoveParallaxOption)
            m_oldCamPos = m_camTransform.position;

        m_oldMoveParallaxOption = m_moveParallax;

        if (!Application.isPlaying && !m_moveParallax)
            return;

        Vector3 diff = m_camTransform.position - m_oldCamPos;
        float direction = (m_moveInOppositeDirection) ? -1.0f : 1.0f;
        transform.position += (diff * m_scrollSpeed) * direction;

        m_oldCamPos = m_camTransform.position;
    }


    [ContextMenu("Refresh Sprite Renderers")]
    public void RefreshRenderers()
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer r in renderers)
        {
            r.sortingLayerName = sortingLayerName;
        }
    }
}
