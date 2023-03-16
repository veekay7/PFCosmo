// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerProfileItem : MonoBehaviour
{
    [SerializeField]
    private Text m_buttonTextComponent = null;
    [SerializeField, ReadOnly]
    private PlayerProfile m_profile = null;
    private Button m_buttonComponent;

    public ProfilesWindow owner { get; set; }


    private void OnEnable()
    {
        m_buttonComponent = GetComponent<Button>();
        m_buttonComponent.onClick.AddListener(InvokeAction);
    }


    public void SetProfile(PlayerProfile profile)
    {
        m_profile = profile;
        if (m_profile != null)
            m_buttonTextComponent.text = profile.username;
        else
            m_buttonTextComponent.text = "Empty";
    }


    public PlayerProfile GetProfile()
    {
        return m_profile;
    }


    private void InvokeAction()
    {
        if (owner == null)
        {
            Debug.LogWarning("UI Player Profile Item Manager is null");
            return;
        }
        owner.current = this;
    }
}
