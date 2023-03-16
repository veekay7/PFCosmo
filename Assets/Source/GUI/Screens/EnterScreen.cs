// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using System.Collections;
using UnityEngine;

public class EnterScreen : BaseScreen
{
    [SerializeField]
    private UIPressAnyKey m_enterGameText = null;
    [SerializeField]
    private UIWidget m_loginMenu = null;
    [SerializeField]
    private ProfilesWindow m_profilesWindow = null;
    [SerializeField]
    private NewProfileWindow m_newProfileWindow = null;
    [SerializeField]
    private UIMainMenuSelection m_mainMenuSelection = null;

    public bool isTransitioning { get; private set; }
    public UIPressAnyKey enterGameText => m_enterGameText;
    public UIWidget loginMenu => m_loginMenu;
    public ProfilesWindow profilesWindow => m_profilesWindow;
    public NewProfileWindow newProfileWindow => m_newProfileWindow;
    public UIMainMenuSelection mainMenuSelection => m_mainMenuSelection;


    protected override void Awake()
    {
        base.Awake();
        isTransitioning = false;
    }


    private void DisableAll()
    {
        enterGameText.Hide();
        loginMenu.Hide();
        profilesWindow.Hide();
        newProfileWindow.Hide();

        mainMenuSelection.Hide();
        mainMenuSelection.enabled = false;
    }


    public void DisplayEnterGameText(bool value)
    {
        if (value)
            m_enterGameText.Show();
        else
            m_enterGameText.Hide();
    }


    public void DisplayLoginMenu(bool value)
    {
        if (value)
            m_loginMenu.Show();
        else
            m_loginMenu.Hide();
    }


    public IEnumerator Co_FadeWidget(CanvasGroup widget, bool fadeout, float maxTime, Action onComplete = null)
    {
        Debug.Assert(widget != null);

        float t = 0.0f;
        float perc = 0.0f;
        float a = fadeout ? 1.0f : 0.0f;

        while (t < maxTime)
        {
            perc = a / maxTime;
            widget.alpha = perc;
            t += Time.deltaTime;

            if (fadeout)
                a -= Time.deltaTime;
            else
                a += Time.deltaTime;

            yield return null;
        }

        if (onComplete != null)
            onComplete.Invoke();
    }
}
