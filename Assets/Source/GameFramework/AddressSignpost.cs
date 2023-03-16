// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// SignAddress class
/// </summary>
[Serializable]
public class SignAddress
{
    public string address;
    public GameObject parent { get; private set; }
    public Text textComponent { get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="parent"></param>
    public SignAddress(GameObject parent)
    {
        this.parent = parent;
        this.textComponent = this.parent.GetComponentInChildren<Text>();
    }


    /// <summary>
    /// Sets a new address to this sign address
    /// </summary>
    /// <param name="newAddress"></param>
    public void SetAddressText(string newAddress)
    {
        address = newAddress;
    }


    /// <summary>
    /// Updates the text component stored in the sign address game object
    /// </summary>
    public void UpdateTextComponent()
    {
        if (textComponent == null && parent != null)
        {
            textComponent = parent.GetComponentInChildren<Text>();
        }

        textComponent.text = address;
    }
}


/// <summary>
/// Address Signpost class
/// </summary>
public class AddressSignpost : MonoBehaviour
{
    public GameObject signPrefab;
    public GameObject signArrowPrefab;

    private const string m_signNamePrefix = "AddressSign_";
    [SerializeField]
    private List<SignAddress> m_signs = new List<SignAddress>();


    public void AddSign(int signType, string addressString)
    {
        if (signPrefab == null || signArrowPrefab == null)
        {
            Debug.LogError("Prefabs are null.");
            return;
        }

        GameObject selectedPrefab = null;
        switch (signType)
        {
            case 0:
                selectedPrefab = signPrefab;
                break;

            case 1:
                selectedPrefab = signArrowPrefab;
                break;
        }

        // Instantiate the sign game object first
        GameObject newSignObject = Instantiate(selectedPrefab);
        newSignObject.transform.SetParent(transform, false);
        newSignObject.SetActive(true);
        newSignObject.name = m_signNamePrefix + addressString;

        SignAddress newSign = new SignAddress(newSignObject);
        newSign.SetAddressText(addressString);
        newSign.UpdateTextComponent();
        m_signs.Add(newSign);
    }


    public void UpdateSignGraphics()
    {
        foreach (SignAddress sign in m_signs)
        {
            sign.UpdateTextComponent();
        }
    }


    public void ClearSigns()
    {
        foreach (SignAddress sign in m_signs)
        {
            DestroyImmediate(sign.parent);
        }

        m_signs.Clear();
    }
}
