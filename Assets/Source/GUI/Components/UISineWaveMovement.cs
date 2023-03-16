// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;

public class UISineWaveMovement : MonoBehaviour
{
    public float speed = 1.0f;
    public float amplitude = 1.0f;

    private Vector3 m_initialPosition;

    public RectTransform rectTransform { get; private set; }


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }


    private void Start()
    {
        m_initialPosition = rectTransform.position;
    }


    private void FixedUpdate()
    {
        float sin = Mathf.Sin(Time.realtimeSinceStartup * speed) * amplitude;
        Vector3 newPosition = rectTransform.up * sin + m_initialPosition;
        rectTransform.position = newPosition;
    }
}
