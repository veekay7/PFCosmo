// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(Puzzle), true)]
public class PuzzleInspector : Editor
{
    public Puzzle puzzle => target as Puzzle;


    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        base.OnInspectorGUI();

        GUIStyle labelStyle = new GUIStyle();
        labelStyle.fontStyle = FontStyle.Bold;

        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("Slots", labelStyle);
        EditorGUILayout.BeginHorizontal();
        bool btnAddSlot = GUILayout.Button("Create");
        bool btnDelLastSlot = GUILayout.Button("Del Last");
        bool btnDelAllSlots = GUILayout.Button("Del All");
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        bool btnInvalidateAllSlots = GUILayout.Button("Invalidate All");
        EditorGUILayout.EndHorizontal();

        if (btnAddSlot)
        {
            Undo.RecordObject(puzzle.slotsCtrl, "Puzzle Slots Add New");
            puzzle.slotsCtrl.CreateSlot();
            EditorUtility.SetDirty(puzzle.slotsCtrl);
        }

        if (btnDelLastSlot)
        {
            Undo.RecordObject(puzzle.slotsCtrl, "Puzzle Slots Deleted Last");
            puzzle.slotsCtrl.DeleteLastSlot();
            EditorUtility.SetDirty(puzzle.slotsCtrl);
        }

        if (btnDelAllSlots)
        {
            Undo.RecordObject(puzzle.slotsCtrl, "Puzzle Slots Cleared All");
            puzzle.slotsCtrl.DeleteAll();
            EditorUtility.SetDirty(puzzle.slotsCtrl);
        }

        if (btnInvalidateAllSlots)
        {
            Undo.RecordObject(puzzle.slotsCtrl, "Puzzle Slots Invalidated All");
            puzzle.slotsCtrl.InvalidateSlots();
            EditorUtility.SetDirty(puzzle.slotsCtrl);
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Platform Pool", labelStyle);
        EditorGUILayout.BeginHorizontal();
        bool btnAddPlatformPool = GUILayout.Button("Add New");
        bool btnDelLastPlatformPool = GUILayout.Button("Del Last");
        bool btnDelPlatformPool = GUILayout.Button("Del All");
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        bool btnInvalidatePlatforms = GUILayout.Button("Invalidate All");
        EditorGUILayout.EndHorizontal();

        if (btnAddPlatformPool)
        {
            Undo.RecordObject(puzzle.platformMgmt, "Puzzle Platform Pool Added");
            puzzle.platformMgmt.CreatePlatform();
            EditorUtility.SetDirty(puzzle.platformMgmt);
        }

        if (btnDelLastPlatformPool)
        {
            Undo.RecordObject(puzzle.platformMgmt, "Puzzle Platform Pool Delete Last");
            puzzle.platformMgmt.DelLastPlatform();
            EditorUtility.SetDirty(puzzle.platformMgmt);
        }

        if (btnDelPlatformPool)
        {
            Undo.RecordObject(puzzle.platformMgmt, "Puzzle Platform Pool Delete All");
            puzzle.platformMgmt.DeletePool();
            EditorUtility.SetDirty(puzzle.platformMgmt);
        }

        if (btnInvalidatePlatforms)
        {
            Undo.RecordObject(puzzle.platformMgmt, "Puzzle Platform Pool Invalidate All");
            puzzle.platformMgmt.InvalidatePlatforms();

            EditorUtility.SetDirty(puzzle.platformMgmt);
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Platform Ctrl", labelStyle);
        EditorGUILayout.BeginHorizontal();
        bool btnAddNodeFirst = GUILayout.Button("Add Node First");
        bool btnAddNodeLast = GUILayout.Button("Add Node Last");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        bool btnClear = GUILayout.Button("Clear");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        bool result = false;
        if (btnAddNodeFirst || btnAddNodeLast || btnClear)
        {
            string errorMsg = string.Empty;
            result = AreThereErrors(out errorMsg);
            if (result)
            {
                if (EditorUtility.DisplayDialog("Error", errorMsg, "Ok"))
                    return;
            }
        }

        if (btnAddNodeFirst)
        {
            // NOTE: Must use Undo.RecordObject(), then call edit function, then EditorUtility.SetDirty() (for non-scene objects like serializables)
            // Otherwise, the script that you are editing will not save its changed. Like WTF right?
            // こんなのクソでしょう？ うんこ 草wwwwww
            Undo.RecordObject(puzzle, "Puzzle Add First");
            puzzle.AddFirst(0);
            EditorUtility.SetDirty(puzzle);
        }

        if (btnAddNodeLast)
        {
            Undo.RecordObject(puzzle, "Puzzle Add Last");
            puzzle.AddLast(0);
            EditorUtility.SetDirty(puzzle);
        }

        if (btnClear)
        {
            Undo.RecordObject(puzzle, "Puzzle Cleared");
            puzzle.Clear();
            EditorUtility.SetDirty(puzzle);
        }

        // これは魔法かな？
        // If the scene still doesn't change, use this! Fuck Unity sometimes man, being inconsistent with their docs!
        //if (GUI.changed)
        //{
        //    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        //}

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }


    private bool AreThereErrors(out string errorMsg)
    {
        string msg = string.Empty;
        if (puzzle.platformMgmt == null)
            msg += "The Puzzle is missing its Platform Manager.\n";
        else if (puzzle.slotsCtrl == null)
            msg += "The Puzzle is missing its Platform Slots Controller.\n";

        errorMsg = msg;
        return !(string.IsNullOrEmpty(msg));
    }
}
