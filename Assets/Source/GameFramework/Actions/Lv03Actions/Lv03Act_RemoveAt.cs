using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PF.Actions
{
    public class Lv03Act_RemoveAt : Act_Base
    {
        private Lv03AltMergeLevel m_theLevel;


        public override void Init(Player player, BaseLinkedListLevel levelBase, int maxUseCount)
        {
            base.Init(player, levelBase, maxUseCount);
            printName = "Remove At";
            description = "Delete a new Node from the desired location.";
            m_theLevel = m_levelBase as Lv03AltMergeLevel;
        }


        public override void InvokeAction()
        {
            Platform p = m_player.GetChara().GetPlatform();
            if (p == null)
            {
                Debug.Log("COSMO: I can't add a platform at this location");
                return;
            }

            int platformIdx = m_theLevel.mainPuzzle.IndexOf(p);
            if (platformIdx != -1)
            {
                m_theLevel.gameCamera.EnableSceneCtrlMode();

                // Move the chara back to the start
                m_player.GetChara().Launch(m_levelBase.start.transform);
                m_player.playerHudInst.DisableInput();

                // Wait for chara to finish movement
                m_player.StartCoroutine(Co_WaitForCharaFinishMovement(() =>
                {
                    m_player.playerHudInst.DisableInput();  // Because Co_WaitForCharaFinishMovement overrides and re-enables input.

                    // Move the camera to the move hint
                    m_player.StartCoroutine(
                    m_theLevel.gameCamera.Co_MoveCamToLocation(m_theLevel.camMoveHint01.position, () =>
                    {
                        // Add at location we require
                        m_theLevel.RemoveAt(platformIdx, (platform) =>
                        {
                            // Reset the camera
                            m_player.StartCoroutine(m_theLevel.gameCamera.Co_ResetCamToTarget(() =>
                            {
                                m_theLevel.gameCamera.DisableSceneCtrlMode();
                                m_player.playerHudInst.EnableInput();
                            }));
                        });
                        useCount++;
                    }));
                }));
            }
        }
    }
}
