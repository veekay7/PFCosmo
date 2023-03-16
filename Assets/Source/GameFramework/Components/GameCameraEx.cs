// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GameCameraEx : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera mainCamera = null;
    public Camera nearCamera = null;
    public Camera farCamera = null;

    [Header("Tracking")]
    public Transform trackingTarget = null;
    public Vector2 cameraBordersSize = Vector2.one;
    public float verticalOffset;

    private bool m_freeMode;
    private bool m_isResetting;
    public float cameraAwayZ;
    private CameraFocusBorder m_focusBorder;
    private Vector2 m_pendingMoveOffset;
    private Vector3 m_oldPosition;


    public Vector2 mainCameraPixelSize
    {
        get
        {
            if (mainCamera == null)
                throw new NullReferenceException("mainCamera");
            Vector2 size = new Vector2
            {
                x = mainCamera.pixelWidth,
                y = mainCamera.pixelHeight
            };
            return size;
        }
    }

    public Vector2 mainCameraPixelSizeScaled
    {
        get
        {
            if (mainCamera == null)
                throw new NullReferenceException("mainCamera");
            Vector2 size = new Vector2
            {
                x = mainCamera.scaledPixelWidth,
                y = mainCamera.scaledPixelHeight
            };
            return size;
        }
    }

    public Vector3 position => transform.position;
    public Quaternion rotation => transform.rotation;
    public Vector3 rotationAngles => transform.rotation.eulerAngles;


    private void Start()
    {
        m_isResetting = false;
        m_freeMode = false;
        m_pendingMoveOffset = Vector2.zero;
        m_oldPosition = Vector2.zero;

        if (mainCamera != null)
        {
            mainCamera.orthographic = true;
            cameraAwayZ = mainCamera.orthographicSize * -1.0f;

            Vector3 pos = mainCamera.transform.localPosition;
            pos.z = cameraAwayZ;
            mainCamera.transform.localPosition = pos;
        }

        if (nearCamera != null)
        {
            nearCamera.transform.localPosition = Vector3.zero;
            Vector3 pos = nearCamera.transform.localPosition;
            pos.z = cameraAwayZ;
            nearCamera.transform.localPosition = pos;
            nearCamera.transform.rotation = Quaternion.identity;
            nearCamera.transform.localScale = Vector3.one;
            nearCamera.orthographic = false;
            nearCamera.transparencySortMode = TransparencySortMode.Orthographic;
        }

        if (farCamera != null)
        {
            farCamera.transform.localPosition = Vector3.zero;
            Vector3 pos = farCamera.transform.localPosition;
            pos.z = cameraAwayZ;
            farCamera.transform.localPosition = pos;
            farCamera.transform.rotation = Quaternion.identity;
            farCamera.transform.localScale = Vector3.one;
            farCamera.orthographic = false;
            farCamera.transparencySortMode = TransparencySortMode.Orthographic;
        }

        if (trackingTarget != null)
        {
            m_focusBorder = new CameraFocusBorder(trackingTarget.GetComponent<Renderer>().bounds, cameraBordersSize);
        }
    }


    private void LateUpdate()
    {
        if (mainCamera == null || farCamera == null || nearCamera == null)
            return;

        // These three behaviours must always be set on each update loop
        mainCamera.orthographic = true;
        nearCamera.orthographic = false;
        farCamera.orthographic = false;

        // Update all three cam Z position
        cameraAwayZ = mainCamera.orthographicSize * -1.0f;

        Vector3 pos = Vector3.zero;
        pos = mainCamera.transform.localPosition;
        pos.z = cameraAwayZ;
        mainCamera.transform.localPosition = pos;

        pos = Vector3.zero;
        pos = nearCamera.transform.localPosition;
        pos.z = cameraAwayZ;
        nearCamera.transform.localPosition = pos;

        pos = Vector3.zero;
        pos = farCamera.transform.localPosition;
        pos.z = cameraAwayZ;
        farCamera.transform.localPosition = pos;

        HandleFreeMove();
        HandleTargetTracking();
    }


    private void OnDrawGizmos() 
    {
        //Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.5f);
        //Gizmos.DrawCube(m_focusBorder.center, cameraBordersSize);
    }


    public bool IsResetting()
    {
        return m_isResetting;
    }


    public bool IsFreeMode()
    {
        return m_freeMode;
    }


    public void ToggleFreeMode()
    {
        if (m_isResetting)
            return;

        m_freeMode = !m_freeMode;
        if (m_freeMode)
        {
            OnFreeMode();
        }
        else
        {
            OnTrackingMode();
        }
    }


    public void SetToFreeMode()
    {
        if (m_freeMode || m_isResetting)
            return;
        OnFreeMode();
    }


    public void SetToTrackingMode()
    {
        if (!m_freeMode || m_isResetting)
            return;
        OnTrackingMode();
    }


    public float GetFOV(float orthoSize, float distFromOrigin)
    {
        // Orthographic size
        float a = orthoSize;

        // Distance from origin
        float b = Mathf.Abs(distFromOrigin);

        float fov = Mathf.Atan(a / b) * Mathf.Rad2Deg * 2.0f;
        return fov;
    }


    private void OnFreeMode()
    {
        m_freeMode = true;
        m_oldPosition = transform.position;
    }


    private void OnTrackingMode(Action onComplete = null)
    {
        // Reset pending move offset vector
        m_pendingMoveOffset = Vector2.zero;

        m_freeMode = false;
        m_isResetting = true;
        StartCoroutine(ResetToOldPosition());
    }


    private void HandleFreeMove()
    {
        if (!m_freeMode)
            return;

        Vector3 newPosition = transform.position;
        newPosition.x += m_pendingMoveOffset.x * Time.deltaTime;
        newPosition.y += m_pendingMoveOffset.y * Time.deltaTime;
        m_pendingMoveOffset = Vector2.zero;
        transform.position = newPosition;
    }


    private void HandleTargetTracking()
    {
        // Do camera tracking
        if (m_freeMode)
            return;

        if (trackingTarget != null)
        {
            m_focusBorder.Update(trackingTarget.GetComponent<Renderer>().bounds);
        }
    }


    private IEnumerator ResetToOldPosition(Action onComplete = null)
    {
        float rate = 2.0f;
        float t = 0.0f;
        bool isApprox = Mathf.Approximately(transform.position.magnitude, m_oldPosition.magnitude);
        if (!isApprox)
        {
            while (t < 1.0f)
            {
                t += (1.0f / rate) * Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, m_oldPosition, t);
                yield return null;
            }
        }

        m_oldPosition = Vector3.zero;
        m_isResetting = false;

        if (onComplete != null)
            onComplete();
    }


    public void MovePosition(float deltaX, float deltaY)
    {
        if (!m_freeMode)
            return;

        m_pendingMoveOffset.x += deltaX;
        m_pendingMoveOffset.y += deltaY;
    }
}
