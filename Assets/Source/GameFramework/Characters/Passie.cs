// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;

public class Passie : Character
{
    [SerializeField, ReadOnly]
    private Platform m_platform;
    private Vector2 m_linkFlowOffset;

    public LineRenderer lineRenderer { get; private set; }
    public SineWaveMovement movement { get; private set; }
    public SpriteRenderer addressDisplayRenderer { get; private set; }

    public Platform platform
    {
        get { return m_platform; }
        private set { m_platform = value; }
    }


    protected override void Awake()
    {
        base.Awake();

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.textureMode = LineTextureMode.Tile;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
        lineRenderer.enabled = false;

        movement = GetComponent<SineWaveMovement>();

        Transform addressDisplayTransform = transform.GetChild(1);
        addressDisplayRenderer = addressDisplayTransform.GetComponent<SpriteRenderer>();
    }


    protected override void Start()
    {
        base.Start();

        platform = null;
        addressDisplayRenderer.sprite = null;
        m_linkFlowOffset = Vector2.zero;
    }


    protected override void Update()
    {
        base.Update();

        if (platform == null)
        {
            lineRenderer.enabled = false;
            return;
        }

        // The line renderer's origin position is always set to the entity's world position
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, platform.transform.position);

        float flowSpeed = 3.0f;
        float maxOffset = 5.0f;
        m_linkFlowOffset.x += Time.deltaTime * flowSpeed;
        if (m_linkFlowOffset.x > maxOffset)
        {
            m_linkFlowOffset = Vector2.zero;
        }

        lineRenderer.material.mainTextureOffset = m_linkFlowOffset;
    }


    public void PointTo(Platform newPlatform)
    {
        if (newPlatform != platform)
        {
            platform = newPlatform;
            if (platform == null)
            {
                addressDisplayRenderer.sprite = null;
                return;
            }

            addressDisplayRenderer.sprite = platform.kana.sprite;
        }
    }
}
