// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPromptOverlay : MonoBehaviour
{
    public Text topTextComponent;
    public Text lowerTextComponent;

    public event Action onOpen = null;
    public event Action onClose = null;

    public CanvasGroup canvasGroup { get; private set; }
    public bool isShown { get; private set; }


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }


    private void Start()
    {
        //lowerTextComponent.text = "Escape to cancel";
        Reset();
    }


    private void Reset()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = false;
    }


    public void Show(string msg1, string msg2)
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        topTextComponent.text = msg1;
        lowerTextComponent.text = msg2;

        isShown = true;

        onOpen?.Invoke();
    }


    public void Close()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        topTextComponent.text = string.Empty;
        lowerTextComponent.text = string.Empty;

        isShown = false;

        onClose?.Invoke();
    }


    public void SetTopMsg(string msg)
    {
        topTextComponent.text = msg;
    }


    public void SetBottomMsg(string msg)
    {
        lowerTextComponent.text = msg;
    }
}
