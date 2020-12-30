using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// 叶节点
    /// </summary>
    [System.Serializable]
    public class NodeLeaf : NodeBase
    {
        public NodeLeaf():base()
        {
        }

        public override ResultType Execute()
        {
            return ResultType.Fail;
        }
    }
}