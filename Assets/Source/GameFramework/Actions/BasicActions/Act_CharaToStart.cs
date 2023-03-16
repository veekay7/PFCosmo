using UnityEngine;

namespace PF.Actions
{
    public class Act_CharaToStart : Act_Base
    {
        public override void Init(Player player, BaseLinkedListLevel levelBase, int maxUseCount)
        {
            base.Init(player, levelBase, maxUseCount);

            printName = "Chara Goto Start";
            description = "Cosmo will return to the starting point.";
        }


        public override void InvokeAction()
        {
            PlayerCharaCosmo chara = m_player.GetChara();
            if (chara.isLaunched)
                return;

            TravelData travelData = m_levelBase.GetTravelData();
            if (chara.GetPlatform() != null)
            {
                chara.Launch(m_levelBase.start.transform);
                m_player.playerHudInst.DisableInput();
                m_player.StartCoroutine(Co_WaitForCharaFinishMovement());
                useCount++;
            }
            else
            {
                Debug.Log("COSMO: I'm already there!");
            }


        }
    }
}
