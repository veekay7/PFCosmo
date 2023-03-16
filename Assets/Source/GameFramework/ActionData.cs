// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using UnityEngine;
using UnityEngine.Events;

namespace PF
{
    [Serializable]
    public class ActionData
    {
        public string printName = string.Empty;
        public Sprite icon = null;
        public string description = string.Empty;
        public bool canUseAction = true;
        public int maxUseCount = 1;
        public ActionButtonClickedEvent ActionListener = null;

        [HideInInspector]
        public int useCount = 0;
        [HideInInspector]
        public UIActionButton button = null;
    }
}

[Serializable]
public class ActionButtonClickedEvent : UnityEvent<PF.ActionData> { }
