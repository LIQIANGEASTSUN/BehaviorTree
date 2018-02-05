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
    // 根节点
    public bool isRootNode = false;
    // 节点类型
    public NodeType NodeType = NodeType.Select;
    // 子节点集合
    public List<NodeValue> childNodeList = new List<NodeValue>();
    // 叶节点脚本名
    public string componentName = string.Empty;

    // 条件节点、行为节点描述（辅助查看）
    public string description = string.Empty;
    // 节点位置（编辑器显示使用）
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
