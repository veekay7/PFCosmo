// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIValueList : MonoBehaviour
{
    [SerializeField]
    private Text m_textComponent = null;
    private List<int> m_values;


    private void Update()
    {
        if (m_values == null)
            return;

        int len = m_values.Count;
        string msg = string.Empty;
        msg += "[ ";
        for (int i = 0; i < len; i++)
        {
            if (m_values[i] == 0)
                msg += " . ";
            else
                msg += m_values[i].ToString();

            if (i == len - 1)
                break;
            else
                msg += " , ";
        }
        msg += " ]";
        m_textComponent.text = msg;
    }


    public void SetValues(List<int> values)
    {
        m_values = values;
    }
}
