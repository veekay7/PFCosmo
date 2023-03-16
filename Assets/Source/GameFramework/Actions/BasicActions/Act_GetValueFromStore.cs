using System;
using UnityEngine;

namespace PF.Actions
{
    [Serializable]
    public class Act_GetValueFromStore : Act_Base
    {
        public override void Init(Player player, BaseLinkedListLevel levelBase, int maxUseCount)
        {
            base.Init(player, levelBase, maxUseCount);

            printName = "Get Value From Store";
            description = "Gets the first available value from store and puts in in the inventory.";
        }


        public override void InvokeAction()
        {
            PlayerCharaCosmo cosmo = m_player.GetChara();
            if (cosmo.GetPlatform() == null)
            {
                // If Cosmo is not standing on any platform (then she's probably at the start)
                // then we are allowed to get a value from the value manager.
                // もしインベントリーの中に何がないなら、
                if (m_player.inventory.Get() == 0)
                {
                    // ここで、まずValueManagerからバリューを受ける、そしてプレイヤーのインベントリーにお置きる。
                    int value = m_levelBase.valueManager.GetValueAndUse();
                    m_player.inventory.Store(value);
                    useCount++;
                }
            }
            else
            {
                Debug.Log("COSMO: I can't get a value from the value store. I need to return to the start point.");
            }
        }
    }
}
