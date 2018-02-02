namespace BehaviorTree
{
    /// <summary>
    /// 叶节点
    /// </summary>
    [System.Serializable]
    public abstract class NodeLeaf : NodeRoot
    {
        public NodeLeaf(NodeType nodeType):base(nodeType)
        { }
    }
}