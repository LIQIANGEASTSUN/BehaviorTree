using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// 叶节点
    /// </summary>
    [System.Serializable]
    public abstract class NodeLeaf : NodeBase
    {
        public NodeLeaf(NODE_TYPE nodeType):base(nodeType)
        { }

        public override ResultType Execute()
        {
            return ResultType.Fail;
        }
    }
}