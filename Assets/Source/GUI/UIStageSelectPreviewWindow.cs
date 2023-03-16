// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIStageSelectPreviewWindow : MonoBehaviourSingleton<UIStageSelectPreviewWindow>
{
    [Header("GUI")]
    [SerializeField]
    private Image m_stageThumbnailImage = null;
    [SerializeField]
    private Text m_stageNameText = null;
    [SerializeField]
    private Text m_stageCompleteStatusText = null;
    [SerializeField]
    private Text m_stageDescriptionText = null;

    public UnityEvent onShow = new UnityEvent();
    public UnityEvent onHide = new UnityEvent();

    public CanvasGroup canvasGroup { get; private set; }


    protected override void OnEnable()
    {
        base.OnEnable();
        canvasGroup = GetComponent<CanvasGroup>();
    }


    private void Start()
    {
        Hide();
    }


    public void Show()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        if (onShow != null)
            onShow.Invoke();
    }


    public void Hide()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        if (onHide != null)
            onHide.Invoke();
    }


    public void SetDetails(Sprite thumbnailSprite, string stageName, string stageDesc, bool stageComplete)
    {
        Debug.Assert(m_stageThumbnailImage != null && m_stageNameText != null && m_stageCompleteStatusText != null && m_stageDescriptionText != null);

        m_stageThumbnailImage.sprite = thumbnailSprite;
        m_stageNameText.text = stageName;
        m_stageDescriptionText.text = stageDesc;
        m_stageCompleteStatusText.text = stageComplete ? "完成" : string.Empty;
    }


    public void ClearDetails()
    {
        m_stageThumbnailImage.sprite = null;
        m_stageNameText.text = string.Empty;
        m_stageDescriptionText.text = string.Empty;
        m_stageCompleteStatusText.text = string.Empty;
    }
}
