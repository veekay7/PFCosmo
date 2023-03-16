// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerHudCanvas playerHudCanvasPrefab = null;
    public LLValueManager valueManager = null;
    public PlayerValueInventory inventory = null;
    public GameState gameState = null;
    public ItemCollection stoneCollection = null;
    public PlayerCharaCosmo m_chara = null;      // The Character the player is controlling
    [SerializeField]
    private float m_camMoveRate = 25.0f;

    private PlayerProfile m_profile;
    private SpriteRenderer m_spawnMarkerSpriteRenderer;
    private CapsuleCollider2D m_capsuleCollider;
    private PlayerActionManager m_actionManager;
    private GameCamera m_camera;
    [HideInInspector]
    public PlayerHudCanvas playerHudInst = null;
    private Vector3 m_currentMousePos;


    private void Awake()
    {
        m_capsuleCollider = GetComponent<CapsuleCollider2D>();

        m_spawnMarkerSpriteRenderer = GetComponent<SpriteRenderer>();
        m_spawnMarkerSpriteRenderer.enabled = true;

        m_actionManager = GetComponent<PlayerActionManager>();
        m_actionManager.SetOwner(this);

        m_profile = GameApp.CurrentProfile;

        // Find the camera
        GameObject cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        if (cameraObject == null)
            Debug.LogWarning("The game camera does not exist!");
        else
            m_camera = cameraObject.GetComponent<GameCamera>();

        // Create UI shit
        playerHudInst = PlayerHudCanvas.instance;
        if (playerHudInst == null)
        {
            playerHudInst = Instantiate(playerHudCanvasPrefab);
            if (playerHudInst == null)
                Debug.LogWarning("Cannot create Player Hud Canvas!");
            else
                playerHudInst.player = this;
        }
        else
        {
            playerHudInst.player = this;
        }
    }


    private void Start()
    {
        // Register with game rule game functions
        GameRules.instance.onGameEnter.AddListener(() =>
        {
            this.enabled = false;
            playerHudInst.Hide();
        });

        GameRules.instance.onGameStart.AddListener(() =>
        {
            this.enabled = true;
            playerHudInst.Show();
        });

        GameRules.instance.onGamePause.AddListener(() =>
        {
            this.enabled = false;
        });

        GameRules.instance.onGameResume.AddListener(() =>
        {
            this.enabled = true;
        });

        GameRules.instance.onGameOver.AddListener(() =>
        {
            this.enabled = false;
            m_actionManager.SetActionsUsageStats();
            StartCoroutine(m_chara.mdlCosmo.animBehaviour.Co_PlayVictoryAnim(null));
        });

        SpawnChara();
        m_currentMousePos = Vector3.zero;
        Cursor.lockState = CursorLockMode.Confined;
    }


    private void Update()
    {
        // NEVER UPDATE THE PLAYER IF THE THERE IS NO CHARACTER
        if (m_chara == null)
            return;

        GameRules.instance.isPausable = m_actionManager.isPendingInput ? false : true;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // もしいまのアクションはインプット待ってちゅう、左クリックなら、メッセージのオーバレイを閉じる。
            if (m_actionManager.isPendingInput)
            {
                print("Cancelled action");
                //m_action.StopPendingInputs();
            }
        }
        else if (Input.GetButtonDown("Camera Move Toggle"))
        {
            // そうしないと、別のインプットを処理する。
            m_camera.ToggleFreeMode();
        }

        HandleCameraMovementnput();
    }


    private void SpawnChara()
    {
        // During spawn, check to see if a Character has been defined
        if (m_chara == null)
        {
            Debug.LogWarning("The Player has no Character to control");
            return;
        }

        m_chara.SetControllingPlayer(this);
        m_chara.onFinalPowerStoneCollect.RemoveAllListeners();

        m_chara.transform.position = transform.position;
        m_chara.gameObject.SetActive(true);

        m_spawnMarkerSpriteRenderer.enabled = false;
    }


    public bool Trace(out GameObject tracedObject, params string[] layerNames)
    {
        if (m_camera == null)
            throw new NullReferenceException("gameCamera");

        Physics2D.queriesStartInColliders = true;
        Camera cam = m_camera.mainCamera;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1500.0f, LayerMask.GetMask(layerNames));
        if (hit.transform != null)
        {
            tracedObject = hit.transform.gameObject;
            return true;
        }

        Physics2D.queriesStartInColliders = false;
        tracedObject = null;
        return false;
    }


    public PlayerCharaCosmo GetChara()
    {
        return m_chara;
    }


    private void HandleCameraMovementnput()
    {
        m_currentMousePos = Input.mousePosition;

        if (m_camera.isFreeMode)
        {
            //Vector2 vpWorldMin = m_camera.mainCamera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, m_camera.mainCamera.nearClipPlane));
            //Vector2 vpWorldMax = m_camera.mainCamera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, m_camera.mainCamera.nearClipPlane));
            Vector3 currentMousePosVp = m_camera.mainCamera.ScreenToViewportPoint(m_currentMousePos);

            // Move camera in X-axis
            if (currentMousePosVp.x <= 0.0f)
                m_camera.MovePosition(-m_camMoveRate, 0.0f);
            else if (currentMousePosVp.x >= 1.0f)
                m_camera.MovePosition(m_camMoveRate, 0.0f);
            else
                m_camera.MovePosition(0.0f, 0.0f);

            // Move camera in Y-axis
            if (currentMousePosVp.y <= 0.0f)
                m_camera.MovePosition(0.0f, -m_camMoveRate);
            else if (currentMousePosVp.y >= 1.0f)
                m_camera.MovePosition(0.0f, m_camMoveRate);
            else
                m_camera.MovePosition(0.0f, 0.0f);
        }
    }


    private void OnDestroy()
    {
        stoneCollection.Clear();
        inventory.Clear();
    }
}
