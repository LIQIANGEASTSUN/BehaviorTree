using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System.IO;

[CustomEditor(typeof(NodeAsset))]
public class NodeAssetEditor : Editor
{
    private static bool isEnable = false;

    private void OnEnable()
    {
        isEnable = true;
    }

    private void OnDisable()
    {
        isEnable = false;
    }

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
        if (!isEnable)
        {
            return false;
        }
        ShowEditor();
        return true;
    }

    private static void ShowEditor()
    {
        UnityEngine.Object obj = Selection.activeObject;
        string path = AssetDatabase.GetAssetPath(obj);

        NodeAsset nodeAsset = AssetDatabase.LoadAssetAtPath<NodeAsset>(path);
        Node.SetNodeAsset(nodeAsset, Path.GetFileNameWithoutExtension(path));

        TreeNodeEditor.ShowWindow();
    }
}