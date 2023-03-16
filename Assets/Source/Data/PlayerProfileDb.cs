// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class PlayerProfileDb : ScriptableObject
{
    private bool m_isInit = false;

    [SerializeField, ReadOnly]
    private List<PlayerProfile> m_profiles = new List<PlayerProfile>();


    public void Init()
    {
        if (m_isInit)
            return;

        // First check to see if the path exists
        bool exists = Directory.Exists(GameApp.PlayerProfilesPath);
        if (!exists)
            Directory.CreateDirectory(GameApp.PlayerProfilesPath);    // Create this directory if it does not exist

        // Now we will try to get all the directories inside this path
        var directories = Directory.GetDirectories(GameApp.PlayerProfilesPath);
        foreach (var dir in directories)
        {
            // Go into that new dir and get the profile
            DirectoryInfo current = new DirectoryInfo(dir);
            string currentDirName = current.Name;

            // Construct the new filename string
            string fileName = string.Empty;
            if (currentDirName.Contains("shiken"))
                fileName = dir + @"\profile.json";
            else if (currentDirName.Contains("dev"))
                continue;

            bool fileExists = File.Exists(fileName);
            if (fileExists)
            {
                // Deserialize the file and store to the database
                PlayerProfile profile = PlayerProfile.LoadFromDisk(fileName);
                m_profiles.Add(profile);
            }
        }

        m_isInit = true;
    }


    /// <summary>
    /// Adds a new profile to the list of profiles.
    /// Returns true if profile has been created successfully.
    /// Returns false if the entered username and email exists.
    /// </summary>
    public bool Create(string username, string email)
    {
        Debug.Assert(!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(email), "Either name or email is empty");

        PlayerProfile newProfile = new PlayerProfile(PlayerProfileType.SHIKEN, Guid.NewGuid().ToString(), username, email);

        // Since this profile is new, we need to create a new directory for it
        string profileFolderPath = GameApp.PlayerProfilesPath + "shiken_" + newProfile.email + @"\";
        bool profileFolderPathExists = Directory.Exists(profileFolderPath);
        if (!profileFolderPathExists)
            Directory.CreateDirectory(profileFolderPath);
        else
            return false;

        string fileName = profileFolderPath + "profile.json";
        newProfile.folderPath = profileFolderPath;
        PlayerProfile.SaveToDisk(fileName, newProfile);
        m_profiles.Add(newProfile);

        return true;
    }


    public PlayerProfile Get(string name, string email)
    {
        Debug.Assert(!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(name), "Either name or email is null");

        PlayerProfile output = null;
        foreach (PlayerProfile profile in m_profiles)
        {
            if (profile.username.Equals(name) && profile.email.Equals(email))
            {
                output = profile;
                break;
            }
        }

        return output;
    }


    public void Delete(PlayerProfile profile)
    {
        //if (!string.IsNullOrEmpty(profile.path))
        //    File.Delete(profile.path);

        Directory.Delete(GameApp.PlayerProfilesPath + profile.username);
        m_profiles.Remove(profile);
    }


    public List<PlayerProfile> GetProfiles()
    {
        return m_profiles;
    }


    private void OnDisable()
    {
        m_isInit = false;
    }
}
