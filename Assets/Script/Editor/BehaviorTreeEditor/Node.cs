using System.Collections.Generic;
using BehaviorTree;
using UnityEditor;

public static class Node  {

    private static NodeAsset nodeAsset;

    public static NodeValue nodeValue;
    public static List<NodeValue> nodeValueList = new List<NodeValue>();

    public static void SetNodeAsset(NodeAsset _nodeAsset)
    {
        nodeAsset = _nodeAsset;
    }

    public static NodeValue NodeAssetToNodeValue()
    {
        //string path = "Assets/NodeAsset/NodeAsset.asset";
        //NodeAsset nodeAsset = AssetDatabase.LoadAssetAtPath<NodeAsset>(path);

        if (nodeAsset == null || (nodeAsset.nodeValue == null))
        {
            return null;
        }

        nodeValue = nodeAsset.nodeValue;

        IterationChild(nodeValue);
        return nodeValue;
    }

    public static void IterationChild(NodeValue nodeValue)
    {
        nodeValueList.Add(nodeValue);
        for (int i = 0; i < nodeValue.childNodeList.Count; ++i)
        {
            NodeValue childNode = nodeValue.childNodeList[i];
            IterationChild(childNode);
        }
    }

    public static void NodeValueToNodeAsset(NodeValue nodeValue)
    {
        if (nodeValue == null)
        {
            return;
        }

        CreateNodeAsset.CreateAsset("New", nodeValue);
    }
}