// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

/// <summary>
/// TinyDataStore static class
/// </summary>
public static class TinyDataStore
{
    /// <summary>
    /// DataStroage internal class
    /// </summary>
    internal class DataStorage
    {
        public List<string> keys = new List<string>();
        public Dictionary<string, int> integerValues = new Dictionary<string, int>();
        public Dictionary<string, float> floatValues = new Dictionary<string, float>();
        public Dictionary<string, string> stringValues = new Dictionary<string, string>();
    }

    private static DataStorage m_dataStorage;

    [JsonIgnore]
    public static string gameAppPath
    {
        get
        {
            string path = Application.persistentDataPath + @"/" + "Data/";
            return path;
        }
    }


    [JsonIgnore]
    public static string saveFilePath
    {
        get
        {
            return gameAppPath + @"/EmbedData.json";
        }
    }


    public static void SetInt(string key, int value)
    {
        if (!m_dataStorage.keys.Contains(key))
        {
            m_dataStorage.keys.Add(key);
            m_dataStorage.integerValues.Add(key, value);
        }

    }


    public static void SetFloat(string key, float value)
    {
        if (!m_dataStorage.keys.Contains(key))
        {
            m_dataStorage.keys.Add(key);
            m_dataStorage.floatValues.Add(key, value);
        }
    }


    public static void SetString(string key, string value)
    {
        if (!m_dataStorage.keys.Contains(key))
        {
            m_dataStorage.keys.Add(key);
            m_dataStorage.stringValues.Add(key, value);
        }
    }


    public static bool HasKey(string key)
    {
        bool hr = m_dataStorage.keys.Contains(key);
        return hr;
    }


    public static int GetInt(string key)
    {
        if (!HasKey(key))
            throw new UnityException("No such key exists");
        return m_dataStorage.integerValues[key];
    }


    public static float GetFloat(string key)
    {
        if (!HasKey(key))
            throw new UnityException("No such key exists");
        return m_dataStorage.floatValues[key];
    }


    public static string GetString(string key)
    {
        if (!HasKey(key))
            throw new UnityException("No such key exists");
        return m_dataStorage.stringValues[key];
    }


    public static void Save()
    {
        string json = JsonConvert.SerializeObject(m_dataStorage);
        File.WriteAllText(saveFilePath, json);
    }


    public static void LoadFromFile()
    {
        // Check does path exist?
        // If it does not, create a new save.
        // If it does, deserialize.
        bool hr = Directory.Exists(saveFilePath);
        if (!hr)
        {
            Save();
        }
        else
        {
            m_dataStorage = null;
            string json = File.ReadAllText(saveFilePath);
            m_dataStorage = JsonConvert.DeserializeObject<DataStorage>(json);
        }
    }


    public static void Delete(string key)
    {
        if (HasKey(key))
        {
            // Check integer Dictionary
            foreach (KeyValuePair<string, int> i in m_dataStorage.integerValues)
            {
                if (i.Key == key)
                {
                    m_dataStorage.integerValues.Remove(i.Key);
                    m_dataStorage.keys.Remove(key);
                    break;
                }
            }

            // Check float Dictionary
            foreach (KeyValuePair<string, float> f in m_dataStorage.floatValues)
            {
                if (f.Key == key)
                {
                    m_dataStorage.floatValues.Remove(f.Key);
                    m_dataStorage.keys.Remove(key);
                    break;
                }
            }


            // Check string Dictionary
            foreach (KeyValuePair<string, string> s in m_dataStorage.stringValues)
            {
                if (s.Key == key)
                {
                    m_dataStorage.stringValues.Remove(s.Key);
                    m_dataStorage.keys.Remove(key);
                    break;
                }
            }
        }
    }


    public static void DeleteAll()
    {
        m_dataStorage.keys.Clear();
        m_dataStorage.integerValues.Clear();
        m_dataStorage.floatValues.Clear();
        m_dataStorage.stringValues.Clear();
    }
}
