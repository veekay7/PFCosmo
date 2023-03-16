// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// 吹き出し (fu-ki-da-shi) is a Speech Balloon
// Author: VinTK
using UnityEngine;
using TMPro;

public class Fukidashi : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro m_textMeshComp = null;
    [SerializeField]
    private string m_text = "Text";


    private void Awake()
    {
        Hide();
    }


    private void LateUpdate()
    {
        m_textMeshComp.text = m_text;
    }


    [ContextMenu("Show")]
    public void Show()
    {
        gameObject.SetActive(true);
    }


    [ContextMenu("Hide")]
    public void Hide()
    {
        gameObject.SetActive(false);
    }


    public void SetText(string text)
    {
        m_text = text;
    }
}
