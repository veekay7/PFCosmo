using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_textComponent = null;


    private void Awake()
    {
        if (m_textComponent == null)
            m_textComponent = GetComponent<TMP_Text>();
    }


    private void Start()
    {
        var linkedListLevel = GameController.instance.levelScript as BaseLinkedListLevel;
        if (linkedListLevel != null)
        {
            m_textComponent.text = linkedListLevel.stageData.instructions;
        }
    }
}
