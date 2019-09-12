using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// 条件节点(叶节点)
    /// </summary>
    public class NodeCondition : NodeLeaf
    {
        public NodeCondition() : base(NODE_TYPE.CONDITION)
        {
        }

        public override ResultType Execute()
        {
            return ResultType.Success;
        }
    }
}