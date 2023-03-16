using System;
using System.Collections;
using UnityEngine;

[Serializable]
public struct CameraBounds
{
    public Vector2 center;
    public Vector2 size;
};

public enum ECamMode
{
    None,
    Free,
    Tracking,
    SceneCtrl,
    Resetting
};

[ExecuteInEditMode]
public class GameCamera : MonoBehaviour
{
    [Header("Tracking")]
    public Transform trackingTarget = null;
    public Bounds moveBounds = new Bounds();
    public Vector3 moveBoundsCenterOffset = Vector3.zero;

    //private bool m_freeMode;
    //private bool m_isResetting;
    private Vector2 m_pendingMoveOffset;
    private Vector3 m_oldPosition;
    private Vector3 m_targetOldPos;
    private ECamMode m_mode;
    private Bounds camViewBounds = new Bounds();

    public Vector3 position => transform.position;
    public Vector3 spPosition => mainCamera.WorldToScreenPoint(position);
    public Quaternion rotation => transform.rotation;
    public Vector3 rotationAngles => transform.rotation.eulerAngles;

    public Camera mainCamera { get; private set; }
    //public bool isResetting => m_isResetting;
    public bool isFreeMode => (m_mode == ECamMode.Free);
    public bool isMovingToHint { get; private set; }


    private void OnEnable()
    {
        mainCamera = GetComponent<Camera>();

        //m_isResetting = false;
        //m_freeMode = false;
        m_mode = ECamMode.None;
        m_pendingMoveOffset = Vector2.zero;

        m_oldPosition = transform.position;

        UpdateCameraViewBounds();
        UpdateCamMoveBounds();

        // Store the target's old position
        if (trackingTarget != null)
        {
            m_mode = ECamMode.Tracking;

            Vector3 newCamPos = Vector3.zero;
            if (trackingTarget.position.x > moveBounds.max.x)
                newCamPos.x = trackingTarget.position.x - moveBounds.extents.x;
            else if (trackingTarget.position.x < moveBounds.min.x)
                newCamPos.x = trackingTarget.position.x + moveBounds.extents.x;

            if (trackingTarget.position.y > moveBounds.max.y)
                newCamPos.y = trackingTarget.position.y - moveBounds.extents.y;
            else if (trackingTarget.position.y < moveBounds.min.y)
                newCamPos.y = trackingTarget.position.y + moveBounds.extents.y;

            newCamPos.z = transform.position.z;

            m_targetOldPos = trackingTarget.position;
        }
    }


    private void LateUpdate()
    {
        UpdateCameraViewBounds();
        UpdateCamMoveBounds();

        // Move camera by chara distance
        if (m_mode == ECamMode.Tracking && trackingTarget != null)
        {
            // Only do movement if and only if the chara's position has changed
            Vector3 targetPosDiff = trackingTarget.position - m_targetOldPos;
            if (targetPosDiff.sqrMagnitude > 0.0f)
            {
                // Move Horizontal
                if (targetPosDiff.x < 0.0f && trackingTarget.position.x <= moveBounds.min.x)
                    m_pendingMoveOffset.x = targetPosDiff.x / Time.deltaTime;   // going left
                else if (targetPosDiff.x > 0.0f && trackingTarget.position.x >= moveBounds.max.x)
                    m_pendingMoveOffset.x = targetPosDiff.x + (trackingTarget.position.x - moveBounds.max.x) / Time.deltaTime;   // going right
                else
                    m_pendingMoveOffset.x = 0.0f;

                // Move Vertical
                if (targetPosDiff.y < 0.0f && trackingTarget.position.y <= moveBounds.min.y)
                    m_pendingMoveOffset.y = targetPosDiff.y / Time.deltaTime;   // going down
                else if (targetPosDiff.y > 0.0f && trackingTarget.position.y >= moveBounds.max.y)
                    m_pendingMoveOffset.y = targetPosDiff.y / Time.deltaTime;   // going up
                else
                    m_pendingMoveOffset.y = 0.0f;
            }
            else
            {
                m_pendingMoveOffset = Vector2.zero;
            }
        }

        // Update position by offset (velocity)
        Vector3 pos = position;
        pos.x += m_pendingMoveOffset.x * Time.deltaTime;
        pos.y += m_pendingMoveOffset.y * Time.deltaTime;
        transform.position = pos;

        // Do edge collision detection?
        Vector3 newPosition = transform.position;
        EdgeCollisionDetection(ref newPosition);
        transform.position = newPosition;

        m_pendingMoveOffset = Vector2.zero;

        if (trackingTarget != null)
            m_targetOldPos = trackingTarget.position;
    }


    private void OnDrawGizmos()
    {
        Color oldColor = Gizmos.color;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(moveBounds.center, moveBounds.size);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(camViewBounds.center, camViewBounds.size);

        Gizmos.color = oldColor;
    }


    public bool IsTransformWithinBounds(Transform t)
    {
        return t.position.x >= moveBounds.min.x && t.position.x <= moveBounds.max.x &&
                t.position.y >= moveBounds.min.y && t.position.y <= moveBounds.max.y;
    }


    public void EnableSceneCtrlMode()
    {
        m_mode = ECamMode.SceneCtrl;
    }


