// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
// References:
// https://answers.unity.com/questions/826062/re-orderable-object-lists-in-inspector.html
// https://answers.unity.com/questions/130477/custom-editor-lack-of-polymorphism.html
// https://gist.github.com/JesseHamburger/3d17f3892e671a4ae17873ec4e8a0926
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(ActionBehaviour), true)]
public class CharaActionBehaviourInspector : Editor
{
    private ReorderableList m_actionsList;
    private ActionBehaviour behaviour => target as ActionBehaviour;
    private string[] m_charaActionDataVars = { "printName", "icon", "description", "canUseAction", "maxUseCount", "ActionListener" };
    private List<bool> m_actionsElementActive = new List<bool>();


    private void OnEnable()
    {
        SerializedProperty actionsListProperty = serializedObject.FindProperty("actions");
        m_actionsList = new ReorderableList(serializedObject, actionsListProperty, true, true, true, true);
        m_actionsList.drawHeaderCallback += DrawListHeader;
        m_actionsList.onAddCallback += AddItem;
        m_actionsList.onRemoveCallback += RemoveItem;
        m_actionsList.drawElementCallback += DrawListElement;
        m_actionsList.elementHeightCallback += ElementHeight;
        //m_actionsList.drawElementBackgroundCallback += DrawElementBg;

        m_actionsElementActive = new List<bool>();
        for(int i = 0; i < actionsListProperty.arraySize; i++)
        {
            m_actionsElementActive.Add(false);
        }
    }


    private void DrawListElement(Rect rect, int index, bool active, bool focused)
    {
        SerializedProperty element = m_actionsList.serializedProperty.GetArrayElementAtIndex(index);
        EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("printName").stringValue);

        rect.y += EditorGUIUtility.singleLineHeight;
        m_actionsElementActive[index] = active;

        if (active)
        {
            float oldHeight = 0.0f;
            for (int i = 0; i < m_charaActionDataVars.Length; i++)
            {
                string propName = m_charaActionDataVars[i];
                SerializedProperty prop = element.FindPropertyRelative(propName);

                float height = EditorGUI.GetPropertyHeight(prop);
                Rect r = new Rect(rect.x, i * oldHeight + rect.y, rect.width, height);
                GUIContent content = new GUIContent();
                content.text = propName;

                EditorGUI.PropertyField(r, prop, content);

                oldHeight = height;
            }
        }
    }


    private float ElementHeight(int index)
    {
        Repaint();

        SerializedProperty element = m_actionsList.serializedProperty.GetArrayElementAtIndex(index);
        float height = 0.0f;

        if (m_actionsElementActive[index] == true)
        {
            for (int i = 0; i < m_charaActionDataVars.Length; i++)
            {
                // Let's get the cumulative height of all elements
                SerializedProperty prop = element.FindPropertyRelative(m_charaActionDataVars[i]);
                height += EditorGUI.GetPropertyHeight(prop);
            }
        }
        else
        {
            height = EditorGUIUtility.singleLineHeight;
        }

        height += EditorGUIUtility.singleLineHeight;

        return height;
    }


    private void DrawElementBg(Rect rect, int index, bool active, bool focused)
    {
        if (index < 0)
            return;

        SerializedProperty element = m_actionsList.serializedProperty.GetArrayElementAtIndex(index);
        float height = 0.0f;
        for (int i = 0; i < m_charaActionDataVars.Length; i++)
        {
            // Let's get the cumulative height of all elements
            SerializedProperty prop = element.FindPropertyRelative(m_charaActionDataVars[i]);
            height += EditorGUI.GetPropertyHeight(prop);
        }

        rect.height = height;

        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, new Color(0.1f, 0.33f, 1f, 0.33f));
        tex.Apply();
        if (active)
        {
            GUI.DrawTexture(rect, tex as Texture);
        }
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Needed for free good editor functionality
        serializedObject.Update();

        // Actually draw the list in the inspector
        if (m_actionsList != null)
            m_actionsList.DoLayoutList();

        // Needed for zero-hassle good editor functionality
        serializedObject.ApplyModifiedProperties();
    }


    private void DrawListHeader(Rect rect)
    {
        GUI.Label(rect, "Actions", EditorStyles.boldLabel);
    }


    private void AddItem(ReorderableList list)
    {
        PF.ActionData action = new PF.ActionData();
        action.printName = "Action";
        behaviour.actions.Add(action);
        m_actionsElementActive.Add(false);
        EditorUtility.SetDirty(behaviour);
    }


    private void RemoveItem(ReorderableList list)
    {
        m_actionsElementActive.RemoveAt(list.index);
        behaviour.actions.RemoveAt(list.index);
        EditorUtility.SetDirty(behaviour);
    }


    private void OnDisable()
    {
        m_actionsList.drawHeaderCallback -= DrawListHeader;
        m_actionsList.onAddCallback -= AddItem;
        m_actionsList.onRemoveCallback -= RemoveItem;
        m_actionsList.drawElementCallback -= DrawListElement;
        m_actionsList.elementHeightCallback -= ElementHeight;
        //m_actionsList.drawElementBackgroundCallback -= DrawElementBg;
    }
}
