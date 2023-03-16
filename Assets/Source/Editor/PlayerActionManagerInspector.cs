// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using PF.Actions;

[CustomEditor(typeof(PlayerActionManager), true)]
public class PlayerActionManagerInspector : Editor
{
    private ReorderableList m_actionsList;
    private PlayerActionManager behaviour => target as PlayerActionManager;


    private void OnEnable()
    {
        m_actionsList = new ReorderableList(behaviour.actionDataList, typeof(LevelActionData), true, true, true, true);
        m_actionsList.onAddCallback += AddItem;
        m_actionsList.onRemoveCallback += RemoveItem;
        m_actionsList.drawHeaderCallback += DrawListHeader;
        m_actionsList.drawElementCallback += DrawListElement;
        m_actionsList.elementHeightCallback += ElementHeight;
        m_actionsList.onReorderCallback += OnActionListReorder;
    }


    private void OnActionListReorder(ReorderableList list)
    {
        // For some fucking weird reason by Unity, we have to use
        // UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenes
        Undo.RecordObject(behaviour, "Player Action Manager List Reordered");
        EditorUtility.SetDirty(behaviour);
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }


    private void AddItem(ReorderableList list)
    {
        Undo.RecordObject(behaviour, "Player Action Manager List Added");
        behaviour.actionDataList.Add(new LevelActionData());
        EditorUtility.SetDirty(behaviour);
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }


    private void RemoveItem(ReorderableList list)
    {
        Undo.RecordObject(behaviour, "Player Action Manager List Removed");
        behaviour.actionDataList.RemoveAt(list.index);
        EditorUtility.SetDirty(behaviour);
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }


    private void DrawListElement(Rect rect, int index, bool active, bool focused)
    {
        LevelActionData item = (LevelActionData)m_actionsList.list[index];

        EditorGUI.BeginChangeCheck();
        string printName = (item.action == null) ? "Action" : item.action.printName;
        EditorGUI.LabelField(
            new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
            printName);

        float accumHeight = 0.0f;
        float height = 0.0f;

        // Action reference
        height = EditorGUI.GetPropertyHeight(SerializedPropertyType.ObjectReference, GUIContent.none);
        accumHeight += height;
        item.action = (Act_Base)EditorGUI.ObjectField(
            new Rect(rect.x, rect.y + accumHeight, rect.width, height), "Action", item.action, typeof(Act_Base), false);

        // Max Use Count
        height = EditorGUI.GetPropertyHeight(SerializedPropertyType.Integer, GUIContent.none);
        accumHeight += height;
        Rect r = new Rect(rect.x, accumHeight + rect.y, rect.width, height);
        item.maxUseCount = EditorGUI.IntField(r, "Max Use Count", item.maxUseCount);

        rect.y += EditorGUIUtility.singleLineHeight;

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(behaviour);
        }
    }


    private float ElementHeight(int index)
    {
        Repaint();

        float height = EditorGUIUtility.singleLineHeight;
        height += EditorGUI.GetPropertyHeight(SerializedPropertyType.ObjectReference, GUIContent.none);
        height += EditorGUI.GetPropertyHeight(SerializedPropertyType.Integer, GUIContent.none);
        height += EditorGUIUtility.singleLineHeight;
        return height;
    }


    private void DrawListHeader(Rect rect)
    {
        GUI.Label(rect, "Action Data List", EditorStyles.boldLabel);
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Needed for free good editor functionality
        serializedObject.Update();

        // Actually draw the list in the inspector
        if (m_actionsList != null)
            m_actionsList.DoLayoutList();

        if (GUI.changed)
        {
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        // Needed for zero-hassle good editor functionality
        serializedObject.ApplyModifiedProperties();
    }


    private void OnDisable()
    {
        m_actionsList.drawHeaderCallback -= DrawListHeader;
        m_actionsList.onAddCallback -= AddItem;
        m_actionsList.onRemoveCallback -= RemoveItem;
        m_actionsList.drawElementCallback -= DrawListElement;
        m_actionsList.elementHeightCallback -= ElementHeight;
        m_actionsList.onReorderCallback -= OnActionListReorder;
        //m_actionsList.drawElementBackgroundCallback -= DrawElementBg;
    }
}
