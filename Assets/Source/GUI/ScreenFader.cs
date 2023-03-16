using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(CanvasGroup))]
public class ScreenFader : MonoBehaviour
{
    public CanvasGroup canvasGroup { get; private set; }
    public Image imageComponent { get; private set; }

    public bool IsShown { get; private set; }


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        imageComponent = GetComponent<Image>();
    }


    [ContextMenu("Show Screen")]
    public void ShowScreen()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }


    [ContextMenu("Hide Screen")]
    public void HideScreen()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }


    public IEnumerator Co_FadeInScreen(float fadeTime, Action onComplete = null)
    {
        float t = 0.0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;

            float perc = t / fadeTime;
            canvasGroup.alpha = Mathf.Lerp(1.0f, 0.0f, perc);
            yield return null;
        }

        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        if (onComplete != null)
            onComplete.Invoke();
    }


    public IEnumerator Co_FadeOutScreen(float fadeTime, Action onComplete = null)
    {
        float t = 0.0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;

            float perc = t / fadeTime;
            canvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, perc);
            yield return null;
        }

        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        IsShown = false;

        if (onComplete != null)
            onComplete.Invoke();
    }
}
