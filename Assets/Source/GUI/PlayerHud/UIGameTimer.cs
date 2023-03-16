// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;
using UnityEngine.UI;

public class UIGameTimer : UIWidget
{
    [SerializeField]
    private Text m_timeText = null;
    [SerializeField]
    private GameTimer m_timer = null;


    protected override void Awake()
    {
        base.Awake();
    }


    private void LateUpdate()
    {
        if (m_timer == null)
            return;

        string strMMSS = Utils.ConvertTimeToMMSS(m_timer.GetTime());
        m_timeText.text = strMMSS;
    }
}
