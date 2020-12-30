using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// 组合节点
    /// </summary>
    public abstract class NodeComposite : NodeBase
    {
        // 保存子节点
        protected List<NodeBase> nodeChildList = new List<NodeBase>();

        public NodeComposite(NODE_TYPE nodeType) : base()
        {
            SetNodeType(nodeType);
        }

        public void AddNode(NodeBase node)
        {
            int count = nodeChildList.Count;
            node.NodeIndex = count;
            nodeChildList.Add(node);
        }

        public List<NodeBase> GetChilds()
        {
            return nodeChildList;
        }

        public override ResultType Execute()
        {
            return ResultType.Fail;
        }
    }
}