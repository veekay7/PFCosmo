// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;

public class SineWaveMovement : MonoBehaviour
{
    public float speed = 1.0f;
    public float amplitude = 1.0f;

    private Vector3 m_startPosition;


    private void Start()
    {
        m_startPosition = transform.localPosition;
    }


    private void FixedUpdate()
    {
        float sin = Mathf.Sin(Time.realtimeSinceStartup * speed) * amplitude;
        Vector3 newPosition = transform.up * sin + m_startPosition;
        transform.localPosition = newPosition;
    }
}
