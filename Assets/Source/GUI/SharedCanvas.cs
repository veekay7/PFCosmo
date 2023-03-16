using UnityEngine;

public class SharedCanvas : MonoBehaviourSingleton<SharedCanvas>
{
    [SerializeField]
    private ScreenFader m_screenFader = null;
    [SerializeField]
    private LoadingScreen m_loadingScreen = null;

    public Canvas canvas { get; private set; }
    public ScreenFader screenFader => m_screenFader;
    public LoadingScreen loadingScreen => m_loadingScreen;


    protected override void OnEnable()
    {
        canvas = GetComponent<Canvas>();
        canvas.sortingOrder = 999;

        HideAll();

        base.OnEnable();
    }


    [ContextMenu("Hide All")]
    public void HideAll()
    {
        screenFader.ShowScreen();
        loadingScreen.Hide();
    }
}
