// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;
using UnityEngine.UI;

public class ResultScreen : BaseScreen
{
    [SerializeField]
    private Text m_outcomeText = null;
    [SerializeField]
    private Text m_crysCollectedText = null;
    [SerializeField]
    private Text m_restartCountText = null;
    [SerializeField]
    private Text m_totalTimeText = null;


    public void SetResultStats(int collectedCrys, int totalCrys, bool allCrysCollected, float totalTime, int restartCount, int maxRestartCount, bool success)
    {
        m_crysCollectedText.text = collectedCrys.ToString() + "/" + totalCrys.ToString();
        m_totalTimeText.text = Utils.ConvertTimeToMMSS(totalTime);
        m_restartCountText.text = restartCount.ToString() + "/" + maxRestartCount.ToString();
        m_outcomeText.text = success ? "Success" : "Failed";
    }
}
