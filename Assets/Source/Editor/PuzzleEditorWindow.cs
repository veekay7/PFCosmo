// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;
using UnityEditor;

public class PuzzleEditorWindow : EditorWindow
{
    public Puzzle puzzle = null;


    [MenuItem("PF/Puzzle Editor")]
    static void Init()
    {
        PuzzleEditorWindow wnd = (PuzzleEditorWindow)GetWindow(typeof(PuzzleEditorWindow));
        wnd.Show();
    }


    private void OnGUI()
    {
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.fontStyle = FontStyle.Bold;

        EditorGUILayout.LabelField("Puzzle", labelStyle);
        puzzle = (Puzzle)EditorGUILayout.ObjectField(puzzle, typeof(Puzzle), true);

        EditorGUILayout.BeginVertical();
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
            puzzle.AddFirst(0);
            EditorUtility.SetDirty(puzzle);
        }

        if (btnAddNodeLast)
        {
            puzzle.AddLast(0);
            EditorUtility.SetDirty(puzzle);
        }

        if (btnClear)
        {
            puzzle.Clear();
            EditorUtility.SetDirty(puzzle);
        }
    }


    private bool AreThereErrors(out string errorMsg)
    {
        string msg = string.Empty;
        if (puzzle == null)
        {
            msg += "Puzzle is null. Input a Puzzle from the Scene.\n";
        }
        else if (puzzle != null)
        {
            if (puzzle.platformMgmt == null)
                msg += "The Puzzle is missing its Platform Manager.\n";
            else if (puzzle.slotsCtrl == null)
                msg += "The Puzzle is missing its Platform Slots Controller.\n";
        }

        errorMsg = msg;
        return !(string.IsNullOrEmpty(msg));
    }
}
