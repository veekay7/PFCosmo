// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class KanaTableWizard : ScriptableWizard
{
    [SerializeField]
    private string m_tableName = "NewKanaTable";
    [SerializeField]
    private List<Sprite> m_sprites = new List<Sprite>();


    [MenuItem("PF/New Kana Table")]
    static void Init()
    {
        DisplayWizard<KanaTableWizard>("Kana Table Creator Wizard");
    }


    private void OnWizardCreate()
    {
        if (string.IsNullOrEmpty(m_tableName))
        {
            bool hr = EditorUtility.DisplayDialog("Error", "Table name is empty.", "Ok");
            if (hr)
            {
                Init();
                return;
            }
        }

        if (m_sprites.Count == 0)
        {
            bool hr = EditorUtility.DisplayDialog("Error", "No sprites to create table from.", "Ok");
            if (hr)
            {
                Init();
                return;
            }
        }

        KanaTable asset = ScriptableObject.CreateInstance<KanaTable>();
        bool result = asset.CreateFromSprites(m_sprites.ToArray());
        if (result)
        {
            AssetDatabase.CreateAsset(asset, "Assets/" + m_tableName + ".asset");
            EditorUtility.DisplayDialog("Success", "Successfully created " + m_tableName, "Ok");
        }
    }
}
