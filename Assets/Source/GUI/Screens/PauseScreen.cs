// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine.Events;

public class PauseScreen : BaseScreen
{
    public UnityEvent onResumeSelected = new UnityEvent();
    public UnityEvent onRestartSelected = new UnityEvent();
    public UnityEvent onQuitSelected = new UnityEvent();


    protected override void Awake()
    {
        base.Awake();
        Hide();
    }


    public void ResumeGame()
    {
        if (onResumeSelected != null)
            onResumeSelected.Invoke();
    }


    public void RestartGame()
    {
        if (onRestartSelected != null)
            onRestartSelected.Invoke();
    }


    public void QuitStage()
    {
        if (onQuitSelected != null)
            onQuitSelected.Invoke();
    }
}
