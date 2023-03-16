// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System;

public class SettingsConfig
{
    private Resolution m_screenReso;
    private bool m_fullscreen;
    private float m_masterVolume;


    public Resolution screenResolution
    {
        get { return m_screenReso; }
        set
        {
            m_screenReso = value;
            Screen.SetResolution(m_screenReso.width, m_screenReso.height, m_fullscreen, m_screenReso.refreshRate);
        }
    }

    public bool fullscreen
    {
        get { return m_fullscreen; }
        set
        {
            m_fullscreen = value;
            Screen.fullScreen = m_fullscreen;
        }
    }

    public float masterVolume
    {
        get { return m_masterVolume; }
        set
        {
            m_masterVolume = value;
            AudioListener.volume = m_masterVolume;
        }
    }


    public SettingsConfig()
    {
        screenResolution = DefaultScreenResolution();
        fullscreen = true;
        masterVolume = 0.75f;
    }


    public static string Path
    {
        get
        {
            return GameApp.GameDataPath + "settings_config.json";
        }
    }


    /// <summary>
    /// 0 - SettingsConfig file Exists
    /// </summary>
    /// <returns></returns>
    public static SettingsConfig Init()
    {
        SettingsConfig inst = null;
        bool hr = GameApp.DoesGameDataFolderExist();
        if (!hr)
        {
            Debug.LogWarning("Game Data folder does not exist! Make a new default one to store in memory.");
            inst = new SettingsConfig();
        }
        else
        {
            // If folder exists, check to see if the file can be found
            hr = File.Exists(Path);
            if (!hr)
            {
                // File cannot be found, try to create
                inst = SettingsConfig.CreateNewSettingsConfig();
            }
            else
            {
                // File can be found. Load it from disk.
                inst = SettingsConfig.LoadFromDisk();
            }
        }

        return inst;
    }


    public static bool SaveToDisk(SettingsConfig config)
    {
        try
        {
            using (StreamWriter file = File.CreateText(Path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, config);
            }
        }
        catch (Exception ex)
        {
            NativeWin32Alert.Error(ex.Message, "Critical Error");
            return false;
        }

        return true;

        //string configJsonTxt = JsonConvert.SerializeObject(config);
        //File.WriteAllText(Path, configJsonTxt);
    }


    public static SettingsConfig LoadFromDisk()
    {
        string configTxt = File.ReadAllText(Path);
        SettingsConfig output = JsonConvert.DeserializeObject<SettingsConfig>(configTxt);
        return output;
    }


    public static SettingsConfig CreateNewSettingsConfig()
    {
        SettingsConfig inst = new SettingsConfig();
        SettingsConfig.SaveToDisk(inst);

        // Return the newly created settings and do naughty things with it
        return inst;
    }


    private Resolution DefaultScreenResolution()
    {
        Resolution reso = new Resolution();
        reso.width = 1280;
        reso.height = 720;
        reso.refreshRate = 60;
        return reso;
    }
}
