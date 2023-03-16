// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;
using UnityEngine.Extensions;
using UnityEngine.Events;
using System;

public class Character : MonoBehaviour
{
    [Header("Base")]
    [Header("Physics - Movement")]
    [SerializeField]
    protected float m_gravityConst = 10.0f;
    [SerializeField]
    protected float m_gravityScale = 1.8f;
    [SerializeField]
    protected float m_terminalSpeed = 16.0f;

    [Header("Physics - Collision Detection")]
    [SerializeField]
    private float m_maxVertTraceDistance = 15.0f;
    [SerializeField]
    protected float m_height = 1.5f;
    [SerializeField]
    protected LayerMask m_collisionLayers;

    [Header("Flags")]
    public bool m_enableGravity = true;
    public bool m_enableCollisionDetection = true;
    public bool m_debugMode = false;

    public UnityEvent onVertCollisionDetection = new UnityEvent();

    protected GameObject m_owner;
    protected float m_groundAngle;
    protected float m_gravity;
    protected Vector2 m_velocity;

    public Rigidbody2D rigidbodyComponent { get; private set; }
    public BoxCollider2D colliderComponent { get; private set; }
    public bool isAlive { get; protected set; } = true;
    public bool onGround { get; protected set; } = true;
    public float height => m_height;
    public float gravityAcc => -(m_gravityConst * m_gravityScale);
    public Vector2 velocity => m_velocity;
    public Vector2 position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public float rotation
    {
        get { return rigidbodyComponent.rotation; }
        set { rigidbodyComponent.MoveRotation(rotation); }
    }

    protected virtual void Awake()
    {
        rigidbodyComponent = GetComponent<Rigidbody2D>();
        colliderComponent = GetComponent<BoxCollider2D>();

        rigidbodyComponent.isKinematic = true;
        rigidbodyComponent.gravityScale = 0.0f;
    }


    protected virtual void Start()
    {
        isAlive = true;
        onGround = false;

        m_groundAngle = 0.0f;
        m_gravity = 0.0f;
        m_velocity = Vector2.zero;
    }


    protected virtual void Update()
    {
        if (!isAlive)
            return;

        //TrackGround();
        if (Mathf.Abs(m_velocity.y) > 0.0f)
            onGround = false;

        if (onGround)
        {
            // If chara is on the ground, no need to do anything
            m_gravity = 0.0f;
        }
        else
        {
            // Resolve gravity acceleration
            if (m_enableGravity)
            {
                if (m_velocity.y > -m_terminalSpeed)
                    m_gravity = !onGround ? gravityAcc : 0.0f;
                else
                    m_gravity = m_terminalSpeed;

                // Integrate gravity acceleration to velocity
                m_velocity.y += m_gravity * Time.deltaTime;
            }
        }

        CollisionDetection_Vert();

        // If the magnitude of the velocity is too small, just set it to zero.
        if (m_velocity.magnitude < Globals.KINDA_SMALL_NUMBER)
            m_velocity = Vector2.zero;

        position += m_velocity * Time.deltaTime;
    }


    private Nullable<RaycastHit2D> m_oldHit = null;
    private void CollisionDetection_Vert()
    {
        if (!m_enableCollisionDetection)
            return;

        // NOTE: This must have to prevent it from colliding with itself
        Physics2D.queriesStartInColliders = false;

        RaycastHit2D hit;
        if (m_velocity.y != 0.0f && !onGround)
        {
            float dirY = Mathf.Sign(m_velocity.y);
            //hit = Physics2D.Raycast(position, transform.Up2D() * dirY, Mathf.Abs(m_velocity.y), m_collisionLayers);
            hit = Physics2D.Raycast(position, transform.Up2D() * dirY, m_maxVertTraceDistance, m_collisionLayers);
            if (hit.transform != null)
            {
                GameObject hitObject = hit.transform.gameObject;
                if (GetFootLevel2D().y <= hit.point.y)
                {
                    // Once contact has been reached, remove velocity y
                    m_velocity.y = 0.0f;

                    Vector2 newPos = position;
                    newPos.y = hit.point.y + height;
                    position = newPos;

                    m_groundAngle = Mathf.Atan2(hit.normal.x, hit.normal.y);
                    onGround = true;

                    OnVertCollision(hit);
                    m_oldHit = hit;
                }
            }
            else
            {
                if (m_oldHit.HasValue)
                    OnVertCollisionExit(m_oldHit.Value);
                m_oldHit = null;
                onGround = false;
            }
        }
        else
        {
            hit = Physics2D.Raycast(position, Vector2.down, m_maxVertTraceDistance, m_collisionLayers);
            //hit = Physics2D.Raycast(position, transform.Down2D(), height, m_collisionLayers);
            if (hit.transform != null)
            {
                GameObject hitObject = hit.collider.gameObject;

                Vector2 newPos = position;
                newPos.y = hit.point.y + height;
                position = newPos;

                m_groundAngle = Mathf.Atan2(hit.normal.x, hit.normal.y);
                m_oldHit = hit;
            }
            else
            {
                if (m_oldHit.HasValue)
                    OnVertCollisionExit(m_oldHit.Value);
                m_oldHit = null;
                onGround = false;
            }
        }

        // NOTE: Always reset back to default afterwards
        Physics2D.queriesStartInColliders = true;
    }


    private void TrackGround()
    {
        // If the chara is not on the ground, there is no reason to track it.
        if (!onGround)
            return;

        RaycastHit2D hit = Physics2D.Raycast(position, transform.Down2D(), height, m_collisionLayers);
        if (hit.transform != null)
        {
            GameObject hitObject = hit.collider.gameObject;
            m_groundAngle = Mathf.Atan2(hit.normal.x, hit.normal.y);
            position = hit.point + (hit.normal * height);
        }
        else
        {
            onGround = false;
        }
    }


    protected virtual void OnVertCollision(RaycastHit2D hit) { }
    protected virtual void OnVertCollisionExit(RaycastHit2D hit) { }


    protected virtual void OnDrawGizmos()
    {
        Color oldColor = Gizmos.color;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, GetFootLevel2D());

        Gizmos.color = Color.blue;
        Vector3 normalized = m_velocity.normalized;
        Gizmos.DrawLine(transform.position, transform.position + (normalized * m_maxVertTraceDistance));
        Gizmos.color = oldColor;
    }


    public Vector2 GetFootLevel2D()
    {
        Vector2 pos2D = new Vector2(transform.position.x, transform.position.y);
        Vector2 result = pos2D + (transform.Down2D() * m_height);
        return result;
    }


    public virtual void Kill()
    {
        isAlive = false;
    }
}

public class CharaWorldCollisionEvent : UnityEvent<GameObject> { }