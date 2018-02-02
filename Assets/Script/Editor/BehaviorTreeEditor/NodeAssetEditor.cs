using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

[CustomEditor(typeof(NodeAsset))]
public class NodeAssetEditor : Editor
{

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical("box");
        GUI.enabled = false;
        base.OnInspectorGUI();
        GUI.enabled = true;
        EditorGUILayout.EndVertical();

        GUILayout.Space(50);
        if (GUILayout.Button("Editor"))
        {
            ShowEditor();
        }
    }

    [OnOpenAsset(1)]
    public static bool OpenBehaviorFromUnityTool(int instanceID, int line)
    {
        ShowEditor();
        return true;
    }

    private static void ShowEditor()
    {
        UnityEngine.Object obj = Selection.activeObject;
        string path = AssetDatabase.GetAssetPath(obj);
        //string path = "Assets/NodeAsset/NodeAsset.asset";

        NodeAsset nodeAsset = AssetDatabase.LoadAssetAtPath<NodeAsset>(path);
        Node.SetNodeAsset(nodeAsset);

        TreeNodeEditor.ShowWindow();
    }
}