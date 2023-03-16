// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;

namespace PF.Actions
{
    public class Lv03Act_GotoNextPuzzle : Act_Base
    {
        private bool m_canInvoke;
        private Lv03AltMergeLevel m_theLevel;


        public override void Init(Player player, BaseLinkedListLevel levelBase, int maxUseCount)
        {
            base.Init(player, levelBase, maxUseCount);
            printName = "Goto Next LL";
            description = "Go to the next Linked List.";
            m_theLevel = m_levelBase as Lv03AltMergeLevel;
            m_canInvoke = true;
        }


        public override void InvokeAction()
        {
            if (!m_canInvoke)
                return;
            m_canInvoke = false;

            Puzzle current = m_theLevel.mainPuzzle;
            Transform location = null;
            if (current == m_theLevel.puzzle01)
            {
                m_theLevel.mainPuzzle = m_theLevel.puzzle02;
                m_theLevel.start = m_theLevel.start02;
                m_theLevel.goal = m_theLevel.goal02;
                location = m_theLevel.start02.transform;
            }
            else if (current == m_theLevel.puzzle02)
            {
                m_theLevel.mainPuzzle = m_theLevel.puzzle01;
                m_theLevel.start = m_theLevel.start01;
                m_theLevel.goal = m_theLevel.goal01;
                location = m_theLevel.start.transform;
            }

            // Now take Cosmo and move her to the desired positions
            m_player.GetChara().position = location.position;
            m_player.GetChara().CheckCurrentStandingPlatform();

            m_theLevel.gameCamera.EnableSceneCtrlMode();
            m_player.StartCoroutine(m_theLevel.gameCamera.Co_ResetCamToTarget(() =>
            {
                m_canInvoke = true;
                m_theLevel.gameCamera.DisableSceneCtrlMode();
            }));
            useCount++;
        }
    }
}
