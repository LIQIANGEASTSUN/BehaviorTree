using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// 组合节点
    /// </summary>
    public abstract class NodeCombiner : NodeRoot
    {
        // 保存子节点
        protected List<NodeRoot> nodeChildList = new List<NodeRoot>();

        public NodeCombiner(NodeType nodeType) : base(nodeType)
        {}

        public void AddNode(NodeRoot nodeRoot)
        {
            int count = nodeChildList.Count;
            nodeRoot.NodeIndex = count;
            nodeChildList.Add(nodeRoot);
        }
    }
}