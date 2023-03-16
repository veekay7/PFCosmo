// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class GameStats : MonoBehaviourSingleton<GameStats>
{
    private string m_name;
    private Dictionary<string, string> m_stats = new Dictionary<string, string>();


    protected override void OnEnable()
    {
        base.OnEnable();
        DontDestroyOnLoad(gameObject);
    }


    public void StartNew(string playerName)
    {
        string datum = DateTime.Now.ToString("yyyyMMddHHmmss");
        m_name = "stats_" + playerName + "_" + datum;
        ClearAll();
    }


    public void Flush()
    {
        m_name = string.Empty;
        ClearAll();
    }


    public void ClearAll()
    {
        m_stats.Clear();
    }


    public int GetInt(string key)
    {
        bool containsKey = m_stats.ContainsKey(key);
        if (!containsKey)
            throw new UnityException("The key " + key + " does not exist!");

        string strValue = m_stats[key];
        if (string.IsNullOrEmpty(strValue))
            return 0;

        int atoi = Convert.ToInt32(strValue);
        return atoi;
    }


    public float GetFloat(string key)
    {
        bool containsKey = m_stats.ContainsKey(key);
        if (!containsKey)
            throw new UnityException("The key " + key + " does not exist!");

        string strValue = m_stats[key];
        if (string.IsNullOrEmpty(strValue))
            return 0.0f;

        float atof = (float)Convert.ToDouble(strValue);
        return atof;
    }


    public string GetString(string key)
    {
        bool containsKey = m_stats.ContainsKey(key);
        if (!containsKey)
            throw new UnityException("The key " + key + " does not exist!");

        string strValue = m_stats[key];
        return strValue;
    }


    public void SetInt(string key, int value)
    {
        string itoa = value.ToString();
        bool containsKey = m_stats.ContainsKey(key);
        if (containsKey)
        {
            m_stats[key] = itoa;
        }
        else
        {
            m_stats.Add(key, itoa);
        }
    }


    public void SetFloat(string key, float value)
    {
        string ftoa = value.ToString();
        bool containsKey = m_stats.ContainsKey(key);
        if (containsKey)
        {
            m_stats[key] = ftoa;
        }
        else
        {
            m_stats.Add(key, ftoa);
        }
    }


    public void SetString(string key, string str)
    {
        bool containsKey = m_stats.ContainsKey(key);
        if (containsKey)
        {
            m_stats[key] = str;
        }
        else
        {
            m_stats.Add(key, str);
        }
    }


    public Dictionary<string, string> GetStats()
    {
        return m_stats;
    }


    public void Save(string path)
    {
        if (string.IsNullOrEmpty(m_name))
        {
            Debug.LogWarning("Name is empty");
            return;
        }

        string fileName = path + @"\" + m_name + ".json";
        string json = JsonConvert.SerializeObject(m_stats);
        File.WriteAllText(fileName, json);
    }
}
