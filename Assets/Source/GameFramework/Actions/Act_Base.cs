using System;
using System.Collections;
using UnityEngine;

namespace PF.Actions
{
    public abstract class Act_Base : ScriptableObject
    {
        //public string id = "Act_Base";
        public string printName = string.Empty;
        public Sprite icon = null;
        public string description = string.Empty;
        
        [ReadOnly]
        public int useCount = 0;
        [ReadOnly]
        public UIActionButton button = null;

        protected Player m_player;
        protected BaseLinkedListLevel m_levelBase;
        protected GameStats m_gameStats;
        protected int m_maxUseCount = 0;
        public bool isPendingInput { get; protected set; }


        /// <summary>
        /// Initialize the action. When overriding, make sure base.Init is called first
        /// </summary>
        /// <param name="player"></param>
        /// <param name="levelBase"></param>
        /// <param name="maxUseCount"></param>
        public virtual void Init(Player player, BaseLinkedListLevel levelBase, int maxUseCount)
        {
            m_player = player;
            m_levelBase = levelBase;
            m_maxUseCount = maxUseCount;
            useCount = 0;
            isPendingInput = false;
        }


        public abstract void InvokeAction();


        public bool IsActionUsable()
        {
            bool c1 = m_maxUseCount > 0 && useCount < m_maxUseCount;
            bool c2 = m_maxUseCount == 0;
            if (c1 || c2)
                return true;
            return false;
        }


        public void ClearState()
        {
            m_player = null;
            m_levelBase = null;
            useCount = 0;
            m_maxUseCount = 0;
            button = null;
        }


        protected IEnumerator Co_WaitForCharaFinishMovement(Action onComplete = null)
        {
            if (m_player.GetChara().isLaunched)
            {
                yield return new WaitUntil(() => m_player.GetChara().isLaunched == false && m_player.GetChara().onGround);
                m_player.playerHudInst.EnableInput();

                if (onComplete != null)
                    onComplete.Invoke();
            }
        }


        protected IEnumerator Co_GetTwoInputs(Action<Platform, Platform> onComplete)
        {
            // TODO: Replace with alert box or some shiat!
            Debug.Log("Select first platform");

            GameObject tracedObject = null;
            Platform p0 = null;
            while (p0 == null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    bool result = m_player.Trace(out tracedObject, "Platforms");
                    if (result)
                        p0 = tracedObject.GetComponent<Platform>();
                }

                yield return null;
            }

            // TODO: Replace with alert box or some schit!
            Debug.Log("Select second platform");

            Platform p1 = null;
            while (p1 == null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    bool result = m_player.Trace(out tracedObject, "Platforms");
                    if (result)
                        p1 = tracedObject.GetComponent<Platform>();
                }

                yield return null;
            }

            if (onComplete != null)
                onComplete.Invoke(p0, p1);
        }
    }
}
