using System;
using UnityEngine;

[Serializable]
public class StageData : ScriptableObject
{
    [SerializeField]
    private string m_sceneName = string.Empty;
    [SerializeField]
    private string m_printName = string.Empty;
    [SerializeField]
    private Sprite m_thumbnail = null;
    [SerializeField, Multiline]
    private string m_instructions = string.Empty;


    public string sceneName => m_sceneName;
    public string printName => m_printName;
    public Sprite thumbnail => m_thumbnail;
    public string instructions => m_instructions;
}
