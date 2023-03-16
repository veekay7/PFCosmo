// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// 吹き出し (fu-ki-da-shi) is a Speech Balloon
// Author: VinTK
using UnityEngine;
using TMPro;

public class PlatformInfoFukidashi : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_kanaRenderer = null;
    [SerializeField]
    private TextMeshPro m_textMeshComp = null;
    public bool hideValue = false;


    private void Awake()
    {
        Hide();
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


    public void SetKana(Sprite kanaSprite)
    {
        m_kanaRenderer.sprite = kanaSprite;
    }


    public void SetValue(int value)
    {
        if (value == 0)
        {
            m_textMeshComp.SetText("0");
        }
        else
        {
            string str = hideValue ? "?" : value.ToString();
            m_textMeshComp.SetText(str);
        }
    }


    public void Clear()
    {
        if (m_textMeshComp != null)
            m_textMeshComp.SetText(string.Empty);
        else
            m_kanaRenderer.sprite = null;
    }
}
