using UnityEngine;

public class GameScreenCanvas : MonoBehaviourSingleton<GameScreenCanvas>
{
    [SerializeField]
    private ScreenFader m_screenFader = null;
    [SerializeField]
    private LoadingScreen m_loadingScreen = null;
    [SerializeField]
    private MissionScreen m_missionScreen = null;
    [SerializeField]
    private PauseScreen m_pauseScreen = null;
    [SerializeField]
    private ResultScreen m_resultScreen = null;

    public Canvas canvas { get; private set; }
    public ScreenFader screenFader => m_screenFader;
    public LoadingScreen loadingScreen => m_loadingScreen;
    public MissionScreen missionScreen => m_missionScreen;
    public PauseScreen pauseScreen => m_pauseScreen;
    public ResultScreen resultScreen => m_resultScreen;


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
        missionScreen.Hide();
        pauseScreen.Hide();
        resultScreen.Hide();
    }
}
