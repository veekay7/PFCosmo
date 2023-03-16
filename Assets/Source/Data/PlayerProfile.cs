// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public enum PlayerProfileType
{
    DEV,        // Developer
    SHIKEN      // For experiment
};

public class PlayerProfile
{
    public PlayerProfileType type;
    public string id;
    public string username;
    public string email;
    public string folderPath;


    public PlayerProfile()
    {
        type = PlayerProfileType.DEV;
        id = string.Empty;
        username = string.Empty;
        email = string.Empty;
        folderPath = string.Empty;
    }

    public PlayerProfile(PlayerProfileType type, string id, string username, string email)
    {
        this.type = type;
        this.id = id;
        this.username = username;
        this.email = email;
        this.folderPath = string.Empty;
    }


    public static PlayerProfile LoadFromDisk(string path)
    {
        string profileTxt = File.ReadAllText(path);
        PlayerProfile output = JsonConvert.DeserializeObject<PlayerProfile>(profileTxt);
        return output;
    }


    public static void SaveToDisk(string path, PlayerProfile profile)
    {
        string profileJsonTxt = JsonConvert.SerializeObject(profile);
        File.WriteAllText(path, profileJsonTxt);
    }


    #region Developer

    /// <summary>
    /// Checks to see if a developer profile exists
    /// </summary>
    public static bool DoesDevProfileExist()
    {
        // If folder exists, check to see if the file can be found
        string path = GameApp.DevPlayerProfilePath;
        bool hr = File.Exists(path);
        return hr;
    }


    /// <summary>
    /// Creates a new developer profile
    /// </summary>
    /// <returns></returns>
    public static PlayerProfile CreateDevProfile()
    {
        string guid = Guid.NewGuid().ToString();

        PlayerProfile inst = new PlayerProfile();
        inst.type = PlayerProfileType.DEV;
        inst.id = guid;
        inst.username = "dev";
        inst.email = string.Empty;

        // Save the new instance to the disk
        PlayerProfile.SaveToDisk(GameApp.DevPlayerProfilePath, inst);

        // Return the newly created local profile to do naughty things with it
        return inst;
    }


    /// <summary>
    /// Loads a developer profile from disk
    /// </summary>
    /// <returns></returns>
    public static PlayerProfile LoadDevProfileFromDisk()
    {
        string profileTxt = File.ReadAllText(GameApp.DevPlayerProfilePath);
        PlayerProfile output = JsonConvert.DeserializeObject<PlayerProfile>(profileTxt);
        return output;
    }

    #endregion
}
