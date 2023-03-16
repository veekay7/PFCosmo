// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;

public class CreditsScreenPage : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private bool m_hidden = false;

    public CanvasGroup canvasGroup { get; private set; }
    public UIAutoscroll autoscroll { get; private set; }


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        autoscroll = GetComponent<UIAutoscroll>();
        autoscroll.Stop();
    }


    public void Show()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        if(autoscroll)
        autoscroll.Begin();
    }


    public void Hide()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        autoscroll.Stop();
    }


    [ContextMenu("Toggle Hidden")]
    private void ToggleHidden()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        m_hidden = !m_hidden;
        canvasGroup.alpha = m_hidden ? 0.0f : 1.0f;
        canvasGroup.interactable = !m_hidden;
        canvasGroup.blocksRaycasts = !m_hidden;
    }
}
