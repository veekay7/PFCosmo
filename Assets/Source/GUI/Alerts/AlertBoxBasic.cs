// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AlertBoxBasic : UIWidget
{
    [Header("Components")]
    [SerializeField]
    private Text m_captionText = null;
    [SerializeField]
    private Text m_messageText = null;
    [SerializeField]
    private Button m_okButton = null;

    public UnityEvent onOkPressed = new UnityEvent();


    protected override void Awake()
    {
        base.Awake();

        m_okButton.onClick.AddListener(OnOkButtonPressedEvent);
        Hide();
    }


    public void Present(string msg, string caption, UnityAction okAction = null)
    {
        Debug.Assert(m_messageText != null && m_captionText != null);

        m_messageText.text = msg;
        m_captionText.text = caption;

        onOkPressed.RemoveAllListeners();

        if (okAction != null)
            onOkPressed.AddListener(okAction);
        onOkPressed.AddListener(Hide);  // NOTE: This must always be here.

        Show();
    }


    public override void Show()
    {
        base.Show();
    }


    public override void Hide()
    {
        base.Hide();

        Debug.Assert(m_messageText != null && m_captionText != null);

        onOkPressed.RemoveAllListeners();
        m_messageText.text = "Text";
        m_captionText.text = "Caption";
    }


    private void OnOkButtonPressedEvent()
    {
        if (onOkPressed != null)
            onOkPressed.Invoke();
    }


    private void OnDestroy()
    {
        onOkPressed.RemoveAllListeners();
    }
}
