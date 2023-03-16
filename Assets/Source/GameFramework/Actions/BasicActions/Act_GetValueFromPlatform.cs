using UnityEngine;

namespace PF.Actions
{
    public class Act_GetValueFromPlatform : Act_Base
    {
        public LLValueManager valueManager;


        public override void Init(Player player, BaseLinkedListLevel levelBase, int maxUseCount)
        {
            base.Init(player, levelBase, maxUseCount);

            printName = "Get Node Value";
            description = "Gets the value from a Node and stores it to the inventory.";
        }


        public override void InvokeAction()
        {
            PlayerCharaCosmo chara = m_player.GetChara();
            Platform currentPlatform = chara.GetPlatform();
            if (currentPlatform == null)
            {
                Debug.Log("COSMO: I can't get a value, I'm not standing on a platform.");
            }
            else if (currentPlatform != null && currentPlatform.value == 0)
            {
                Debug.Log("COSMO: There is no value on this platform.");
            }
            else
            {

                // Check if the player's inventory is full. If it is full, don't take any values
                if (m_player.inventory.Get() != 0)
                {
                    Debug.Log("COSMO: I'm already holding a value in my bag.");
                }
                else
                {
                    int value = currentPlatform.value;
                    m_player.inventory.Store(value);
                    currentPlatform.SetValue(0);
                    useCount++;

                    // これじゃだめですよ！
                    //valueManager.UnuseValue(value);
                }
            }
        }
    }
}
