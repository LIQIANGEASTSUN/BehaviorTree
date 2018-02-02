using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BehaviorTree;

public class NodeEditor {
    public static void Draw(NodeValue nodeValue)
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        nodeValue.isRootNode = EditorGUILayout.Toggle( nodeValue.isRootNode, GUILayout.Width(50));
        EditorGUILayout.LabelField("根节点", GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.LabelField("类型", GUILayout.Width(40));
        nodeValue.NodeType = (NodeType)EditorGUILayout.EnumPopup(nodeValue.NodeType, GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);

        EditorGUILayout.EndVertical();
    }
}
