// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using System.Collections.Generic;
using UnityEngine;
using PF.Actions;

[Serializable]
public class LevelActionData
{
    public Act_Base action = null;
    public int maxUseCount = 0;
};

public class PlayerActionManager : MonoBehaviour
{
    public BaseLinkedListLevel activeLevel = null;
    [HideInInspector]
    public List<LevelActionData> actionDataList = new List<LevelActionData>();
    private List<Act_Base> m_actions = new List<Act_Base>();

    public Player player { get; protected set; }
    public bool levelIsChanging { get; set; }
    public bool charaIsMoving { get; set; }
    public bool isPendingInput { get; protected set; }


    public void SetOwner(Player player)
    {
        Debug.Assert(player != null, "player is null");
        this.player = player;
    }


    private void Start()
    {
        // Register all the functions
        for (int i = 0; i < actionDataList.Count; i++)
        {
            LevelActionData d = actionDataList[i];
            d.action.Init(player, activeLevel, d.maxUseCount);
            m_actions.Add(d.action);
        }

        player.playerHudInst.buttonList.SetEntityActions(m_actions.ToArray());
        player.playerHudInst.valueList.SetValues(player.valueManager.usableValues);

        levelIsChanging = false;
        charaIsMoving = false;
        isPendingInput = false;
    }


    private void Update()
    {
        //if (charaIsMoving)
        //{
        //    if (!m_player.GetChara().isLaunched)
        //    {
        //        charaIsMoving = false;
        //    }
        //}
    }


    public void SetActionsUsageStats()
    {
        if (GameStats.instance == null)
            return;

        for (int i = 0; i < m_actions.Count; i++)
        {
            Act_Base act = m_actions[i];
            Type actType = act.GetType();
            GameStats.instance.SetInt(actType.ToString(), act.useCount);
        }
    }


    private void OnDestroy()
    {
        for (int i = 0; i < m_actions.Count; i++)
        {
            Act_Base a = m_actions[i];
            a.ClearState();
        }
    }
}
