using UnityEngine;
using BehaviorTree;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NodeAsset", menuName = "NewNodeAsset", order = 1)]
[System.Serializable]
public class NodeAsset : ScriptableObject
{
    [SerializeField]
    public NodeValue nodeValue = null;
}

[Serializable]
public class NodeValue
{
    public bool isRootNode = false;
    public NodeType NodeType = NodeType.Select;
    public string actionName = string.Empty;
    public List<NodeValue> childNodeList = new List<NodeValue>();

    public Rect position = new Rect(0, 0, 100, 100);

    /// <summary>
    /// 是否为有效节点， isRelease = true 为已经销毁的节点，为无效节点
    /// </summary>
    public bool isRelease = false;
    /// <summary>
    /// 删除节点时调用
    /// </summary>
    public void Release()
    {
        isRelease = true;
    }
}
