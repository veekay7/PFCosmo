using UnityEngine;
using UnityEngine.UI;
using System;

namespace PF.Actions
{
    public class Lv04Act_AddAt : Act_Base
    {
        private Lv04InsertLL m_theLevel;

        public override void Init(Player player, BaseLinkedListLevel levelBase, int maxUseCount)
        {
            base.Init(player, levelBase, maxUseCount);
            printName = "Add At";
            description = "Insert a new Node at the desired location.";
            m_theLevel = m_levelBase as Lv04InsertLL;
        }


        public override void InvokeAction()
        {

            if (m_player.inventory.Get() == 0)
            {
                Debug.Log("COSMO: I'm not holding a value in my inventory.");
                return;
            }

            Platform p = m_player.GetChara().GetPlatform();
            if (p == null)
            {
                Debug.Log("COSMO: I can't add a platform at this location.");
                return;
            }

            int platformIdx = m_theLevel.mainPuzzle.IndexOf(p);
            if (platformIdx != 0)
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
                            m_theLevel.AddAt(platformIdx, (platform) =>
                            {
                                if (platform != null)
                                {
                                    int value = m_player.inventory.Get();
                                    //int value = 0;
                                    //value = Convert.ToInt32(platformValue.submitName());
                                    platform.SetValue(value);
                                    m_player.inventory.Clear();
                                }
                                else
                                {
                                    Debug.Log("Cannot add new platform here.");
                                }
                                    
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
