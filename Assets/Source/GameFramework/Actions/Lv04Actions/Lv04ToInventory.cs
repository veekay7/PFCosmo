using UnityEngine;
using UnityEngine.UI;
using System;

namespace PF.Actions
{
    public class Lv04ToInventory : Act_Base
    {
        private Lv04InsertLL m_theLevel;

        public override void Init(Player player, BaseLinkedListLevel levelBase, int maxUseCount)
        {
            Debug.Log("INITIALIZING L04INVENTORY");
            base.Init(player, levelBase, maxUseCount);
            printName = "To Inventory";
            description = "Insert a new Node at the desired location.";
            m_theLevel = m_levelBase as Lv04InsertLL;
        }


        public override void InvokeAction()
        {

            string name = GameObject.Find("InputField").GetComponent<InputField>().text;
            Debug.Log("Saving " + name + "into Inventory");
            int value = System.Convert.ToInt32(name);
            m_player.inventory.Store(value);


        }
    }
}