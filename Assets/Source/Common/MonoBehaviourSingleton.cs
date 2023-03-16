// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T m_instance;


    public static T instance
    {
        get { return m_instance; }
    }


    protected virtual void OnEnable()
    {
        if (m_instance == null)
        {
            m_instance = this as T;
        }
        else if (m_instance != this)
        {
            Debug.LogWarning("There can only be one instance of: " + typeof(T).ToString());
            DestroyImmediate(gameObject);
            return;
        }
    }
}
