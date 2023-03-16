using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PF.Actions
{
    public class Lv01BaseLevelAct_SwapPlatforms : Act_Base
    {
        protected Lv01BaseLevel m_theLevel;


        public override void Init(Player player, BaseLinkedListLevel levelBase, int maxUseCount)
        {
            base.Init(player, levelBase, maxUseCount);

            printName = "Swap Node";
            description = "Swaps positions with the Node in front of the current Node Cosmo is standing on.";
            m_theLevel = m_levelBase as Lv01BaseLevel;
        }


        public override void InvokeAction()
        {
            // Get the first platform which Cosmo is standing on.
            PlayerCharaCosmo cosmo = m_player.GetChara();
            Platform p0 = cosmo.GetPlatform();
            if (p0 == null)
            {
                Debug.Log("COSMO: I'm not standing on anything.");
                return;
            }

            // Now try to get the next platform we're trying to swap with
            Platform p1 = p0.next;
            if (p1 == null)
            {
                Debug.Log("COSMO: There's no platform next to it for me to swap.");
                return;
            }

            // Swap that schit!
            // Get the platform of p0 and p1s
            PlatformSlotsController slotCtrl = m_theLevel.mainPuzzle.slotsCtrl;
            PlatformSlot s0 = slotCtrl.Get(p0);
            PlatformSlot s1 = slotCtrl.Get(p1);

            // Get the previous of p0
            Platform p0Prev = m_theLevel.mainPuzzle.GetPreviousOf(p0);
            if (p0Prev == null)     // If we encounter a node with no previous, it probably is the head
            {
                // Do the swap
                Platform p1Next = p1.next;
                p1.next = p0;
                p0.next = p1Next;

                m_theLevel.mainPuzzle.head = p1;

                //if (p0.next == null)
                //    m_theLevel.mainPuzzle.tail = p0;
            }
            else if (p0Prev != null && p1.next == null) // If we encounter a node with no next, it is most likely the tail
            {
                Platform p1Next = p1.next;
                p0Prev.next = p1;
                p1.next = p0;
                p0.next = p1Next;

                //if (p0.next == null)
                //    m_theLevel.mainPuzzle.tail = p0;
            }
            else if (p0Prev != null && p1.next != null)
            {
                // There is a previous, it is most likely a node in the middle
                Platform p1Next = p1.next;
                p0Prev.next = p1;
                p0.next = p1Next;
                p1.next = p0;
            }

            cosmo.m_enableCollisionDetection = false;
            m_player.playerHudInst.DisableInput();
            m_player.StartCoroutine(p0.Co_MoveSinerp(s1.transform.position, () =>
            {
                s1.SetPlatform(p0);
                m_player.StartCoroutine(p1.Co_MoveSinerp(s0.transform.position, () =>
                {
                    m_theLevel.isModified = true;
                    s0.SetPlatform(p1);
                    cosmo.m_enableCollisionDetection = true;
                    m_player.playerHudInst.EnableInput();
                    //m_theLevel.CheckSolution();
                }));
            }));
            useCount++;
        }
    }
}
