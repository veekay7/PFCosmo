// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameRules : MonoBehaviourSingleton<GameRules>
{
    [SerializeField]
    private GameScreenCanvas m_gameScreenCanvasPrefab = null;
    [SerializeField]
    private GameStats m_gameStatsPrefab = null;
    [SerializeField]
    private GameTimer m_gameTimer = null;
    [SerializeField]
    private GameState m_gameState = null;
    [SerializeField]
    private RestartCounter m_restartCounter = null;
    [SerializeField]
    private int m_maxRestartCount = 0;
    private GameStats gameStatsInst;
    private GameScreenCanvas gameScreenCanvasInst;

    public UnityEvent onGameEnter = new UnityEvent();
    public UnityEvent onGameStart = new UnityEvent();
    public UnityEvent onGamePause = new UnityEvent();
    public UnityEvent onGameResume = new UnityEvent();
    public UnityEvent onGameOver = new UnityEvent();

    public bool isPausable { get; set; }
    public bool hasGameStarted { get; private set; }
    public bool isPaused { get; private set; }
    public bool isGameOver { get; private set; }

    // Game outcome
    // 0 - Nothing
    // -1 - You Failed!
    // 1 - You Win!
    public int gameOutcome { get; private set; }


    protected override void OnEnable()
    {
        base.OnEnable();

        // Instantiate UI shit
        if (GameScreenCanvas.instance == null)
        {
            gameScreenCanvasInst = Instantiate(m_gameScreenCanvasPrefab);
            if (gameScreenCanvasInst == null)
                Debug.LogWarning("Game Screen Canvas cannot be created!");
        }

        if (GameStats.instance == null)
        {
            gameStatsInst = Instantiate(m_gameStatsPrefab);
            if (gameStatsInst == null)
                Debug.LogWarning("Game Stats cannot be created!");
        }

        gameScreenCanvasInst.pauseScreen.onResumeSelected.AddListener(ResumeGame);
        gameScreenCanvasInst.pauseScreen.onRestartSelected.AddListener(RestartStage);
        gameScreenCanvasInst.pauseScreen.onQuitSelected.AddListener(() =>
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("StageSelect", LoadSceneMode.Single);
        });
    }


    private void Start()
    {
        isPausable = true;
        hasGameStarted = false;
        isPaused = false;
        isGameOver = false;

        gameOutcome = 0;
        Time.timeScale = 1.0f;

        // Show the mission instructions then give a timer before allowing input
        BaseLinkedListLevel level = GameController.instance.levelScript as BaseLinkedListLevel;
        gameScreenCanvasInst.missionScreen.SetInfo(level.stageData.instructions);
        gameScreenCanvasInst.missionScreen.Show();

        if (onGameEnter != null)
            onGameEnter.Invoke();
    }


    private void Update()
    {
        m_gameTimer.Update();

        if (!hasGameStarted)
        {
            if (Input.anyKeyDown)
            {
                hasGameStarted = true;
                m_gameTimer.Start();
                gameScreenCanvasInst.missionScreen.Hide();

                // When the game starts, start new analytics
                if (GameApp.CurrentProfile != null)
                {
                    BaseLinkedListLevel level = (BaseLinkedListLevel)GameController.instance.levelScript;
                    GameStats.instance.StartNew(level.stageData.sceneName);
                    GameStats.instance.SetString("stageName", level.stageData.sceneName);
                }

                if (onGameStart != null)
                    onGameStart.Invoke();
            }
        }
        else
        {
            if (isGameOver)
            {
                if (Input.anyKeyDown)
                {
                    if (GameStats.instance != null && GameApp.CurrentProfile != null)
                    {
                        GameStats.instance.Save(GameApp.CurrentProfile.folderPath);
                        GameStats.instance.Flush();
                        PlayerProfile.SaveToDisk(GameApp.CurrentProfile.folderPath + "profile.json", GameApp.CurrentProfile);
                        m_restartCounter.Reset();
                        Destroy(GameStats.instance.gameObject);
                    }

                    SceneManager.LoadScene("StageSelect", LoadSceneMode.Single);
                }
            }

            if (isPausable && !isGameOver)
            {
                if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
                    PauseGame();
                else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
                    ResumeGame();
            }
        }
    }


    public void PauseGame()
    {
        if (hasGameStarted && isPaused)
            return;

        isPaused = true;

        // Pause the game timer
        m_gameTimer.Pause();

        // Set the global engine timescale to 0 to stop all movement based on time
        Time.timeScale = 0.0f;

        if (onGamePause != null)
            onGamePause.Invoke();

        GameScreenCanvas.instance.pauseScreen.Show();
    }


    public void ResumeGame()
    {
        if (hasGameStarted && !isPaused)
            return;

        isPaused = false;

        // UnPause the game timer
        m_gameTimer.UnPause();

        // Set the global engine timescale back to full speed
        Time.timeScale = 1.0f;

        GameScreenCanvas.instance.pauseScreen.Hide();

        if (onGameResume != null)
            onGameResume.Invoke();
    }


    public void RestartStage()
    {
        Time.timeScale = 1.0f;
        m_restartCounter.Increment();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
    }


    public void GameOver()
    {
        isGameOver = true;

        // Pause the timer
        m_gameTimer.Pause();

        // Gather statistics and shit
        bool allCrysCollected = (m_gameState.collectedCrystals == m_gameState.totalCrystals);
        float totalPlayTime = m_gameTimer.GetTime();

        if (GameApp.CurrentProfile != null)
        {
            GameStats.instance.SetFloat("totalPlayTime", totalPlayTime);
            GameStats.instance.SetInt("restartCount", m_restartCounter.GetCount());
            GameStats.instance.SetInt("maxRestartCount", m_maxRestartCount);
            GameStats.instance.SetInt("stageCollectedCrystals", m_gameState.collectedCrystals);
            GameStats.instance.SetInt("stageTotalCrystals", m_gameState.totalCrystals);
        }

        m_gameTimer.Stop();

        if (onGameOver != null)
            onGameOver.Invoke();

        GameScreenCanvas.instance.resultScreen.SetResultStats(
            m_gameState.collectedCrystals,
            m_gameState.totalCrystals,
            allCrysCollected,
            totalPlayTime,
            m_restartCounter.GetCount(),
            m_maxRestartCount,
            allCrysCollected);  // TODO: Change whether or not level is completed depending on rules instead of just if all crystals are collected.
        GameScreenCanvas.instance.resultScreen.Show();
    }


    public bool IsMaxFront(int[] list)
    {
        int max = Mathf.Max(list);
        int maxIdx = Array.FindIndex(list, (x) => x == max);
        if (maxIdx == 0)
            return true;

        return false;
    }


    public bool IsMaxBack(int[] list)
    {
        int max = Mathf.Max(list);
        int maxIdx = Array.FindIndex(list, (x) => x == max);
        if (maxIdx == list.Length - 1)
            return true;

        return false;
    }


    public bool IsMinFront(int[] list)
    {
        int min = Mathf.Min(list);
        int minIdx = Array.FindIndex(list, (x) => x == min);
        if (minIdx == 0)
            return true;

        return false;
    }


    public bool IsMinBack(int[] list)
    {
        int min = Mathf.Min(list);
        int minIdx = Array.FindIndex(list, (x) => x == min);
        if (minIdx == list.Length - 1)
            return true;

        return false;
    }


    public bool IsAscending(int[] list)
    {
        for (int i = 0; i < list.Length- 1; i++)
        {
            if (list[i] > list[i + 1])
                return false;
        }

        return true;
    }


    public bool IsDescending(int[] list)
    {
        for (int i = 0; i < list.Length - 1; i++)
        {
            if (list[i] < list[i + 1])
                return false;
        }

        return true;
    }


    public bool AreEvenItemsBack(int[] list)
    {
        // First collect all the even numbers
        List<int> evens = new List<int>();
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i] % 2 == 0)
                evens.Add(i);
        }

        if (evens.Count == 0)
            return false;

        // Now reverse iterate the list array and match with evens
        for (int i = list.Length - 1; i >= 0; i--)
        {
            if (evens.Count == 0)
                break;
            int current = list[i];
            int idxOf = evens.IndexOf(current);
            if (idxOf == -1)
                return false;
            evens.RemoveAt(idxOf);
        }

        return true;
    }


    public bool AreOddItemsBack(int[] list)
    {
        // First collect all the odd numbers
        List<int> odds = new List<int>();
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i] % 2 != 0)
                odds.Add(i);
            odds.Add(i);
        }

        if (odds.Count == 0)
            return false;

        // Now reverse iterate the list array and match with odds
        for (int i = list.Length - 1; i >= 0; i--)
        {
            if (odds.Count == 0)
                break;
            int current = list[i];
            int idxOf = odds.IndexOf(current);
            if (idxOf == -1)
                return false;
            odds.RemoveAt(idxOf);
        }

        return true;
    }


    public void SetGameOutcome(int outcome)
    {
        if (outcome < -1 || outcome > 1)
            throw new UnityException("Invalid outcome code entered.");

        gameOutcome = outcome;
        if (m_gameState.collectedCrystals == m_gameState.totalCrystals)
            gameOutcome = 1;
        else
            gameOutcome = -1;
    }


    private void OnDestroy()
    {
        m_gameTimer.Stop();
        onGameEnter.RemoveAllListeners();
        onGamePause.RemoveAllListeners();
        onGameResume.RemoveAllListeners();
        onGameOver.RemoveAllListeners();
    }
}
