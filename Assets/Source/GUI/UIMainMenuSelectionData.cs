using System;
using UnityEngine.Events;

[Serializable]
public class UIMainMenuSelectionData
{
    public string printName = string.Empty;
    public bool enabled = false;
    public UnityEvent onInteract = new UnityEvent();
}
