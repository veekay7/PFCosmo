// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;

public class MainMenu_TitleState : MonoBehaviour
{
    [SerializeField]
    private EnterScreen m_enterScreen = null;
    [SerializeField]
    private AudioSource m_enterButtonSE = null;
    [SerializeField]
    private AudioSource m_bgmPlayer = null;

    private readonly float m_fadeInTime = 3.0f;
    private StateMachineController m_stateController;


    public void Init(StateMachineController stateCtrl)
    {
        m_stateController = stateCtrl;
    }


    public void OnTitleStateBegin()
    {
        StartCoroutine(SharedCanvas.instance.screenFader.Co_FadeInScreen(m_fadeInTime, () =>
        {
            m_stateController.TickState = true;
            m_bgmPlayer.Play();
        }));

        m_enterScreen.Show();
        m_enterScreen.DisplayLoginMenu(false);
        m_enterScreen.DisplayEnterGameText(true);
    }


    public void OnTitleStateUpdate()
    {
        if (Input.anyKeyDown)
        {
            m_enterButtonSE.Play();
            m_stateController.TickState = false;
            StartCoroutine(m_enterScreen.Co_FadeWidget(m_enterScreen.enterGameText.canvasGroup, true, 1.0f, () =>
            {
                m_stateController.GoToState("LoginMenu");
            }));
        }
    }


    public void OnTitleStateEnd()
    {
        m_stateController.TickState = false;
    }
}
