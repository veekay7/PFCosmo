using UnityEngine;

namespace PF.Actions
{
    public class Act_SetValueToPlatform : Act_Base
    {
        public override void Init(Player player, BaseLinkedListLevel levelBase, int maxUseCount)
        {
            base.Init(player, levelBase, maxUseCount);

            printName = "Set Value to Node";
            description = "Sets the value from inventory to Node.";
        }


        public override void InvokeAction()
        {
            PlayerCharaCosmo chara = m_player.GetChara();
            int inventoryValue = m_player.inventory.Get();
            if (chara.GetPlatform() != null)
            {
                if (inventoryValue != 0)
                {
                    chara.GetPlatform().SetValue(m_player.inventory.Get());
                    m_player.inventory.Clear();
                    useCount++;
                }
                else
                {
                    Debug.Log("COSMO: I don't have any values in my inventory");
                }
            }
        }
    }
}
