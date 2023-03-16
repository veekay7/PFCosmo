// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;

[ExecuteInEditMode]
public class CamEnableDepth : MonoBehaviour
{
    private Camera m_camera;


    private void Awake()
    {
        m_camera = GetComponent<Camera>();
        m_camera.depthTextureMode = DepthTextureMode.Depth;
    }
}
