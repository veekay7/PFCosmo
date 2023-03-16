// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Authors: VinTK
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Extensions;

public class PlayerCharaCosmoOld : Character
{
    [SerializeField, ReadOnly]
    private Platform m_pPlatform = null;
    [SerializeField]
    private PlatformInfoFukidashi m_fukidashi = null;
    [SerializeField]
    private float m_travelLockTime = 0.5f;

    [Header("Physics - Movement")]
    [SerializeField]
    protected float m_runMaxSpeed = 6.0f;
    [SerializeField, ReadOnly]
    protected float m_jumpPower = 1.0f;
    [SerializeField]
    protected float m_baseJumpHeight = 3.0f;    // NOTE: Jump Height is calculated in Unity units (1 unit)

    [Header("Physics - Movement Capabilities")]
    [SerializeField]
    protected bool m_canWalk = true;
    [SerializeField]
    protected bool m_canJump = true;

    [Header("SoundFX")]
    [SerializeField]
    private List<AudioClip> m_seClips = new List<AudioClip>();

    public UnityEvent onFinishTravel = new UnityEvent();
    public UnityEvent onFinalPowerStoneCollect = new UnityEvent();

    private Player m_player;
    private bool m_travelMode;
    private ETravelState m_travelState;
    private TravelData m_travelData;
    private Vector2 m_platformPosition;

    public SpriteRenderer spriteRenderer { get; private set; }
    public AudioSource audioSourceSE { get; private set; }
    public CosmoAnimBehaviourOld animBehaviour { get; private set; }
    public PlatformInfoFukidashi fukidashi => m_fukidashi;
    public bool isJumping { get; private set; }
    public bool isLaunched { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSourceSE = GetComponent<AudioSource>();
        animBehaviour = GetComponent<CosmoAnimBehaviourOld>();
        animBehaviour.owner = this;
    }


    protected override void Start()
    {
        base.Start();

        isAlive = true;
        isJumping = false;
        isLaunched = false;

        m_pPlatform = null;
        m_travelMode = false;
        m_travelState = ETravelState.Ready;
    }


    private void OnValidate()
    {
        // Recalculate jump power when anything is changed
        CalculateJumpPower(m_baseJumpHeight);
    }


    protected override void Update()
    {
        if (!isAlive)
            return;

        // If the chara is in travel mode, we need to do some checks
        if (m_travelMode)
        {
            // Well, I'm ready to travel, I guess I'll start
            if (m_travelState == ETravelState.Ready)
            {
                WaypointNode current = m_travelData.current;
                if (current != null)
                {
                    if (current.hint == EWaypointTravelHint.Jump)
                    {
                        Launch(current.transform);
                        m_travelState = ETravelState.Travelling;
                    }
                }
            }

            return;
        }

        // Before we start integrating the velocity, check if the chara is standing on a platform
        if (m_pPlatform != null && onGround && (!isJumping || !isLaunched))
        {
            if (m_platformPosition != m_pPlatform.position)
            {
                Vector2 platformDiff = m_pPlatform.position - m_platformPosition;
                Vector2 newPos = position;
                newPos.x += platformDiff.x;
                newPos.y += platformDiff.y;
                position = newPos;
            }

            m_platformPosition = m_pPlatform.position;
        }

        base.Update();
    }


    protected override void OnVertCollision(RaycastHit2D hit)
    {
        base.OnVertCollision(hit);

        spriteRenderer.flipX = false;

        // If the chara was jumping before, stop jumping.
        if (isJumping)
            isJumping = false;

        // If the chara was in launch mode before, set velocity x and y to zero
        if (isLaunched)
        {
            m_velocity.x = 0.0f;
            m_velocity.y = 0.0f;
            isLaunched = false;
        }

        CheckForPlatform(hit);

        if (m_travelMode)
        {
            // If I was traveling before, I need to do some checks since I probably hit something
            if (m_travelState == ETravelState.Travelling)
            {
                m_travelState = ETravelState.Finished;
                StartCoroutine(Co_WaitThenChangeLocation());
            }
        }
    }


