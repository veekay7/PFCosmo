// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;

public struct CameraFocusBorder
{
    private Vector2 m_center;
    private Vector2 m_velocity;
    private float m_left, m_right;
    private float m_top, m_bottom;

    public Vector2 center => m_center;
    public Vector2 velocity => m_velocity;
    public float left => m_left;
    public float right => m_right;
    public float top => m_top;
    public float bottom => m_bottom;


    public CameraFocusBorder(Bounds targetBounds, Vector2 size)
    {
        m_left = targetBounds.center.x - size.x * 0.5f;
        m_right = targetBounds.center.x + size.x * 0.5f;
        m_bottom = targetBounds.min.y;
        m_top = targetBounds.min.y + size.y;

        m_center = new Vector2();
        m_center.x = (m_left + m_right) * 0.5f;
        m_center.y = (m_top + m_bottom) * 0.5f;

        m_velocity = Vector2.zero;
    }


    public void Update(Bounds targetBounds)
    {
        // Update X position
        float shiftX = 0.0f;
        if (targetBounds.min.x < m_left)
        {
            shiftX = targetBounds.min.x - m_left;
        }
        else if (targetBounds.max.x > m_right)
        {
            shiftX = targetBounds.max.x - m_right;
        }

        m_left += shiftX;
        m_right += shiftX;

        // Update Y position
        float shiftY = 0.0f;
        if (targetBounds.min.y < m_bottom)
        {
            shiftY = targetBounds.min.y - m_bottom;
        }
        else if (targetBounds.max.y > m_top)
        {
            shiftY = targetBounds.max.y - m_top;
        }

        m_top += shiftY;
        m_bottom += shiftY;

        m_center.x = (m_left + m_right) * 0.5f;
        m_center.y = (m_top + m_bottom) * 0.5f;
        m_velocity = new Vector2(shiftX, shiftY);
    }
}