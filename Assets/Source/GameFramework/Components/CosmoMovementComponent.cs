// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Authors: VinTK, Shi Hezi (The greatest, and most noble and absolute genius)
using UnityEngine;
using UnityEngine.Extensions;
using UnityEngine.Events;
using System.Collections;

public class CosmoMovementComponent : MonoBehaviour
{
    public enum ECosmoMovementMode
    {
        Nothing,
        Jump,
        Walk
    }

    [Header("Physics - General")]
    public float height = 1.5f;
    public bool enableGravity = true;
    public bool enableCollisionDetection = true;

    [Header("Physics - Running")]
    public float runAcceleration = 0.5f;
    public float runMaxSpeed = 6.0f;

    [Header("Physics - Air")]
    public float jumpPower = 1.0f;

    [Header("Physics - Gravity")]
    public float gravityScale = 1.8f;
    public float terminalSpeed = 16.0f;

    [Header("Events")]
    public UnityEvent onLandAfterLaunch = new UnityEvent();

    private GameObject m_owner;
    private float m_launchTime;
    private Vector3 m_launchVelocity;
    private Vector3 m_launchTargetPosition;

    [Header("Internal (Do not change)")]
    public bool onGround;
    public ECosmoMovementMode movementMode;
    public float m_groundAngle;
    public Vector3 m_gravity;
    public Vector3 m_velocity;

    public BoxCollider2D colliderComponent { get; private set; }


    private void Awake()
    {
        colliderComponent = GetComponent<BoxCollider2D>();
    }


    private void Start()
    {
        onGround = false;
        movementMode = ECosmoMovementMode.Nothing;

        m_groundAngle = 0.0f;
        m_launchTime = 0.0f;
        m_launchVelocity = Vector3.zero;
        m_gravity = Vector3.zero;
        m_velocity = Vector3.zero;

        StopMovementImmediately();
    }


