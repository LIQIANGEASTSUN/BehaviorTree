using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BehaviorTree;

public class NodeEditor {

    public static void Draw(NodeValue nodeValue)
    {
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("根节点", GUILayout.Width(50));
        nodeValue.isRootNode = EditorGUILayout.Toggle( nodeValue.isRootNode, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.LabelField("类型", GUILayout.Width(40));
        nodeValue.NodeType = (NodeType)EditorGUILayout.EnumPopup(nodeValue.NodeType, GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        DrawTypeNode(nodeValue);
    }

    private static void DrawTypeNode(NodeValue nodeValue)
    {
        switch (nodeValue.NodeType)
        {
            case NodeType.Select: // 选择节点
            case NodeType.Sequence: // 顺序节点
            case NodeType.Decorator:// 修饰节点
            case NodeType.Random:   // 随机节点
            case NodeType.Parallel: // 并行节点
                SetHight(nodeValue, 65);
                break;
            case NodeType.Condition:// 条件节点
            case NodeType.Action:   // 行为节点
                DrawLeftNode(nodeValue);
                break;
        }
    }

    private static UnityEngine.Component obj = null;
    #region 叶节点
    /// <summary>
    /// 条件节点
    /// </summary>
    private static void DrawLeftNode(NodeValue nodeValue)
    {
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField("脚本", GUILayout.Width(50));
        nodeValue.componentName = EditorGUILayout.TextField(nodeValue.componentName);

        GUILayout.Space(5);

        EditorGUILayout.LabelField("描述");
        nodeValue.description = EditorGUILayout.TextField(nodeValue.description);

        EditorGUILayout.EndVertical();

        SetHight(nodeValue, 150);
    }
    #endregion

    private static void SetHight(NodeValue nodeValue, float hight)
    {
        Rect rect = nodeValue.position;
        rect.height = hight;
        nodeValue.position = rect;
    }

    public static string GetTitle(NodeType nodeType)
    {
        string title = string.Empty;
        switch (nodeType)
        {
            case NodeType.Select: // 选择节点
                title = "选择节点";
                break;
            case NodeType.Sequence: // 顺序节点
                title = "顺序节点";
                break;
            case NodeType.Decorator:// 修饰节点
                title = "修饰节点";
                break;
            case NodeType.Random:   // 随机节点
                title = "随机节点";
                break;
            case NodeType.Parallel: // 并行节点
                title = "并行节点";
                break;
            case NodeType.Condition:// 条件节点
                title = "条件节点";
                break;
            case NodeType.Action:   // 行为节点
                title = "行为节点";
                break;
        }

        return title;
    }

    public static void CheckNode(List<NodeValue> nodeValueList)
    {
        int rootNodeCount = 0;
        bool nodeInvalid = false;
        // 开始绘制节点 
        // 注意：必须在  BeginWindows(); 和 EndWindows(); 之间 调用 GUI.Window 才能显示
        for (int i = 0; i < nodeValueList.Count; i++)
        {
            NodeValue nodeValue = nodeValueList[i];
            if (nodeValue.isRootNode)
            {
                ++rootNodeCount;
            }

            if ((nodeValue.NodeType == NodeType.Condition || nodeValue.NodeType == NodeType.Action) && nodeValue.childNodeList.Count > 0)
            {
                nodeInvalid = true; // 叶节点 不能有子节点
            }
        }

        string meg = string.Empty;
        if (rootNodeCount > 1)
        {
            meg = "只能有一个根节点";
        }
        else if (rootNodeCount == 0)
        {
            meg = "必须有一个根节点";
        }

        if (nodeInvalid)
        {
            meg = "条件节点、行为节点 不能有子节点";
        }

        if (TreeNodeEditor.window != null)
        {
            TreeNodeEditor.window.ShowNotification(!string.IsNullOrEmpty(meg), meg);
        }
    }
}