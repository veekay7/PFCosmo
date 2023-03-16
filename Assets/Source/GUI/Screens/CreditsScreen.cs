// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CreditsScreen : BaseScreen
{
    [SerializeField]
    private List<CreditsScreenPage> m_pages = new List<CreditsScreenPage>();
    [SerializeField]
    public int m_startPageIndex = 0;

    public UnityEvent onEscPressed = new UnityEvent();

    private int m_oldPageIdx;
    private int m_currentPageIdx;

    public bool isTweening { get; private set; }
    public int maxPageCount => (m_pages.Count - 1);


    protected override void Awake()
    {
        base.Awake();

        isTweening = false;
        m_oldPageIdx = 0;
        m_currentPageIdx = 0;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (onEscPressed != null)
                onEscPressed.Invoke();
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Previous();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Next();
        }
    }


    public override void Show()
    {
        base.Show();

        // When this finally shows, if we have pages and have set which page index to show on start, do it
        if (m_pages.Count > 0 && m_startPageIndex < maxPageCount)
        {
            m_currentPageIdx = m_startPageIndex;
            m_pages[m_currentPageIdx].gameObject.SetActive(true);
            m_pages[m_currentPageIdx].Show();
            m_oldPageIdx = m_startPageIndex;
        }
    }


    public override void Hide()
    {
        base.Hide();
    }


    public void Next()
    {
        if (m_pages.Count == 0 || isTweening)
            return;

        m_oldPageIdx = m_currentPageIdx;
        m_currentPageIdx++;
        m_currentPageIdx = Utils.Cycle(m_currentPageIdx, 0, maxPageCount);

        StartCoroutine(Co_GoToNewPage());
    }


    public void Previous()
    {
        if (m_pages.Count == 0 || isTweening)
            return;

        m_oldPageIdx = m_currentPageIdx;
        m_currentPageIdx--;
        m_currentPageIdx = Utils.Cycle(m_currentPageIdx, 0, maxPageCount);

        StartCoroutine(Co_GoToNewPage());
    }


    public IEnumerator Co_GoToNewPage()
    {
        isTweening = true;

        float a = 0.0f;
        while (a < 1.0f)
        {
            m_pages[m_oldPageIdx].canvasGroup.alpha = Mathf.Lerp(1.0f, 0.0f, a);
            a += Time.deltaTime;
            yield return null;
        }
        m_pages[m_oldPageIdx].Hide();

        a = 0.0f;
        while (a < 1.0f)
        {
            m_pages[m_currentPageIdx].canvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, a);
            a += Time.deltaTime;
            yield return null;
        }
        m_pages[m_currentPageIdx].Show();

        isTweening = false;
    }
}
