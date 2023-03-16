using UnityEngine;

namespace PF.Actions
{
    public class Act_ReturnValueToStore : Act_Base
    {
        public override void Init(Player player, BaseLinkedListLevel levelBase, int maxUseCount)
        {
            base.Init(player, levelBase, maxUseCount);

            printName = "Return Value To Store";
            description = "Returns a value stored in the inventory back to the store";
        }


        public override void InvokeAction()
        {
            int value = m_player.inventory.Get();
            if (value != 0)
            {
                m_player.inventory.Clear();
                m_player.valueManager.UnuseValue(value);
                useCount++;
            }
            else
            {
                Debug.Log("COSMO: I'm not holding any value.");
            }
        }
    }
}
