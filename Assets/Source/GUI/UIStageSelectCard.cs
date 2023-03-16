// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;
using UnityEngine.UI;

public class UIStageSelectCard : MonoBehaviour
{
    [SerializeField]
    private Image m_thumbnail = null;
    [SerializeField]
    private Text m_printNameText = null;
    [SerializeField]
    private Sprite m_defaultThumbnail = null;
    [SerializeField]
    private StageData m_stageData = null;

    private Button m_buttonComponent;


    private void OnEnable()
    {
        m_buttonComponent = GetComponent<Button>();
        m_buttonComponent.onClick.AddListener(() =>
        {
            if (m_stageData != null &&
            GameController.instance != null &&
            GameController.instance.levelScript != null)
            {
                StageSelectLevel level = GameController.instance.levelScript as StageSelectLevel;
                if (level != null)
                {
                    level.LoadStage(m_stageData);
                }
            }
        });
    }


    private void OnValidate()
    {
        if (m_stageData != null)
        {
            this.name = "StageSelectCard_" + m_stageData.sceneName;
            if (m_stageData.thumbnail == null)
                m_thumbnail.sprite = m_defaultThumbnail;
            else
                m_thumbnail.sprite = m_stageData.thumbnail;
            m_printNameText.text = m_stageData.printName;
        }
        else
        {
            m_printNameText.text = "StageSelectCard";
            m_thumbnail.sprite = m_defaultThumbnail;
            this.name = "StageSelectCard";

        }
    }
}