    protected override void OnVertCollisionExit(RaycastHit2D hit)
    {
        base.OnVertCollisionExit(hit);

        GameObject hitObject = hit.transform.gameObject;
        Platform p = hitObject.GetComponent<Platform>();

        // If the last thing that was collided was a Platform, we can remove it.
        if (p != null)
        {
            m_pPlatform = null;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collectible collectible = collision.gameObject.GetComponent<Collectible>();
        if (collectible != null)
        {
            PowerStoneEffect powerStone = collectible.GetEffect() as PowerStoneEffect;
            if (powerStone != null && powerStone.IsLastPiece())
            {
                if (onFinalPowerStoneCollect != null)
                    onFinalPowerStoneCollect.Invoke();
            }
        }
    }


    public void CheckCurrentStandingPlatform()
    {
        // Do a raycast check
        RaycastHit2D slotHit = Physics2D.Raycast(transform.position, transform.Down2D(), 8.0f, LayerMask.GetMask("Slots"));
        if (slotHit.transform != null)
        {
            // Get dem slot
            GameObject platformObject = slotHit.transform.gameObject;
            PlatformSlot slot = platformObject.GetComponent<PlatformSlot>();
            m_pPlatform = slot.GetPlatform();
        }
        else
        {
            m_pPlatform = null;
        }
    }


    public void Launch(Transform goal)
    {
        // I cannot launch myself into the air to my goal if I'm not on the ground or if I'm launching
        if (!onGround || isLaunched)
            return;

        //float hypoDist = Vector3.Distance(m_launchTargetPosition, footLevel);
        //float heightOpps = goal.position.y - GetFootLevel2D().y;
        //float horzAdj = goal.position.x - GetFootLevel2D().x;
        //float g = gravityAcc * -1.0f;   // Have to inverse, because the gravityAcc is negative. This eqn only supports positive g.
        //m_velocity.x = (horzAdj * g / (m_jumpPower + Mathf.Sqrt((m_jumpPower * m_jumpPower) - heightOpps)));
        //m_velocity.y = m_jumpPower;

        float horzDist = goal.position.x - GetFootLevel2D().x;
        float height = goal.position.y - GetFootLevel2D().y;

        // Check the height if it is more than the base jump height.
        // If it is more than the base jump height, add the difference to the base jump height
        // If lower than base jump height, just use our jump height
        float jumpHeight = 0.0f;
        if (height > m_baseJumpHeight)
            jumpHeight = m_baseJumpHeight + height;
        else
            jumpHeight = m_baseJumpHeight;

        // Calculate the time required when I'm coming downwards towards my goal
        float y_time_down = Mathf.Sqrt(2.0f * (height - jumpHeight) / gravityAcc);

        // Calculate the time required when launching myself upwards
        float y_time_up = Mathf.Sqrt((-2.0f * jumpHeight) / gravityAcc);

        float xsp = horzDist / (y_time_up + y_time_down);
        float ysp = Mathf.Sqrt(-2.0f * gravityAcc * jumpHeight);

        m_velocity.x = xsp;
        m_velocity.y = ysp;

        if (m_velocity.x < 0.0f)
            spriteRenderer.flipX = true;

        audioSourceSE.clip = m_seClips[0];
        audioSourceSE.Play();

        isLaunched = true;

        if (m_pPlatform != null)
        {
            m_fukidashi.Clear();
            m_fukidashi.Hide();
            m_pPlatform.forceHideFukidashi = false;
        }
    }


    public void Jump()
    {
        // I can't jump if I'm on the ground or if I'm already jumping
        // (NO, DON'T EVEN FUCKEN' THINK OF DOUBLE JUMPING, THIS AIN'T NO ROCKMAN GAME!!)
        if (!onGround || isJumping)
            return;

        isJumping = true;
        m_velocity.y = m_jumpPower;
    }


    public void Travel(BaseLinkedListLevel levelScript)
    {
        if (levelScript == null)
            return;

        // If I'm already traveling, don't tell me to keep doing it.
        // ええええ、私は今超怠いは、面倒くさい、後でね。
        if (m_travelMode)
            return;

        // Prepare the travel data
        m_travelData = levelScript.GetTravelData();
        m_travelData.current = m_travelData.start;

        m_travelMode = true;
        m_travelState = ETravelState.Ready;
    }


    public Platform GetPlatform()
    {
        return m_pPlatform;
    }


    public void StopTravel()
    {
        if (!m_travelMode)
            return;
        m_travelMode = false;
        m_travelState = ETravelState.Ready;
    }


    public Player GetControllingPlayer()
    {
        return m_player;
    }


    public void SetControllingPlayer(Player newPlayer)
    {
        m_player = newPlayer;
    }


    private void CalculateJumpPower(float jumpHeight)
    {
        m_jumpPower = Mathf.Sqrt(-2.0f * gravityAcc * jumpHeight);
    }


    private void CheckForPlatform(RaycastHit2D hit)
    {
        GameObject hitObject = hit.transform.gameObject;
        Platform p = hitObject.GetComponent<Platform>();
        if (p == null)
        {
            m_fukidashi.Clear();
            m_fukidashi.Hide();

            m_platformPosition = Vector2.zero;
            m_pPlatform = null;
            return;
        }

        m_pPlatform = p;
        m_platformPosition = m_pPlatform.position;

        m_pPlatform.forceHideFukidashi = true;
        m_fukidashi.SetKana(m_pPlatform.kana.sprite);
        m_fukidashi.SetValue(m_pPlatform.value);
        m_fukidashi.Show();
    }


    private IEnumerator Co_CountdownLockTimer(float lockTime, Action onComplete)
    {
        yield return new WaitForSeconds(lockTime);

        if (onComplete != null)
            onComplete();
    }


    private IEnumerator Co_WaitThenChangeLocation()
    {
        yield return new WaitForSeconds(m_travelLockTime);
        if (m_travelData.current != null)
        {
            if (m_travelData.current != m_travelData.end)
            {
                // If I have not reached the end, try to go to the next one
                WaypointNode next = m_travelData.current.next;
                if (next != null)
                    m_travelData.current = next;
                else
                    m_travelData.current = m_travelData.end;

                m_travelState = ETravelState.Ready;
            }
            else
            {
                // I have reached my goal, I should stop traveling.
                m_travelData.current = null;
                StopTravel();
            }
        }
    }
}

