using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeAssetToNode {

	public NodeRoot GetNode(NodeAsset nodeAsset)
    {
        NodeValue nodeValue = nodeAsset.nodeValue;
        if (nodeValue == null)
        {
            Debug.LogError("RootNode is null");
            return null;
        }

        NodeRoot rootNode = GetNodeRoot(nodeValue);

        if (rootNode.GetType().IsSubclassOf(typeof(NodeCombiner)))
        {
            IterationChild((NodeCombiner)rootNode, nodeValue);
        }

        return rootNode;
    }

    public void IterationChild(NodeCombiner parentNode, NodeValue nodeValue)
    {
        for (int i = 0; i < nodeValue.childNodeList.Count; ++i)
        {
            NodeValue childNode = nodeValue.childNodeList[i];
            NodeRoot node = GetNodeRoot(childNode);
            parentNode.AddNode(node);

            if (node.GetType().IsSubclassOf(typeof(NodeCombiner)))
            {
                IterationChild((NodeCombiner)node, childNode);
            }
        }
    }

    private NodeRoot GetNodeRoot(NodeValue nodeValue)
    {
        NodeRoot nodeRoot = null;
        switch (nodeValue.NodeType)
        {
            case NodeType.Select:    // 选择节点
                nodeRoot = new NodeSelect();
                break;
            case NodeType.Sequence:  // 顺序节点
                nodeRoot = new NodeSequence();
                break;
            case NodeType.Decorator: // 修饰节点
                nodeRoot = new NodeDecorator();
                break;
            case NodeType.Random:    // 随机节点
                nodeRoot = new NodeRandom();
                break;
            case NodeType.Parallel:  // 并行节点
                nodeRoot = new NodeParallel();
                break;
            case NodeType.Condition: // 条件节点
                nodeRoot = GetLeafNode(nodeValue);
                break;
            case NodeType.Action:    // 行为节点
                nodeRoot = GetLeafNode(nodeValue);
                break;
        }

        return nodeRoot;
    }

    private NodeRoot GetLeafNode(NodeValue nodeValue)
    {
        string componentName = nodeValue.componentName;

        NodeRoot nodeRoot = null;
        switch (componentName)
        {
            case "NodeActionCooking":
                nodeRoot = new NodeActionCooking();
                break;
            case "NodeActionEat":
                nodeRoot = new NodeActionEat();
                break;
            case "NodeActionMove":
                nodeRoot = new NodeActionMove();
                break;
            case "NodeConditionHasFood":
                nodeRoot = new NodeConditionHasFood();
                break;
            case "NodeConditionHungry":
                nodeRoot = new NodeConditionHungry();
                break;
        }

        return nodeRoot;
    }
}