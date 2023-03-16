// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MissionScreen : BaseScreen
{
    [SerializeField]
    private TextMeshProUGUI m_instructionsText = null;

    public bool isTransitioning { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        isTransitioning = false;
    }


    protected void Update()
    {
        if (GameController.instance.gameRules.hasGameStarted)
        {
            if (Input.anyKeyDown)
                Hide();
        }
    }


    public void SetInfo(string instructionsMsg)
    {
        m_instructionsText.text = instructionsMsg;
    }


    public void Clear()
    {
        m_instructionsText.text = "Instructions";
    }


    public IEnumerator Co_Fade(float start, float final, float time, Action onFinish)
    {
        isTransitioning = true;

        float a = 0.0f;
        while (a < time)
        {
            float perc = a / time;
            canvasGroup.alpha = Mathf.Lerp(start, final, perc);
            yield return null;
        }

        isTransitioning = false;
    }
}
