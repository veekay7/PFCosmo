using UnityEngine;

namespace PF.Actions
{
    public class Act_MoveChara : Act_Base
    {
        public override void Init(Player player, BaseLinkedListLevel levelBase, int maxUseCount)
        {
            base.Init(player, levelBase, maxUseCount);

            printName = "Move Cosmo";
            description = "Moves Cosmo one step forward to the finishing point.";
            PlayerHudCanvas.instance.CosmoSay("Move me!");
        }


        public override void InvokeAction()
        {
            PlayerCharaCosmo chara = m_player.GetChara();
            if (chara.isLaunched)
                return;

            TravelData travelData = m_levelBase.GetTravelData();
            if (chara.GetPlatform() == null)
            {
                // If the chara is not standing on a platform right now,
                // the chara is probably on a Cliff of some kind.
                if (travelData.head != null)
                {
                    chara.Launch(travelData.head.transform);
                    m_player.playerHudInst.DisableInput();
                    m_player.StartCoroutine(Co_WaitForCharaFinishMovement());
                    useCount++;
                }
                else
                {
                    Debug.Log("COSMO: There's nothing for me to cross");
                    PlayerHudCanvas.instance.CosmoSay("There's nothing for me to cross");
                }
            }
            else
            {
                // If the character is standing on a platform, move to the next one
                // Check if there is a next first before moving...
                if (chara.GetPlatform().next != null)
                {
                    // If there is a next, we can move to the next platform
                    chara.Launch(chara.GetPlatform().next.waypoint.transform);
                    m_player.playerHudInst.DisableInput();
                    m_player.StartCoroutine(Co_WaitForCharaFinishMovement());
                    useCount++;
                }
                else
                {
                    // If there is no next, then we need to check if the current standing platform is:
                    // 1) The Tail
                    // 2) Its index is the last in the list
                    // 3) Has the solution been solved.
                    if (!m_levelBase.isSolved)
                    {
                        // If the solution is not solved yet, Cosmo can't jump to the end.
                        Debug.Log("COSMO: I can't go any further.");
                        PlayerHudCanvas.instance.CosmoSay("I can't go any further.");
                    }
                    else
                    {
                        // TODO:
                        //if (chara.currentStandingPlatform == m_level.mainPuzzle.tail &&
                        //    m_level.mainPuzzle.IndexOf(chara.currentStandingPlatform) == m_level.mainPuzzle.count)
                        //{
                        chara.Launch(m_levelBase.goal.transform);
                        m_player.playerHudInst.DisableInput();
                        m_player.StartCoroutine(Co_WaitForCharaFinishMovement());
                        useCount++;
                        //}
                    }
                }
            }
        }
    }
}
