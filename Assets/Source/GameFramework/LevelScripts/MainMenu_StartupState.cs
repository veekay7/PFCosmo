using UnityEngine;

public class MainMenu_StartupState : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup m_startupScreen = null;
    [SerializeField]
    private EnterScreen m_enterScreen = null;

    private readonly float m_fadeInTime = 3.0f;
    private readonly float m_fadeOutTime = 3.0f;
    private StateMachineController m_stateController;
    private float m_elapsedDisplayTime = 0.0f;
    private float m_displayTime = 3.5f;


    public void Init(StateMachineController stateCtrl)
    {
        m_stateController = stateCtrl;
    }


    public void OnStartupStateBegin()
    {
        m_elapsedDisplayTime = 0.0f;

        // Disable the Main Menu Selection
        m_enterScreen.mainMenuSelection.enabled = false;

        m_startupScreen.alpha = 1.0f;
        m_startupScreen.interactable = true;
        m_startupScreen.blocksRaycasts = true;

        m_stateController.TickState = true;

        StartCoroutine(SharedCanvas.instance.screenFader.Co_FadeInScreen(m_fadeInTime, null));
    }


    public void OnStartupStateUpdate()
    {
        if (Input.anyKeyDown)
        {
            m_elapsedDisplayTime = m_displayTime;
            SharedCanvas.instance.screenFader.HideScreen();
            StopAllCoroutines();
            m_stateController.GoToState("Title");
            return;
        }

        if (m_elapsedDisplayTime < m_displayTime)
        {
            m_elapsedDisplayTime += Time.deltaTime;
        }
        else
        {
            StartCoroutine(SharedCanvas.instance.screenFader.Co_FadeOutScreen(m_fadeOutTime, () =>
            {
                m_stateController.GoToState("Title");
            }));
        }
    }


    public void OnStartupStateEnd()
    {
        m_startupScreen.alpha = 0.0f;
        m_startupScreen.interactable = false;
        m_startupScreen.blocksRaycasts = false;

        m_elapsedDisplayTime = m_displayTime;
        m_stateController.TickState = false;
    }
}
