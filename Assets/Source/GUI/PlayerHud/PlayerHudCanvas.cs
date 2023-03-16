// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;
using UnityEngine.UI;

public class PlayerHudCanvas : MonoBehaviourSingleton<PlayerHudCanvas>
{
    [ReadOnly]
    public Player player;
    public ItemCollection playerStoneCollection;
    public GameState gameStats;

    [Header("UI")]
    [SerializeField]
    private UIActionButtonList m_buttonList = null;
    [SerializeField]
    private UIValueList m_valueList = null;
    [SerializeField]
    private Text m_holdingValue = null;
    [SerializeField]
    private Text m_crystalsValue = null;
    [SerializeField]
    private CanvasGroup m_inputBlock = null;
    [SerializeField]
    private Text m_cosmoSayText = null;

    [Header("Head/Tail Display")]
    [SerializeField]
    private Image m_headKanaImage = null;
    //[SerializeField]
    //private Image m_tailKanaImage = null;
    [SerializeField]
    private Sprite m_defaultKanaSprite = null;

    [Header("Windows")]
    [SerializeField]
    private UIPromptOverlay m_promptOverlayUI = null;
    [SerializeField]
    private UIMessageOverlay m_messageOverlayUI = null;

    public RectTransform rectTransform { get; private set; }
    public Canvas canvas { get; private set; }
    public GraphicRaycaster raycaster { get; private set; }

    public UIActionButtonList buttonList => m_buttonList;
    public UIValueList valueList => m_valueList;
    public Text holdingValueText => m_holdingValue;
    public UIPromptOverlay promptOverlayUI => m_promptOverlayUI;
    public UIMessageOverlay messageOverlayUI => m_messageOverlayUI;


    protected override void OnEnable()
    {
        base.OnEnable();

        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponent<Canvas>();
        raycaster = GetComponent<GraphicRaycaster>();

        EnableInput();
    }


    private void LateUpdate()
    {
        if (player == null)
            return;

        // Update game stats
        if (gameStats.headPlatform != null && gameStats.headPlatform.kana != null)
            SetHeadKanaImage(gameStats.headPlatform.kana.sprite);

        //if (gameStats.tailPlatform != null && gameStats.tailPlatform.kana != null)
        //    SetTailKanaImage(gameStats.tailPlatform.kana.sprite);

        m_crystalsValue.text = player.stoneCollection.collectedAmount.ToString() + " / " + gameStats.totalCrystals.ToString();

        // Update inventory
        int inventoryValue = player.inventory.Get();
        if (inventoryValue != 0)
            m_holdingValue.text = "  " + inventoryValue.ToString();
        else
            m_holdingValue.text = "  .  ";
    }


    public void EnableInput()
    {
        m_inputBlock.alpha = 0.0f;
        m_inputBlock.interactable = false;
        m_inputBlock.blocksRaycasts = false;
    }


    public void DisableInput()
    {
        m_inputBlock.alpha = 1.0f;
        m_inputBlock.interactable = true;
        m_inputBlock.blocksRaycasts = true;
    }


    public void SetHeadKanaImage(Sprite sprite)
    {
        if (sprite == null)
            m_headKanaImage.sprite = m_defaultKanaSprite;
        else
            m_headKanaImage.sprite = sprite;
    }


    public void CosmoSay(string msg)
    {
        m_cosmoSayText.text = msg;
    }

    //public void SetTailKanaImage(Sprite sprite)
    //{
    //    if (sprite == null)
    //        m_tailKanaImage.sprite = m_defaultKanaSprite;
    //    else
    //        m_tailKanaImage.sprite = sprite;
    //}

    public void OnPauseButtonPressed()
    { 
        GameRules.instance.PauseGame();
    }
    public void Show()
    {
        canvas.enabled = true;
    }


    public void Hide()
    {
        canvas.enabled = false;
    }
}
