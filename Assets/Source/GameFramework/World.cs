// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEditor;
using UnityEngine;

public class World : MonoBehaviourSingleton<World>
{
    [Header("Boundary")]
    [SerializeField]
    private float _top = 100.0f;
    [SerializeField]
    private float _left = -100.0f;
    [SerializeField]
    private float _bottom = -100.0f;
    [SerializeField]
    private float _right = 100.0f;
    [SerializeField]
    private float _killY = -200.0f;

    private ParallaxLayer[] m_parallaxLayers;

    public float Top => _top;
    public float Bottom => _bottom;
    public float Left => _left;
    public float Right => _right;
    public float KillY => _killY;


    protected override void OnEnable()
    {
        base.OnEnable();
    }


    [ContextMenu("Create New Layer")]
    private void CreateNewLayer()
    {
        GameObject newLayerObject = new GameObject();
        newLayerObject.AddComponent<ParallaxLayer>();
        newLayerObject.name = "New Layer";

        newLayerObject.transform.SetParent(transform, false);
        newLayerObject.transform.localPosition = Vector3.zero;
        newLayerObject.transform.localRotation = Quaternion.identity;
        newLayerObject.transform.localScale = Vector3.one;
    }


    [ContextMenu("Refresh Sorting Layers")]
    private void RefreshRendererSortingLayerInLayers()
    {
        m_parallaxLayers = GetComponentsInChildren<ParallaxLayer>();
        for (int i = 0; i < m_parallaxLayers.Length; i++)
        {
            m_parallaxLayers[i].RefreshRenderers();
        }
    }


    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Color prevColor = Gizmos.color;

        // Based on the game world, draw the lines required to infinitum
        // Draw horizontal line first
        Gizmos.color = new Color(0.0f, 0.0f, 1.0f, 1.0f);
        Vector2 topleft = new Vector2()
        {
            x = Left,
            y = Top
        };
        Vector2 topRight = new Vector2()
        {
            x = Right,
            y = Top
        };
        Vector2 bottomLeft = new Vector2()
        {
            x = Left,
            y = Bottom
        };
        Vector2 bottomRight = new Vector2()
        {
            x = Right,
            y = Bottom
        };

        // Draw horizontal lines
        Gizmos.DrawLine(topleft, topRight);
        Gizmos.DrawLine(bottomLeft, bottomRight);

        // Draw vertical lines
        Gizmos.DrawLine(topleft, bottomLeft);
        Gizmos.DrawLine(topRight, bottomRight);

        // Now draw the KillY line

        Camera activeCam = null;
        if (Globals.isEditMode)
            activeCam = SceneView.lastActiveSceneView.camera;
        else
            activeCam = Camera.main;

        Gizmos.color = Color.red;
        Vector3 from = new Vector3();
        from.x = activeCam.transform.position.x * -1500.0f;
        from.y = KillY;

        Vector3 to = new Vector3();
        to.x = activeCam.transform.position.x * 1500.0f;
        to.y = KillY;

        Gizmos.DrawLine(from, to);

        Gizmos.color = prevColor;
#endif
    }
}
