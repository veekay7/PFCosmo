// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AlertBoxConfirm : UIWidget
{
    [SerializeField]
    private Text m_captionText = null;
    [SerializeField]
    private Text m_messageText = null;
    [SerializeField]
    private Button m_okButton = null;
    [SerializeField]
    private Button m_cancelButton = null;

    public UnityEvent onOkPressed = new UnityEvent();
    public UnityEvent onCancelPressed = new UnityEvent();


    protected override void Awake()
    {
        base.Awake();

        m_okButton.onClick.AddListener(OnOkPressedEvent);
        m_cancelButton.onClick.AddListener(OnCancelPressedEvent);
        Hide();
    }


    public void Present(string msg, string caption, UnityAction okAction, UnityAction cancelAction)
    {
        Debug.Assert(m_captionText != null && m_messageText != null);

        m_captionText.text = caption;
        m_messageText.text = msg;

        // Remove all listeners first
        onOkPressed.RemoveAllListeners();
        onCancelPressed.RemoveAllListeners();

        if (okAction != null)
            onOkPressed.AddListener(okAction);
        onOkPressed.AddListener(Hide);

        if (cancelAction != null)
            onCancelPressed.AddListener(cancelAction);
        onCancelPressed.AddListener(Hide);

        Show();
    }


    public override void Show()
    {
        base.Show();
    }


    public override void Hide()
    {
        base.Hide();

        Debug.Assert(m_captionText != null && m_messageText != null);

        
        m_captionText.text = "Caption";
        m_messageText.text = "Message";
    }


    private void OnOkPressedEvent()
    {
        if (onOkPressed != null)
            onOkPressed.Invoke();
    }


    private void OnCancelPressedEvent()
    {
        if (onCancelPressed != null)
            onCancelPressed.Invoke();
    }


    private void OnDestroy()
    {
        onOkPressed.RemoveAllListeners();
        onCancelPressed.RemoveAllListeners();
    }
}
