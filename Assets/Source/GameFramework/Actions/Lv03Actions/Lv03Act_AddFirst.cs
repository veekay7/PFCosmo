using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PF.Actions
{
    public class Lv03Act_AddFirst : Act_Base
    {
        protected Lv03AltMergeLevel m_theLevel;
        private bool m_canInvoke;


        public override void Init(Player player, BaseLinkedListLevel levelBase, int maxUseCount)
        {
            base.Init(player, levelBase, maxUseCount);
            printName = "Add First";
            description = "Insert a new Node towards the starting point.";
            m_theLevel = m_levelBase as Lv03AltMergeLevel;
            m_canInvoke = true;
        }


        public override void InvokeAction()
        {
            if (!m_canInvoke)
                return;
            m_canInvoke = false;

            m_theLevel.gameCamera.EnableSceneCtrlMode();
            m_player.StartCoroutine(
                m_theLevel.gameCamera.Co_MoveCamToLocation(m_theLevel.camMoveHint01.position, () =>
                {
                    m_theLevel.AddFirst((platform) =>
                    {
                        // Always call to update the current standing platform after moving
                        m_player.GetChara().CheckCurrentStandingPlatform();
                        m_player.StartCoroutine(
                            m_theLevel.gameCamera.Co_ResetCamToTarget(() =>
                            {
                                m_canInvoke = true;
                                m_theLevel.gameCamera.DisableSceneCtrlMode();
                            }));
                    });
                    useCount++;
                }));
        }
    }
}
