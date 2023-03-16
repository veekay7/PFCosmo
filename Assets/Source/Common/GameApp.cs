// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using System.IO;
using UnityEngine;

public static class GameApp
{
    public static string GameName = "PFCosmoGame";
    public static bool FirstStart = true;
    public static DirectoryInfo GameDataFolderInfo = null;
    public static PlayerProfile CurrentProfile = null;
    public static string CurrentProfilePath = null;
    public static SettingsConfig CurrentSettings = null;
    private static bool _init = false;


    public static string GameDataPath
    {
        get
        {
            string path = string.Empty;

//#if UNITY_STANDALONE_WIN
            path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", @"\");
            path += @"\" + GameName + @"\";
//#else
//        path = Application.persistentDataPath + @"\GameData\";
//#endif

            return path;
        }
    }

    public static string PlayerProfilesPath => GameApp.GameDataPath + @"profiles\";
    public static string DevPlayerProfilePath => GameApp.PlayerProfilesPath + @"eveeking.json";


    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        if (_init)
            return;

        Debug.Log("Initializing...");

        Physics2D.alwaysShowColliders = false;

        // Check to see if the game has its storage folder created.
        GameDataFolderInfo = CreateGameDataFolder();
        if (GameDataFolderInfo == null)
        {
            Quit();
            return;
        }

        // Try to load settings from the game data folder
        // Once successfully loaded, it will apply its settings to the application.
        CurrentSettings = SettingsConfig.Init();

        _init = true;
    }


    /// <summary>
    /// Creates the game data folder
    /// </summary>
    /// <returns></returns>
    public static DirectoryInfo CreateGameDataFolder()
    {
        DirectoryUtils.CreateDirectory(GameDataPath, out DirectoryInfo info);
        return info;
    }


    /// <summary>
    /// Checks if the game data folder exists or not
    /// </summary>
    /// <returns></returns>
    public static bool DoesGameDataFolderExist()
    {
        DirectoryInfo info = DirectoryUtils.GetDirectoryInfo(GameDataPath);
        return (info != null);
    }


    /// <summary>
    /// Closes the game application!!
    /// </summary>
    public static void Quit()
    {
#if UNITY_EDITOR
        // In editor mode, Application.Quit is ignored. To stop the game,
        // set UnityEditor.EditorApplication.isPlaying to false.
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