    public void DisableSceneCtrlMode()
    {
        m_mode = ECamMode.Tracking;
    }


    public void ToggleFreeMode()
    {
        if (m_mode == ECamMode.Resetting || m_mode == ECamMode.SceneCtrl)
            return;

        if (m_mode == ECamMode.Free)
        {
            // Reset pending move offset vector
            m_pendingMoveOffset = Vector2.zero;
            m_mode = ECamMode.Resetting;

            StartCoroutine(Co_ResetCamToTarget(() => {
                m_mode = ECamMode.Tracking;
            }));
        }
        else if (m_mode == ECamMode.Tracking)
        {
            m_mode = ECamMode.Free;

            // Store the current position as the position we will be returning to after turning off free mode
            m_oldPosition = transform.position;
        }
    }


    public void MovePosition(float deltaX, float deltaY)
    {
        if (m_mode != ECamMode.Free)
            return;
        m_pendingMoveOffset.x += deltaX;
        m_pendingMoveOffset.y += deltaY;
    }


    public IEnumerator Co_MoveCamToLocation(Vector3 location, Action onComplete = null)
    {
        float rate = 0.5f;
        float t = 0.0f;
        Vector3 startPos = transform.position;
        location.z = startPos.z;
        while (true)
        {
            t += (1.0f / rate) * Time.deltaTime;
            transform.position = Interpolation.Sinerp(startPos, location, t);
            if (t >= 1.0f)
                break;
            yield return null;
        }

        if (onComplete != null)
            onComplete.Invoke();
    }


    public IEnumerator Co_ResetCamToTarget(Action onComplete = null)
    {
        if (trackingTarget == null)
            yield break;

        float rate = 1.25f;
        float t = 0.0f;
        Vector3 start = transform.position;
        Vector3 end = Vector3.zero;
        if (trackingTarget.position.x > moveBounds.max.x)
            end.x = trackingTarget.position.x - moveBounds.extents.x;
        else if (trackingTarget.position.x < moveBounds.min.x)
            end.x = trackingTarget.position.x + moveBounds.extents.x;
        else
            end.x = trackingTarget.position.x + moveBounds.extents.x;

        if (trackingTarget.position.y > moveBounds.max.y)
            end.y = trackingTarget.position.y - moveBounds.extents.y;
        else if (trackingTarget.position.y < moveBounds.min.y)
            end.y = trackingTarget.position.y + moveBounds.extents.y;
        else
            end.y = trackingTarget.position.y;

        end.z = start.z;

        while (true)
        {
            t += (1.0f / rate) * Time.deltaTime;
            transform.position = Interpolation.Sinerp(start, end, t);
            if (t >= 1.0f)
                break;
            yield return null;
        }

        if (onComplete != null)
            onComplete.Invoke();
    }


    public IEnumerator Co_ResetToInitialPos(Action onComplete = null)
    {
        float rate = 0.5f;
        float t = 0.0f;
        Vector3 startPos = transform.position;
        while (true)
        {
            t += (1.0f / rate) * Time.deltaTime;
            transform.position = Interpolation.Sinerp(startPos, m_oldPosition, t);
            if (t >= 1.0f)
                break;
            yield return null;
        }

        if (onComplete != null)
            onComplete.Invoke();
    }


    private void HandleFreeMove()
    {
        if (m_mode != ECamMode.Free)
            return;

        Vector3 newPosition = transform.position;
        newPosition.x += m_pendingMoveOffset.x * Time.deltaTime;
        newPosition.y += m_pendingMoveOffset.y * Time.deltaTime;
    }


    private void UpdateCameraViewBounds()
    {
        if (camViewBounds == null)
            camViewBounds = new Bounds();

        if (mainCamera == null)
            mainCamera = GetComponent<Camera>();

        // Update camera bounds
        camViewBounds.center = position;

        // Referenced from:
        // https://answers.unity.com/questions/501893/calculating-2d-camera-bounds.html
        //https://answers.unity.com/questions/230190/how-to-get-the-width-and-height-of-a-orthographic.html
        Vector3 spRectExtents = new Vector3(
                mainCamera.orthographicSize * Screen.width / Screen.height,
                mainCamera.orthographicSize,
                0.0f);
        camViewBounds.extents = spRectExtents;
    }


    private void UpdateCamMoveBounds()
    {
        // Update the move bounds
        moveBounds.center = position + moveBoundsCenterOffset;

        Vector3 extents = moveBounds.extents;
        extents.z = moveBounds.center.z;
        moveBounds.extents = extents;
    }


    private void EdgeCollisionDetection(ref Vector3 pos)
    {
        UpdateCameraViewBounds();
        UpdateCamMoveBounds();

        if (World.instance == null)
            return;

        if (camViewBounds.min.x < World.instance.Left)
        {
            pos.x += (World.instance.Left - camViewBounds.min.x);
        }

        if (camViewBounds.max.x > World.instance.Right)
        {
            pos.x -= (camViewBounds.max.x - World.instance.Right);
        }

        if (camViewBounds.min.y < World.instance.Bottom)
        {
            pos.y += (World.instance.Bottom - camViewBounds.min.y);
        }

        if (camViewBounds.max.y > World.instance.Top)
        {
            pos.y -= (camViewBounds.max.y - World.instance.Top);
        }
    }
}
