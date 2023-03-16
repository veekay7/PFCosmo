// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Reference:
// http://wiki.unity3d.com/index.php/LineRenderer_Rope
// https://en.wikibooks.org/wiki/Cg_Programming/Unity/B%C3%A9zier_Curves
// Author: VinTK
using System;
using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private const int m_segments = 16;

    [ReadOnly]
    public Puzzle owner;
    [SerializeField]
    private PlatformInfoFukidashi m_fukidashi = null;
    [SerializeField]
    private WaypointNode m_waypoint = null;
    [SerializeField]
    private Platform m_next;

    [SerializeField, ReadOnly]
    private Kana m_kana = null;
    [SerializeField, ReadOnly]
    private int m_value = 0;

    public BoxCollider2D colliderComponent { get; private set; }
    public LineRenderer line { get; private set; }
    
    public WaypointNode waypoint => m_waypoint;
    public PlatformInfoFukidashi fukidashi => m_fukidashi;
    public bool isMoving { get; private set; }
    public bool forceHideFukidashi { get; set; }


    private void OnEnable()
    {
        colliderComponent = GetComponent<BoxCollider2D>();

        line = GetComponent<LineRenderer>();
        line.useWorldSpace = true;
        line.textureMode = LineTextureMode.RepeatPerSegment;
    }


    private void Start()
    {
        forceHideFukidashi = false;

        // When the game starts, check to see if the platform has a next connection or not
        if (m_next != null)
            m_waypoint.next = m_next.waypoint;
        else
            m_waypoint.next = null;
    }


    public Kana kana
    {
        get { return m_kana; }
    }


    public Platform next
    {
        get { return m_next; }
        set
        {
            m_next = value;
            if (m_next != null)
                m_waypoint.next = m_next.waypoint;
            else
                m_waypoint.next = null;
        }
    }


    public int value
    {
        get { return m_value; }
        private set { m_value = value; }
    }


    private void LateUpdate()
    {
        MakeBezierLine();

        if (kana != null)
        {
            m_fukidashi.Show();
            if (forceHideFukidashi)
                m_fukidashi.Hide();
        }
        else
        {
            m_fukidashi.Hide();
        }
    }


    public void SetKana(Kana newKana)
    {
        // If there was a kana before, we must unuse it
        if (m_kana != null)
        {
            m_kana.isUsed = false;
            m_fukidashi.SetKana(null);
        }

        m_kana = newKana;
        if (m_kana != null)
        {
            m_fukidashi.SetKana(m_kana.sprite);
            m_kana.isUsed = true;
        }
    }


    public void SetValue(int value)
    {
        Debug.Assert(value >= 0, "Value inserted cannot be less than 0.");
        m_value = value;
        m_fukidashi.SetValue(m_value);
    }


    [ContextMenu("Invalidate")]
    public void Invalidate()
    {
        if (kana != null)
            kana.isUsed = false;

        name = "Platform";
        SetKana(null);
        SetValue(0);
        next = null;
    }


    public IEnumerator Co_MoveBerp(Vector3 targetPosition, Action onComplete = null)
    {
        // Disable the collider first
        //boxCollider.enabled = false;
        isMoving = true;

        float lerpSpeed = 0.8f;
        float t = 0.0f;
        Vector3 start = transform.position;
        while (true)
        {
            t += Time.deltaTime * (1.0f / lerpSpeed);
            transform.position = Interpolation.Berp(start, targetPosition, t);
            if (t >= 1.0f)
                break;
            yield return null;
        }

        //boxCollider.enabled = true;
        isMoving = false;

        if (onComplete != null)
            onComplete();
    }


    public IEnumerator Co_MoveSinerp(Vector3 targetPosition, Action onComplete = null)
    {
        //boxCollider.enabled = false;
        isMoving = true;

        float lerpSpeed = 0.8f;
        float t = 0.0f;
        Vector3 start = transform.position;
        while (true)
        {
            t += Time.deltaTime * (1.0f / lerpSpeed);
            transform.position = Interpolation.Sinerp(start, targetPosition, t);
            if (t >= 1.0f)
                break;
            yield return null;
        }

        //boxCollider.enabled = true;
        isMoving = false;

        if (onComplete != null)
            onComplete();
    }


    public float GetHalfHeight()
    {
        float output = colliderComponent.size.y * 0.5f;
        return output;
    }


    public float GetHeight()
    {
        float output = colliderComponent.size.y;
        return output;
    }


    [ContextMenu("Resize Collider")]
    private void ResizeCollider()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        BoxCollider2D colliderComponent = GetComponent<BoxCollider2D>();

        if (spriteRenderer.sprite != null)
        {
            Vector2 size = spriteRenderer.sprite.bounds.size;
            colliderComponent.size = size;
            colliderComponent.offset = new Vector2(0.0f, 0.0f);
        }
    }


    //public float scale = 1.0f;
    private void MakeBezierLine()
    {
        if (line == null || next == null)
        {
            line.enabled = false;
            return;
        }

        line.enabled = true;
        line.positionCount = m_segments;

        // Create the link's curve
        Vector3 p0 = Vector3.zero;
        Vector3 p1 = Vector3.zero;
        Vector3 p2 = Vector3.zero;

        p0 = transform.position;
        p2 = m_next.transform.position;

        // Before determining the sign of the horizontal component of the middle point, check the sign of the p0.xx.
        // If the sign of p0.x is negative, the platform is on the left, we should move p1.x towards the left.
        // If the sign of p0.x is positive, the platform is on the right. we should move p1.x towards the right.
        //float sign = Mathf.Sign(p0.x);
        float midpoint = (p2.x + p0.x) * 0.5f;
        float distance = (p2.x - p0.x);
        float heightDiff = p2.y - p0.y;
        float sign = 1.0f;
        if (heightDiff > 16.0f)
            sign = -1.0f;
        else
            sign = 1.0f;

        //float height = (p2.y - p0.y > 0.0f) ? (p2.y - p0.y) : p2.y;
        p1 = (Vector2.right * midpoint) + (Vector2.down * sign * distance * 0.5f);

        // Do Bezier interpolation
        float t = 0.0f;
        Vector3 currentPosition = Vector3.zero;
        for (int i = 0; i < m_segments; i++)
        {
            t = i / (m_segments - 1.0f);
            currentPosition = (1.0f - t) * (1.0f - t) * p0 +
                2.0f * (1.0f - t) * t * p1 + t * t * p2;
            line.SetPosition(i, currentPosition);
        }
    }


    Vector2 m_currentLinkTexOffset = Vector2.zero;
    private void UpdateLineTextureOffset()
    {
        // NOTE: Originally it was gonna be like a light trail/energy flow. But it involved some math
        // for offsetting the line's mesh mainTextureOffset, so I didn't bother.
        // Determine the flow of the link's light trail
        float flowSpeed = 3.0f;
        float maxTextureOffsetLen = 5.0f;

        Vector2 direction = Vector2.zero;
        direction = transform.position - next.transform.position;
        direction.Normalize();

        Vector2 targetTextureOffset = (direction * maxTextureOffsetLen);
        m_currentLinkTexOffset = Vector2.LerpUnclamped(m_currentLinkTexOffset, targetTextureOffset, Time.deltaTime * flowSpeed);

        int maxOffsetMag = Mathf.CeilToInt(targetTextureOffset.magnitude);
        int linkOffsetMag = Mathf.CeilToInt(m_currentLinkTexOffset.magnitude);
        if (maxOffsetMag == linkOffsetMag)
        {
            m_currentLinkTexOffset = Vector2.zero;
        }

        float dirScale = 1.0f;
        if (direction.x > 0.0f || direction.y > 0.0f)
            dirScale = -1.0f;
        line.material.mainTextureOffset = m_currentLinkTexOffset * dirScale;
    }


    private void OnDestroy()
    {
        Invalidate();
    }


    public Vector2 position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }
}
