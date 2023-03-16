// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenuSelection : UIWidget
{
    [SerializeField]
    private UIMainMenuSelectionItem m_selectionItem = null;
    [SerializeField]
    private AudioSource m_navBtnClickSE = null;
    [SerializeField]
    public List<UIMainMenuSelectionData> m_selectionDataSet = new List<UIMainMenuSelectionData>();
    private int m_currentSelectionIdx;

    public int maxSelectionCount => (m_selectionDataSet.Count - 1);


    public void ResetCurrentSelectionIndex()
    {
        m_currentSelectionIdx = 0;
        m_selectionItem.SetData(m_selectionDataSet[m_currentSelectionIdx]);
    }


    private void Start()
    {
        // Get the first enabled occurrence from the data set and set the index
        for (int i = 0; i < m_selectionDataSet.Count; i++)
        {
            if (m_selectionDataSet[i].enabled)
            {
                m_selectionItem.SetData(m_selectionDataSet[i]);
                break;
            }
        }
    }


    private void Update()
    {
        if (Globals.alertBoxIsShown || !canvasGroup.interactable)
            return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (m_navBtnClickSE != null)
                m_navBtnClickSE.Play();
            Back();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (m_navBtnClickSE != null)
                m_navBtnClickSE.Play();
            Next();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            m_selectionItem.TriggerEvent();
        }
    }


    public void Next()
    {
        // Increase current selection index and make sure to cycle
        m_currentSelectionIdx++;
        m_currentSelectionIdx = Utils.Cycle(m_currentSelectionIdx, 0, maxSelectionCount);
        m_selectionItem.SetData(m_selectionDataSet[m_currentSelectionIdx]);
    }


    public void Back()
    {
        // Decrease current selection index and make sure to cycle
        m_currentSelectionIdx--;
        m_currentSelectionIdx = Utils.Cycle(m_currentSelectionIdx, 0, maxSelectionCount);
        m_selectionItem.SetData(m_selectionDataSet[m_currentSelectionIdx]);
    }
}
