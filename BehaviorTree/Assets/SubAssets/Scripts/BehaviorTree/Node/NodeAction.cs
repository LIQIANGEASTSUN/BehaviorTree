using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// 行为节点(叶节点)
    /// </summary>
    public abstract class NodeAction : NodeLeaf
    {

        public NodeAction() : base(NODE_TYPE.ACTION)
        {
        }

    }

}