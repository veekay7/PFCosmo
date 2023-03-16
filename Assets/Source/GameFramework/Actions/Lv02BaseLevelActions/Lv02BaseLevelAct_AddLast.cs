using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PF.Actions
{
    public class Lv02BaseLevelAct_AddLast : Act_Base
    {
        protected Lv02BaseLevel m_theLevel;
        private bool m_canInvoke;

        public override void Init(Player player, BaseLinkedListLevel levelBase, int maxUseCount)
        {
            base.Init(player, levelBase, maxUseCount);

            printName = "Add Last";
            description = "Insert a new Node towards the finishing point.";
            m_theLevel = m_levelBase as Lv02BaseLevel;
            m_canInvoke = true;
        }


        public override void InvokeAction()
        {
            if (!m_canInvoke)
                return;
            m_canInvoke = false;
            m_theLevel.gameCamera.EnableSceneCtrlMode();

            m_player.playerHudInst.DisableInput();
            m_player.StartCoroutine(
                m_theLevel.gameCamera.Co_MoveCamToLocation(m_theLevel.camMoveHint01.position, () =>
                {
                    m_theLevel.AddLast((platform) =>
                    {
                        m_player.GetChara().CheckCurrentStandingPlatform();
                        m_player.StartCoroutine(
                            m_theLevel.gameCamera.Co_ResetCamToTarget(() =>
                            {
                                m_canInvoke = true;
                                m_player.playerHudInst.EnableInput();
                                m_theLevel.gameCamera.DisableSceneCtrlMode();
                            }));
                    });
                    useCount++;
                }));
        }
    }
}
