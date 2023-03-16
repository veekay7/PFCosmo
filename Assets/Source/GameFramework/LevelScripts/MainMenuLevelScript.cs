// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuLevelScript : LevelScript
{
    [SerializeField]
    private PlayerProfileDb m_playerProfileDb = null;
    [SerializeField]
    private AudioSource m_bgmPlayer = null;
    [SerializeField]
    private ProfilesWindow m_profileWindow = null;
    [SerializeField]
    private NewProfileWindow m_newProfileWindow = null;
    //[SerializeField]
    //private CanvasGroup m_startupScreen = null;
    [SerializeField]
    private EnterScreen m_enterScreen = null;
    [SerializeField]
    private CanvasGroup m_inputBlock = null;
    [SerializeField]
    private Text m_loggedInPlayerText = null;

    [Header("Canvas")]
    [SerializeField]
    private SharedCanvas m_sharedCanvasPrefab = null;
    [SerializeField]
    private AlertBoxController m_alertBoxCtrlPrefab = null;

    // States
    private StateMachineController m_stateController;
    private MainMenu_StartupState m_startupState;
    private MainMenu_TitleState m_titleState;


    protected override void Awake()
    {
        // Create shit we need
        SharedCanvas sharedCanvasInst = Instantiate(m_sharedCanvasPrefab);
        if (sharedCanvasInst == null)
            Debug.LogWarning("Shared Canvas could not be instantiated!");

        AlertBoxController alertBoxCtrlInst = Instantiate(m_alertBoxCtrlPrefab);
        if (alertBoxCtrlInst == null)
            Debug.LogWarning("Alert Box Controller could not be instantiated!");

        // Initialize the player profile database
        m_playerProfileDb.Init();
        m_profileWindow.SetList(m_playerProfileDb.GetProfiles());

        m_stateController = new StateMachineController(this);
        m_stateController.AddState("Startup");
        m_stateController.AddState("Title");
        m_stateController.AddState("LoginMenu");
        m_stateController.AddState("MainMenu");
        m_stateController.onStateBegin.AddListener(OnStateBegin);
        m_stateController.onStateUpdate.AddListener(OnStateUpdate);
        m_stateController.onStateEnd.AddListener(OnStateEnd);

        m_startupState = GetComponent<MainMenu_StartupState>();
        m_startupState.Init(m_stateController);
        m_titleState = GetComponent<MainMenu_TitleState>();
        m_titleState.Init(m_stateController);
    }


    protected override void Start()
    {
        // When entering this level, two conditions to check beforehand
        // If entered from when first starting the game, start from startup
        if (GameApp.FirstStart)
        {
            // The game has not start from opening exe but came from elsewhere.
            // Now that we have started the game before, set flag to true.
            GameApp.FirstStart = false;
            m_stateController.GoToState("Startup");
        }
        else
        {
            // TODO:
            // If the game app has already started before, check the following conditions:
            // If there is a logged in player (local/online), show Main Menu Selection.
            // If there is no logged in player, show Title Screen.
            if (GameApp.CurrentProfile != null)
            {
                if (!m_bgmPlayer.isPlaying)
                    m_bgmPlayer.Play();
                m_stateController.GoToState("MainMenu");
            }
            else
            {
                if (!m_bgmPlayer.isPlaying)
                    m_bgmPlayer.Play();
                m_stateController.GoToState("Title");
            }
        }

        EnableInput();
    }


    private void Update()
    {
        m_stateController.Update();
    }


    public void PlayFreePlayMode()
    {
        // If the profile does not exist, create one and log in.
        bool devProfileExists = PlayerProfile.DoesDevProfileExist();
        if (!devProfileExists)
        {
            // If a developer profile does not exist, create one
            GameApp.CurrentProfile = PlayerProfile.CreateDevProfile();
        }
        else
        {
            // Local profile exists. Load that profile instead.
            GameApp.CurrentProfile = PlayerProfile.LoadDevProfileFromDisk();
        }

        m_stateController.GoToState("MainMenu");
    }


    public void Login()
    {
        if (m_profileWindow.current != null)
        {
            UIPlayerProfileItem item = m_profileWindow.current;
            GameApp.CurrentProfile = m_playerProfileDb.Get(item.GetProfile().username, item.GetProfile().email);
            if (GameApp.CurrentProfile == null)
            {
                AlertBoxBasic alertBox = AlertBoxController.instance.basicAlertBox;
                alertBox.Present("An unknown error occurred.", "Error");
            }
            else
            {
                m_loggedInPlayerText.gameObject.SetActive(true);
                m_loggedInPlayerText.text = GameApp.CurrentProfile.username;
                DisableInput();
                m_profileWindow.Hide();
                m_stateController.GoToState("MainMenu");
            }
        }
    }


    public void Logout()
    {
        if (GameApp.CurrentProfile != null)
            GameApp.CurrentProfile = null;

        m_stateController.GoToState("LoginMenu");
    }


    public void CreateNewProfile()
    {
        // First, let's validate if the form is valid
        m_newProfileWindow.Validate((data) =>
        {
            // Now create the profile
            string username = data["name"];
            string email = data["email"];
            bool createResult = m_playerProfileDb.Create(username, email);
            if (createResult)
            {
                // After creating profile is successful, let's try to get it for logging in.
                GameApp.CurrentProfile = m_playerProfileDb.Get(username, email);
                if (GameApp.CurrentProfile == null)
                {
                    AlertBoxBasic alertBox = AlertBoxController.instance.basicAlertBox;
                    alertBox.Present("An unknown error occurred.", "Error", null);
                }
                else
                {
                    DisableInput();
                    m_newProfileWindow.Hide();
                    m_stateController.GoToState("MainMenu");
                }
            }
            else
            {
                AlertBoxBasic alertBox = AlertBoxController.instance.basicAlertBox;
                alertBox.Present("A profile with the entered username and email already exists.", "Error", null);
            }
        }, (error) =>
        {
            AlertBoxBasic alertBox = AlertBoxController.instance.basicAlertBox;
            alertBox.Present(error, "Error", null);
        });
    }


    public void GoToStageSelect()
    {
        StartCoroutine(Co_GoToNextScene("StageSelect"));
    }


    public void QuitApplication()
    {
        AlertBoxConfirm confirmBox = AlertBoxController.instance.confirmAlertBox;
        string msg = "Are you sure you want to quit the game?";

        confirmBox.Present(msg, "Quit", GameApp.Quit, () => { Globals.alertBoxIsShown = false; });
        Globals.alertBoxIsShown = true;    // Used to change logic of any UI element or game object when an alertBox is shown
    }


    private IEnumerator Co_GoToNextScene(string scene)
    {
        if (string.IsNullOrEmpty(scene))
        {
            Debug.LogWarning("Scene parameter is empty");
            yield break;
        }

        DisableInput();
        yield return SharedCanvas.instance.screenFader.Co_FadeOutScreen(1.0f);
        SceneManager.LoadScene(scene);
        EnableInput();
    }


    private void OnStateBegin(string state)
    {
        switch (state)
        {
            case "Startup":
                m_startupState.OnStartupStateBegin();
                break;

            case "Title":
                m_titleState.OnTitleStateBegin();
                break;

            case "LoginMenu":
                OnLoginMenuStateBegin();
                break;

            case "MainMenu":
                OnMainMenuBegin();
                break;

            default:
                Debug.LogWarning("No such state at " + state);
                break;
        }
    }


    private void OnStateUpdate(string state)
    {
        switch (state)
        {
            case "Startup":
                m_startupState.OnStartupStateUpdate();
                break;

            case "Title":
                m_titleState.OnTitleStateUpdate();
                break;
        }
    }


    private void OnStateEnd(string state)
    {
        switch (state)
        {
            case "Startup":
                m_startupState.OnStartupStateEnd();
                break;

            case "Title":
                m_titleState.OnTitleStateEnd();
                break;

            case "LoginMenu":
                OnLoginMenuStateEnd();
                break;

            case "MainMenu":
                OnMainMenuEnd();
                break;
        }
    }


    private void OnLoginMenuStateBegin()
    {
        m_enterScreen.DisplayLoginMenu(true);
        m_stateController.TickState = true;
    }


    private void OnLoginMenuStateEnd()
    {
        m_stateController.TickState = false;
    }


    private void OnMainMenuBegin()
    {
        float loginMenuDisplay = 0.0f;
        if (GameApp.CurrentProfile != null)
            loginMenuDisplay = 0.0f;
        else
            loginMenuDisplay = 1.0f;

        // Fade out the Login Menu
        StartCoroutine(m_enterScreen.Co_FadeWidget(m_enterScreen.loginMenu.canvasGroup, true, loginMenuDisplay, () =>
        {
            m_enterScreen.DisplayLoginMenu(false);

            // Show the Main Menu Selection
            m_enterScreen.mainMenuSelection.enabled = true;
            StartCoroutine(m_enterScreen.Co_FadeWidget(m_enterScreen.mainMenuSelection.canvasGroup, false, 1.0f, () =>
            {
                m_enterScreen.mainMenuSelection.Show();
                m_stateController.TickState = true;
                EnableInput();
            }));
        }));
    }


    private void OnMainMenuEnd()
    {
        m_enterScreen.mainMenuSelection.ResetCurrentSelectionIndex();
        m_enterScreen.mainMenuSelection.enabled = false;
        m_enterScreen.DisplayLoginMenu(true);
        m_stateController.TickState = false;
    }


    private void EnableInput()
    {
        m_inputBlock.alpha = 0.0f;
        m_inputBlock.interactable = false;
        m_inputBlock.blocksRaycasts = false;
    }


    private void DisableInput()
    {
        m_inputBlock.alpha = 1.0f;
        m_inputBlock.interactable = true;
        m_inputBlock.blocksRaycasts = true;
    }
}
