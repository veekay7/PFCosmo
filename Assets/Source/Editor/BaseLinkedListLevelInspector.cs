// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
// Referenced from: https://answers.unity.com/questions/826062/re-orderable-object-lists-in-inspector.html
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(BaseLinkedListLevel), true)]
public class BaseLinkedListLevelInspector : Editor
{
    private ReorderableList m_usableValuesList;
    private BaseLinkedListLevel level => target as BaseLinkedListLevel;


    private void OnEnable()
    {
        m_usableValuesList = new ReorderableList(level.usableValues, typeof(int), true, true, true, true);
        m_usableValuesList.drawHeaderCallback += DrawUsableListHeader;
        m_usableValuesList.drawElementCallback += DrawUsableListElement;
        m_usableValuesList.onAddCallback += AddItem;
        m_usableValuesList.onRemoveCallback += RemoveItem;
        m_usableValuesList.onReorderCallback += OnUsableValuesListReorder;
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Needed for free good editor functionality
        serializedObject.Update();

        // Actually draw the list in the inspector
        if (m_usableValuesList != null)
            m_usableValuesList.DoLayoutList();

        // Needed for zero-hassle good editor functionality
        serializedObject.ApplyModifiedProperties();
    }


    private void OnDisable()
    {
        // Make sure we don't get memory leaks etc.
        m_usableValuesList.drawHeaderCallback -= DrawUsableListHeader;
        m_usableValuesList.drawElementCallback -= DrawUsableListElement;
        m_usableValuesList.onAddCallback -= AddItem;
        m_usableValuesList.onRemoveCallback -= RemoveItem;
        m_usableValuesList.onReorderCallback -= OnUsableValuesListReorder;
    }


    private void AddItem(ReorderableList list)
    {
        if (list == m_usableValuesList)
        {
            if (level.usableValues.Count < Globals.MaxNodesPerPuzzle)
            {
                level.usableValues.Add(0);
                EditorUtility.SetDirty(level);
                Undo.RecordObject(level, "Usable Values Added");
                UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
            }
        }
    }


    private void RemoveItem(ReorderableList list)
    {
        if (list == m_usableValuesList)
        {
            level.usableValues.RemoveAt(list.index);
            Undo.RecordObject(level, "Usable Values Removed");
            EditorUtility.SetDirty(level);
            UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        }
    }


    private void OnUsableValuesListReorder(ReorderableList list)
    {
        EditorUtility.SetDirty(level);
        Undo.RecordObject(level, "Usable Values Reordered");
        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
    }


    private void DrawUsableListHeader(Rect rect)
    {
        GUI.Label(rect, "Usable Values List");
    }


    private void DrawUsableListElement(Rect rect, int index, bool active, bool focused)
    {
        int item = (int)m_usableValuesList.list[index];

        EditorGUI.BeginChangeCheck();
        m_usableValuesList.list[index] = EditorGUI.IntField(new Rect(rect.x + 8, rect.y, rect.width - 8, rect.height), item);
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(level);
        }

        // If you are using a custom PropertyDrawer, this is probably better
        // EditorGUI.PropertyField(rect, serializedObject.FindProperty("list").GetArrayElementAtIndex(index));
        // Although it is probably smart to cach the list as a private variable ;)
    }
}
