using UnityEngine;
using UnityEngine.UI;

public class UIMessageOverlay : MonoBehaviour
{
    [Header("Config")]
    [Tooltip("Fades out this message overlay automatically after set amount of time. 0.0 will not make the overlay fade out automatically.")]
    public float fadeOutAfterSeconds = 0.0f;
    [SerializeField]
    public Text messageTextComponent;

    public CanvasGroup canvasGroup { get; private set; }
    public bool isShown { get; private set; }


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }


    private void Reset()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        isShown = false;
    }


    private void Start()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        isShown = false;
    }


    public void Open(string msg)
    {
        if (isShown)
            return;

        messageTextComponent.text = msg;
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        isShown = true;
    }


    public void Close()
    {
        if (!isShown)
            return;

        messageTextComponent.text = string.Empty;
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        isShown = false;
    }
}
