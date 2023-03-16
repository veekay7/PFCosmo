using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PF.Actions
{
    public class Act_CmpMoreThan : Act_Base
    {
        public override void Init(Player player, BaseLinkedListLevel levelBase, int maxUseCount)
        {
            base.Init(player, levelBase, maxUseCount);

            printName = "Cmp More Than";
            description = "platform.value > inventory.value;";
        }


        public override void InvokeAction()
        {
            if (isPendingInput)
                return;
            isPendingInput = true;

            PlayerCharaCosmo chara = m_player.GetChara();
            if (chara.GetPlatform() != null)
            {
                int inventoryValue = m_player.inventory.Get();
                int platformValue = chara.GetPlatform().value;
                if (inventoryValue == 0)
                {
                    Debug.Log("COSMO: I am not holding a value that I can use to compare.");
                }
                else if (platformValue == 0)
                {
                    Debug.Log("COSMO: The platform has no value for me to compare.");
                }
                else
                {
                    // Show the fukidashi
                    //chara.fukidashi.SetText(inventoryValue.ToString() + " > " + platformValue.ToString());
                    chara.fukidashi.Show();
                    isPendingInput = true;
                }
            }
            else
            {
                Debug.Log("COSMO: I can't do any comparison like this.");
            }
        }
    }
}
