// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectLevel : LevelScript
{
    [Header("General")]
    [SerializeField]
    private float m_fadeInTime = 1.0f;
    [SerializeField]
    private float m_fadeOutTime = 1.0f;
    [SerializeField]
    private RestartCounter m_restartCounter = null;
    [SerializeField]
    private AudioSource m_audioSourceBGM = null;
    [SerializeField]
    private LoadingScreen m_loadingScreen = null;
    [SerializeField]
    private ScreenFader m_screenFader = null;
    [SerializeField]
    private CanvasGroup m_inputBlocker = null;
    private bool m_enableInput;


    protected override void Awake()
    {
        base.Awake();
        DisableInput();
    }


    protected override void Start()
    {
        base.Start();

        StartCoroutine(m_screenFader.Co_FadeInScreen(m_fadeInTime, () =>
        {
            EnableInput();
            m_audioSourceBGM.Play();
        }));
    }


    private void Update()
    {
        if (m_enableInput && Input.GetKeyDown(KeyCode.Escape))
        {
            GoToMainMenu();
        }
    }


    /// <summary>
    /// Goes to the main menu
    /// </summary>
    public void GoToMainMenu()
    {
        DisableInput();
        StartCoroutine(m_screenFader.Co_FadeOutScreen(m_fadeOutTime, () =>
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }));
    }


    public void LoadStage(StageData stageData)
    {
        m_restartCounter.Reset();
        StartCoroutine(Co_LoadStage(stageData.sceneName));
    }


    /// <summary>
    /// Loads a stage
    /// </summary>
    /// <param name="stageName"></param>
    /// <returns></returns>
    private IEnumerator Co_LoadStage(string stageName)
    {
        if (string.IsNullOrEmpty(stageName))
        {
            Debug.LogWarning("Stage name is null or empty");
            yield break;
        }

        // Block all inputs first
        DisableInput();

        // Fade the screen out
        yield return m_screenFader.Co_FadeOutScreen(m_fadeOutTime, null);

        // Show the loading screen
        m_loadingScreen.Show();

        // Show the screen
        yield return m_screenFader.Co_FadeInScreen(m_fadeOutTime, null);

        AsyncOperation op = SceneManager.LoadSceneAsync(stageName);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            Debug.Log("Loading Progress: " + (progress * 100) + "%");

            // Loading completed
            if (op.progress >= 0.9f)
            {
                // Activate the scene sisturrr
                op.allowSceneActivation = true;
            }

            yield return null;
        }

        // Hide the loading screen
        m_loadingScreen.Hide();
    }


    private void EnableInput()
    {
        m_enableInput = true;
        m_inputBlocker.alpha = 0.0f;
        m_inputBlocker.interactable = false;
        m_inputBlocker.blocksRaycasts = false;
    }

    private void DisableInput()
    {
        m_enableInput = false;
        m_inputBlocker.alpha = 1.0f;
        m_inputBlocker.interactable = true;
        m_inputBlocker.blocksRaycasts = true;
    }
}
