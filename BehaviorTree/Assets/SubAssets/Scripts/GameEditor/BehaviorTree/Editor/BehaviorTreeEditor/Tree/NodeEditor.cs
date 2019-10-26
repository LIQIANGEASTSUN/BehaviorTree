using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BehaviorTree;

public class NodeEditor {

    private static int height = 75;
    private static float value = 0.5f;

    public static void Draw(NodeValue nodeValue, int selectNodeId, float value = 0f)
    {
        EditorGUILayout.BeginVertical("box", GUILayout.Height(height));
        {
            if (nodeValue.id == selectNodeId)
            {
                GUI.backgroundColor = new Color(0, 1, 0, 0.15f);
                GUI.Box(new Rect(5, 20, nodeValue.position.width - 10, height), string.Empty);
                GUI.backgroundColor = Color.white;
            }

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(new GUIContent("根节点"), GUILayout.Width(50));
                nodeValue.isRootNode = EditorGUILayout.Toggle(nodeValue.isRootNode, GUILayout.Width(50));
            }
            EditorGUILayout.EndHorizontal();

            int parentID = nodeValue.parentNodeID;
            //if (parentID > 0)
            {
                string msg = string.Format("父节点_{0}", parentID);
                EditorGUILayout.LabelField(msg);
            }

            nodeValue.descript = EditorGUILayout.TextArea(nodeValue.descript);

            {
                float slider = NodeNotify.NodeDraw(nodeValue.id);

                Rect runRect = new Rect(8, 80, nodeValue.position.width - 16, 8);
                GUI.Box(runRect, string.Empty);
                if (slider > 0)
                {
                    runRect.width = runRect.width * slider;
                    GUI.backgroundColor = Color.green;
                    GUI.Box(runRect, string.Empty);
                    GUI.backgroundColor = Color.white;
                }
            }
           
            //GUILayout.HorizontalSlider(value, 0, 1);
        }
        EditorGUILayout.EndVertical();

        SetHight(nodeValue);
    }

    private static void SetHight(NodeValue nodeValue)
    {
        RectT rect = nodeValue.position;
        rect.height = 95;
        nodeValue.position = rect;
    }

    public static string GetTitle(NODE_TYPE nodeType)
    {
        int index = EnumNames.GetEnumIndex<NODE_TYPE>(nodeType);
        string title = EnumNames.GetEnumName<NODE_TYPE>(index);
        return title;
    }
}