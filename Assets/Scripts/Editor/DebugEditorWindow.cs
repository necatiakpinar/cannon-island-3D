using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DebugEditorWindow : EditorWindow
{


    [MenuItem("Madcraft/Save_Debug")]
    public static void ShowWindow()
    {
        DebugEditorWindow window = (DebugEditorWindow)GetWindow<DebugEditorWindow>("Debug Editor");
        window.minSize = new Vector2(300, 160);
        window.maxSize = new Vector2(300, 160);
        SaveSystem.Load();
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(20, 120, 260, 60));
        if (GUILayout.Button("Reset Everything"))
        {
            SaveSystem.Data = new SaveData();
            SaveSystem.Save();
        }
        GUILayout.EndArea();
    }

}