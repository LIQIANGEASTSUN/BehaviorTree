using System.Collections.Generic;
using BehaviorTree;
using UnityEditor;

public static class Node  {

    private static NodeAsset nodeAsset;

    public static NodeValue nodeValue;
    public static List<NodeValue> nodeValueList = new List<NodeValue>();
    public static string fileName = string.Empty;

    public static void SetNodeAsset(NodeAsset _nodeAsset, string _fileName)
    {
        nodeAsset = _nodeAsset;
        fileName = _fileName;
    }

    public static NodeValue NodeAssetToNodeValue()
    {
        if (nodeAsset == null || (nodeAsset.nodeValue == null))
        {
            return null;
        }

        nodeValue = nodeAsset.nodeValue;

        nodeValueList.Clear();

        IterationChild(nodeValue);

        Clear();
        return nodeValue;
    }

    public static void Clear()
    {
        nodeAsset = null;
        fileName = string.Empty;
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

    public static void NodeValueToNodeAsset(NodeValue nodeValue, string fileName)
    {
        if (nodeValue == null)
        {
            return;
        }

        CreateNodeAsset.CreateAsset(nodeValue, fileName);
    }
}