// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseLinkedListLevel : LevelScript
{
    [Header("Base")]
    public StageData stageData;
    public KanaTable kanaTable;
    public LLValueManager valueManager;
    public GameState gameState;
    public AudioSource audioSrcBgm;
    public AudioSource audioSrcVictory;
    [HideInInspector]
    public List<int> usableValues = new List<int>();

    [ReadOnly]
    public bool isModified;
    [ReadOnly]
    public bool isSolved;

    protected TravelData m_travelData;
    public GameCamera gameCamera { get; protected set; }
    public Player player { get; protected set; }
    public Puzzle mainPuzzle { get; set; }
    public WaypointNode start { get; set; }
    public WaypointNode goal { get; set; }


    protected override void Awake()
    {
        base.Awake();
        valueManager.Init(usableValues.ToArray());
    }


    private void PlayBgm()
    {
        audioSrcBgm.Play();
    }


    protected override void Start()
    {
        base.Start();

        // Find the camera
        GameObject cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        if (cameraObject == null)
            Debug.LogWarning("The game camera does not exist!");
        else
            gameCamera = cameraObject.GetComponent<GameCamera>();

        // Find the player
        player = GameObject.FindObjectOfType<Player>();
        if (player == null)
            Debug.LogWarning("The Player does not exist!");

        GameRules.instance.onGameStart.AddListener(PlayBgm);

        isModified = false;
        isSolved = false;
    }


    protected virtual void Update()
    {
        gameState.headPlatform = mainPuzzle.head;

        if (isModified)
        {
            CheckSolution();
            isModified = false;
        }
    }


    public TravelData GetTravelData()
    {
        m_travelData.level = this;
        m_travelData.head = mainPuzzle.head;
        m_travelData.start = start;
        m_travelData.end = goal;
        return m_travelData;
    }


    public virtual void AddFirst(Action<Platform> onComplete) { }
    public virtual void AddLast(Action<Platform> onComplete) { }
    public virtual void AddAt(int index, Action<Platform> onComplete) { }
    public virtual void RemoveFirst(Action<Platform> onComplete) { }
    public virtual void RemoveLast(Action<Platform> onComplete) { }
    public virtual void RemoveAt(int index, Action<Platform> onComplete) { }
    public virtual void CheckSolution() { }


    protected virtual void OnDestroy()
    {
        valueManager.Clear();
        gameState.Clear();
    }

    protected void PlayVictoryChime()
    {
        audioSrcVictory.Play();
    }
}
