// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System.Collections.Generic;
using UnityEngine;
using PF;
using PF.Actions;

public class UIActionButtonList : UIWidget
{
    [SerializeField]
    private UIActionButton m_actionButtonPrefab = null;
    [SerializeField]
    private Transform m_actionScrollContent = null;
    [SerializeField, ReadOnly]
    private List<UIActionButton> m_actionButtons = new List<UIActionButton>();


    protected override void Awake()
    {
        base.Awake();

        // Remember to deactivate action button prefab
        m_actionButtonPrefab.gameObject.SetActive(false);
    }


    public void SetEntityActions(Act_Base[] actions)
    {
        Clear();

        if (actions != null || actions.Length != 0)
        {
            for (int i = 0; i < actions.Length; i++)
            {
                Act_Base a = actions[i];
                UIActionButton newActionBtn = CreateNewButton();
                newActionBtn.SetActionData(a);
                newActionBtn.gameObject.SetActive(true);
                m_actionButtons.Add(newActionBtn);
            }
        }
    }


    private UIActionButton CreateNewButton()
    {
        Debug.Assert(m_actionButtonPrefab != null, "Prefab is null");

        UIActionButton newButton = Instantiate(m_actionButtonPrefab);
        newButton.transform.SetParent(m_actionScrollContent, false);
        return newButton;
    }


    private void Clear()
    {
        for (int i = 0; i < m_actionButtons.Count; i++)
        {
            Destroy(m_actionButtons[i]);
        }

        m_actionButtons.Clear();
    }


    private void OnDestroy()
    {
        Clear();
    }
}
