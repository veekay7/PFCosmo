// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;
using UnityEngine.UI;
using PF;
using PF.Actions;

public class UIActionButton : MonoBehaviour
{
    //[SerializeField]
    private Button m_button;
    private Image m_buttonImage;
    private TooltipTrigger m_tooltipTrigger;
    private string m_printName;
    private string m_desc;
    private Sprite m_icon;
    private Act_Base m_action;


    private void OnEnable()
    {
        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(OnActionButtonClicked);

        m_buttonImage = GetComponent<Image>();
        m_buttonImage.sprite = m_icon;

        m_tooltipTrigger = GetComponent<TooltipTrigger>();
        m_tooltipTrigger.SetTooltip(m_printName + "\n" + m_desc);
    }


    public void SetActionData(Act_Base newAction)
    {
        Debug.Assert(newAction != null, "");
        m_action = newAction;
        m_action.button = this;

        m_icon = m_action.icon;
        m_printName = m_action.printName;
        m_desc = m_action.description;
    }


    public void CleanUpButton()
    {
        m_action = null;

        if (m_buttonImage != null)
            m_buttonImage.sprite = null;

        m_printName = string.Empty;
        m_desc = string.Empty;
    }


    public void SetInteractable(bool value)
    {
        m_button.interactable = value;
    }


    private void OnActionButtonClicked()
    {
        if (m_action == null)
            return;
        m_action.InvokeAction();
    }
}
