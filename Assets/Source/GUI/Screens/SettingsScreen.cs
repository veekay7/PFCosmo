using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SettingsScreen : BaseScreen
{
    [SerializeField]
    private Dropdown m_screenResoDropdown = null;
    [SerializeField]
    private Toggle m_fullscreenToggle = null;
    [SerializeField]
    private Slider m_masterVolumeSlider = null;

    public UnityEvent onSettingsApplied = new UnityEvent();

    private int m_currentScreenResoIdx;
    private bool m_currentFullscreenState;
    private float m_currentMasterVolume;
    private Resolution[] m_supportedResos;


    protected override void Awake()
    {
        base.Awake();

        // Set up the screen resolution dropdown
        m_supportedResos = Screen.resolutions;
        if (m_screenResoDropdown != null)
        {
            m_screenResoDropdown.ClearOptions();
            List<string> supported = new List<string>();
            for (int i = 0; i < m_supportedResos.Length; i++)
            {
                Resolution current = m_supportedResos[i];
                supported.Add(current.width.ToString() + " x " + current.height.ToString() + "@" + current.refreshRate.ToString());
            }

            m_screenResoDropdown.AddOptions(supported);
            m_screenResoDropdown.onValueChanged.AddListener(UpdateCurrentScreenResoSelection);
        }

        // Set up full screen toggle
        if (m_fullscreenToggle != null)
            m_fullscreenToggle.onValueChanged.AddListener(UpdateWindowModeSelection);

        // Set up master volume slider
        if (m_masterVolumeSlider != null)
            m_masterVolumeSlider.onValueChanged.AddListener(UpdateMasterVolSelection);
    }


    public void ApplySettingsToConfig()
    {
        if (GameApp.CurrentSettings == null)
            return;

        GameApp.CurrentSettings.screenResolution = m_supportedResos[m_currentScreenResoIdx];
        GameApp.CurrentSettings.fullscreen = m_currentFullscreenState;
        GameApp.CurrentSettings.masterVolume = m_currentMasterVolume;

        // Save the current settings to file
        bool hr = SettingsConfig.SaveToDisk(GameApp.CurrentSettings);
        if (hr)
        {
            if (AlertBoxController.instance != null)
            {
                AlertBoxController.instance.basicAlertBox.Present("Settings saved successfully.", "Information", () =>
                {
                    if (onSettingsApplied != null)
                        onSettingsApplied.Invoke();
                });
            }
        }
        else
        {
            AlertBoxController.instance.basicAlertBox.Present("Unable to save settings. An unknown error occurred.", "Error", () =>
            {
                if (onSettingsApplied != null)
                    onSettingsApplied.Invoke();
            });
        }
    }


    public void Open()
    {
        Show();

        // Get all values from the current settings and set to the temporary vars
        if (GameApp.CurrentSettings == null)
            return;

        // Set the screen reso dropdown
        m_currentScreenResoIdx = GetIndexFromScreenReso(GameApp.CurrentSettings.screenResolution);
        m_screenResoDropdown.value = m_currentScreenResoIdx;

        // Set the fullscreen toggle
        m_currentFullscreenState = GameApp.CurrentSettings.fullscreen;
        m_fullscreenToggle.isOn = m_currentFullscreenState;

        // Set master volume slider
        m_currentMasterVolume = GameApp.CurrentSettings.masterVolume;
        m_masterVolumeSlider.value = GameApp.CurrentSettings.masterVolume;
    }


    public void Close()
    {
        Hide();

        if (GameApp.CurrentSettings == null)
            return;

        int val = GetIndexFromScreenReso(GameApp.CurrentSettings.screenResolution);
        if (val > -1)
            m_currentScreenResoIdx = val;

        m_currentFullscreenState = GameApp.CurrentSettings.fullscreen;

        m_currentMasterVolume = GameApp.CurrentSettings.masterVolume;
        AudioListener.volume = m_currentMasterVolume;
    }


    private int GetIndexFromScreenReso(Resolution reso)
    {
        if (m_supportedResos != null)
        {
            for (int i = 0; i < m_supportedResos.Length; i++)
            {
                if (m_supportedResos[i].Equals(reso))
                    return i;
            }
        }

        return -1;
    }


    private void UpdateCurrentScreenResoSelection(int newIdx)
    {
        m_currentScreenResoIdx = newIdx;
    }


    private void UpdateWindowModeSelection(bool value)
    {
        m_currentFullscreenState = value;
    }


    private void UpdateMasterVolSelection(float newValue)
    {
        m_currentMasterVolume = newValue;
        AudioListener.volume = m_currentMasterVolume;
    }
}
