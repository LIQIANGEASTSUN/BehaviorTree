using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BehaviorTree;

public class NodeEditor {

    private static int height = 45;
    public static void Draw(NodeValue nodeValue, int selectNodeId, float value = 0f)
    {
        nodeValue.position.height = GetHight(nodeValue);
        height = (int)nodeValue.position.height - 15;

        EditorGUILayout.BeginVertical(/*"box",*/ GUILayout.Height(height));
        {
            Rect backRect = new Rect(5, 20, nodeValue.position.width - 10, height - 8);
            GUI.backgroundColor = GetColor(nodeValue, selectNodeId);
            GUI.Box(backRect, string.Empty);
            GUI.backgroundColor = Color.white;

            nodeValue.descript = EditorGUILayout.TextField(nodeValue.descript);

            if (!nodeValue.showChildNode)
            {
                EditorGUILayout.LabelField("子节点已隐藏");
            }
            if (nodeValue.NodeType == (int)NODE_TYPE.SUB_TREE)
            {
                EditorGUILayout.LabelField("子树");
            }

            ResultType resultType = ResultType.Fail;
            float slider = NodeNotify.NodeDraw(nodeValue.id, ref resultType);

            Rect runRect = new Rect(8, height, nodeValue.position.width - 16, 8);
            GUI.Box(runRect, string.Empty);
            if (slider > 0)
            {
                runRect.width = runRect.width * slider;
                GUI.backgroundColor = GetSliderColor(resultType);
                GUI.Box(runRect, string.Empty);
                GUI.backgroundColor = Color.white;
            }
        }
        EditorGUILayout.EndVertical();
    }

    private static Color GetColor(NodeValue nodeValue, int selectNodeId)
    {
        Color color = Color.white;
        if (nodeValue.isRootNode || nodeValue.subTreeEntry)
        {
            color = Color.green;
        }
        else if (nodeValue.id == selectNodeId)
        {
            color = new Color(0, 1, 0, 0.35f);
        }
        else
        {
            color = new Color(1f, 0.92f, 0.016f, 1f);
        }

        return color;
    }

    private static Color GetSliderColor(ResultType resultType)
    {
        Color color = Color.white;
        if (resultType == ResultType.Fail)
        {
            color = Color.red;
        }
        else if (resultType == ResultType.Running)
        {
            color = Color.green;
        }
        else if (resultType == ResultType.Success)
        {
            color = Color.blue;
        }
        return color;
    }

    public static string GetTitle(NODE_TYPE nodeType)
    {
        int index = EnumNames.GetEnumIndex<NODE_TYPE>(nodeType);
        string title = EnumNames.GetEnumName<NODE_TYPE>(index);
        return title;
    }

    private static int GetHight(NodeValue nodeValue)
    {
        int height = 60;
        if (!nodeValue.showChildNode || nodeValue.NodeType == (int)NODE_TYPE.SUB_TREE)
        {
            height = 80;
        }

        return height;
    }

}