// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfilesWindow : UIWidget
{
    [SerializeField]
    private UIPlayerProfileItem m_itemPrefab = null;
    [SerializeField]
    private RectTransform m_content = null;
    [SerializeField]
    private Button m_loginButton = null;
    private List<PlayerProfile> m_pProfiles = null;     // pointer to profiles
    private List<UIPlayerProfileItem> m_items = new List<UIPlayerProfileItem>();

    public UIPlayerProfileItem current { get; set; }


    private void Update()
    {
        m_loginButton.interactable = (current != null);
    }


    public void SetList(List<PlayerProfile> profiles)
    {
        if (m_pProfiles != profiles)
        {
            m_pProfiles = profiles;
            DestroyAllItems();
            if (m_pProfiles != null)
            {
                for (int i = 0; i < m_pProfiles.Count; i++)
                {
                    if (m_content == null)
                    {
                        Debug.LogWarning("Cannot create instances of UIPlayerProfileitem. No content transform set");
                        break;
                    }

                    UIPlayerProfileItem newItem = Instantiate(m_itemPrefab);
                    newItem.owner = this;
                    newItem.SetProfile(m_pProfiles[i]);
                    newItem.gameObject.SetActive(true);
                    newItem.transform.SetParent(m_content, false);
                }
            }
        }
    }


    private void DestroyAllItems()
    {
        for (int i = 0; i < m_items.Count; i++)
        {
            UIPlayerProfileItem item = m_items[i];
            if (item == null)
                continue;

            Destroy(item.gameObject);
        }

        m_items.Clear();
    }
}