    private void Update()
    {
        // Do gravity and and collision detection
        Vector3 position = transform.position;
        if (onGround)
        {
            m_gravity = Vector3.zero;
            TrackGround(ref position);
        }
        else
        {
            if (enableGravity)
            {
                m_gravity = Vector3.down * gravityScale;
                if (m_velocity.y > -terminalSpeed)
                    m_velocity += m_gravity * Time.deltaTime;
            }

            CollisionDetection_Vert(ref position);
        }

        switch (movementMode)
        {
            case ECosmoMovementMode.Walk:
                {
                    if (m_velocity.magnitude < runMaxSpeed)
                    {
                        m_velocity.x += runAcceleration * Mathf.Cos(m_groundAngle);
                        m_velocity.y += runAcceleration * Mathf.Sin(m_groundAngle);
                    }
                }

                break;

            case ECosmoMovementMode.Jump:
                {
                    Fall();

                    float time = (Time.time - m_launchTime);
                    Vector3 grv = (Vector3.down * gravityScale);

                    //GetDeltaHeightInLaunch(time, grv);

                    Vector3 newVelocity = m_launchVelocity + grv * time;
                    m_velocity = newVelocity;
                }

                break;
        }

        // If the magnitude of the velocity is too small, just set it to zero.
        if (m_velocity.sqrMagnitude < Globals.KINDA_SMALL_NUMBER * 10.0f)
            m_velocity = Vector3.zero;

        transform.position = position + m_velocity * Time.deltaTime;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, GetFootLevel2D());
    }


    public void Init(GameObject owner)
    {
        m_owner = owner;
    }


    public GameObject GetOwner()
    {
        return m_owner;
    }


    public void WalkTo(Transform target)
    {
        if (movementMode == ECosmoMovementMode.Walk || !onGround)
            return;
        movementMode = ECosmoMovementMode.Walk;
    }


    /// <summary>
    /// Sets a pending launch velocity to launch the character upward.
    /// </summary>
    public void LaunchTo(Transform target)
    {
        if (movementMode == ECosmoMovementMode.Jump || !onGround)
        {
            return;
        }

        Vector3 footLevel = GetFootLevel();
        m_launchTargetPosition = target.position;
        m_launchTargetPosition.y = m_launchTargetPosition.y + (m_launchTargetPosition.y * 0.5f);

        //float hypoDist = Vector3.Distance(m_launchTargetPosition, footLevel);
        float heightOpps = m_launchTargetPosition.y - footLevel.y;
        float horzAdj = m_launchTargetPosition.x - footLevel.x;

        //Vector3 h_velocity = Vector3.right * (hypoDist / (2.0f * jumpPower / gravityScale));
        Vector3 h_velocity = Vector3.right * (horzAdj * gravityScale / (jumpPower + Mathf.Sqrt(jumpPower * jumpPower - heightOpps)));

        m_launchVelocity = h_velocity + (Vector3.up * jumpPower);
        m_launchTime = Time.time;

        movementMode = ECosmoMovementMode.Jump;

        //m_velocity = h_velocity + newVelocity;
    }


    public void ResetLaunchVelocity()
    {
        movementMode = ECosmoMovementMode.Nothing;
        m_launchVelocity = Vector3.zero;
        m_launchTime = 0.0f;
        m_launchTargetPosition = Vector3.zero;

        if (onLandAfterLaunch != null)
            onLandAfterLaunch.Invoke();
    }


    public GameObject CheckCurrentlyStandingGround()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = transform.Down();
        RaycastHit2D hit2D = Physics2D.Raycast(origin, direction, 16.0f, LayerMask.GetMask("World Geometry"));
        if (hit2D.collider != null)
        {
            GameObject hitObject = hit2D.collider.gameObject;
            return hitObject;
        }

        return null;
    }


    public Vector3 GetDeltaHeightInLaunch(float currentTime, Vector3 gravityAcceleration)
    {
        Vector3 deltaHeight = m_launchVelocity * currentTime - 0.5f * gravityAcceleration * (currentTime * currentTime);
        return deltaHeight;
    }


    public void StopMovementImmediately()
    {
        m_velocity = Vector3.zero;
        movementMode = ECosmoMovementMode.Nothing;
    }


    public float GetMaxJumpHeightTime()
    {
        return jumpPower / -gravityScale;
    }


    private void Fall()
    {
        if (!onGround)
            return;
        onGround = false;
    }


    public Vector3 GetFootLevel()
    {
        return transform.position - (transform.Up() * height);
    }


    public Vector2 GetFootLevel2D()
    {
        Vector3 footLevel3D = GetFootLevel();
        Vector2 result = new Vector2(footLevel3D.x, footLevel3D.y);
        return result;
    }


    public float GetHeight()
    {
        return height;
    }


    private void CollisionDetection_Vert(ref Vector3 currentPosition)
    {
        if (!enableCollisionDetection || onGround)
            return;

        Vector3 origin = transform.position;
        Vector3 direction = transform.Down();
        float maxTraceDistance = 1500.0f;
        RaycastHit2D hitInfo = Physics2D.Raycast(origin, direction, maxTraceDistance, LayerMask.GetMask("WorldStatic", "WorldDynamic", "Platforms"));
        if (hitInfo.transform != null)
        {
            Vector2 contact = new Vector2(hitInfo.point.x, hitInfo.point.y);
            GameObject hitObject = hitInfo.transform.gameObject;
            if (GetFootLevel2D().y < contact.y)
            {
                // If foot level y is lower than the ground's y, we probably are below the ground
                // Let's check if this object is a ray object
                IRayCollidable rc = hitObject.GetComponent(typeof(IRayCollidable)) as IRayCollidable;
                if (rc != null)
                {
                    rc.OnEnterRayCollision(gameObject);
                }

                m_velocity.x = 0.0f;
                m_velocity.y = 0.0f;
                onGround = true;

                // Reset launch velocity
                ResetLaunchVelocity();
                movementMode = ECosmoMovementMode.Nothing;

                // Oh, I hit a world geometry, I need to push myself up or some shiat
                m_groundAngle = Mathf.Atan2(hitInfo.normal.x, hitInfo.normal.y);
                currentPosition = hitInfo.point + (hitInfo.normal * height);
            }
        }
    }


    private void TrackGround(ref Vector3 currentPosition)
    {
        if (!enableCollisionDetection)
            return;

        Vector3 origin = transform.position;
        Vector3 direction = transform.Down();
        float maxTraceDistance = 16.0f;
        RaycastHit2D hitInfo = Physics2D.Raycast(origin, direction, maxTraceDistance, LayerMask.GetMask("WorldStatic", "Platforms"));
        if (hitInfo.transform != null)
        {
            Vector2 contact = hitInfo.point;
            GameObject hitObject = hitInfo.collider.gameObject;
            Vector3 footToGround = contact - GetFootLevel2D();
            float distance = footToGround.magnitude;
            if (distance <= 0.23f)
            {
                // Don't continue traveling downwards anymore, I'm on the ground!
                m_velocity.x = 0.0f;
                m_velocity.y = 0.0f;

                // Oh, I hit a world geometry, I need to push myself up or do some angle or some shiat!
                m_groundAngle = Mathf.Atan2(hitInfo.normal.x, hitInfo.normal.y);
                currentPosition = hitInfo.point + (hitInfo.normal * height);
            }
            else
            {
                IRayCollidable rayCollidable = hitObject.GetComponent(typeof(IRayCollidable)) as IRayCollidable;
                if (rayCollidable != null)
                {
                    rayCollidable.OnExitRayCollision(gameObject);
                }

                Fall();
            }
        }
    }
}
