using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIWidget : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private bool m_hidden = false;  // NOTE: Editor Only!

    public CanvasGroup canvasGroup { get; private set; }


    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }


    [ContextMenu("Toggle Hidden")]
    protected void ToggleHidden()
    {
        m_hidden = !m_hidden;
        if (m_hidden)
            Hide();
        else
            Show();
    }


    public virtual void Show()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }


    public virtual void Hide()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
