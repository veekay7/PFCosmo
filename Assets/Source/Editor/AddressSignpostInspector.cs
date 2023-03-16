// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AddressSignpost))]
public class AddressSignpostInspector : Editor
{
    private int m_selectedSignType = 0;
    private string m_newSignAddressStr = "";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AddressSignpost actor = target as AddressSignpost;

        string[] options = { "Basic", "Arrow" };
        m_selectedSignType = EditorGUILayout.Popup("Sign Type: ", m_selectedSignType, options);
        m_newSignAddressStr = EditorGUILayout.TextField("Address: ", m_newSignAddressStr);

        if (GUILayout.Button("Add Sign"))
        {
            if (string.IsNullOrEmpty(m_newSignAddressStr) || string.IsNullOrWhiteSpace(m_newSignAddressStr))
            {
                Debug.LogWarning("No address entered. Please ensure that address is entered into the text field before adding.");
                return;
            }

            Undo.RecordObject(actor, "Add Sign");
            actor.AddSign(m_selectedSignType, m_newSignAddressStr);
            EditorUtility.SetDirty(actor);
        }

        if (GUILayout.Button("Update Sign Graphics"))
        {
            Undo.RecordObject(actor, "Update Sign Graphics");
            actor.UpdateSignGraphics();
            EditorUtility.SetDirty(actor);
        }

        if (GUILayout.Button("Clear Signs"))
        {
            Undo.RecordObject(actor, "Clear Signs");
            actor.ClearSigns();
            EditorUtility.SetDirty(actor);
        }
    }
}
