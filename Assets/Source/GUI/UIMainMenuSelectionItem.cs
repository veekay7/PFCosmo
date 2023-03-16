// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMainMenuSelectionItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Color m_normalColor = Color.white;  // 64CFFF
    [SerializeField]
    private Color m_disabledColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 1.0f);

    private UnityEvent onInteract;

    public UIMainMenuSelectionData data { get; private set; }
    public CanvasGroup canvasGroup { get; private set; }
    public AudioSource audioSource { get; private set; }
    public Text label { get; private set; }
    public bool isInteractable { get; private set; } = true;


    private void OnEnable()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();
        label = GetComponent<Text>();
        label.text = "Selection_Item";
    }


    private void Update()
    {
        label.color = isInteractable ? m_normalColor : m_disabledColor;
    }


    public void SetData(UIMainMenuSelectionData newData)
    {
        Debug.Assert(newData != null);

        if (data != newData)
        {
            // Clear out the event listener attached
            if (onInteract != null)
            {
                onInteract.RemoveAllListeners();
                onInteract = null;
            }

            // Set the new data
            label.text = newData.printName;
            gameObject.name = "Selection_" + label.text;
            isInteractable = newData.enabled;
            onInteract = newData.onInteract;
        }
    }


    public void TriggerEvent()
    {
        if (isInteractable)
        {
            audioSource.Play();

            if (onInteract != null)
                onInteract.Invoke();
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        TriggerEvent();
    }
}
