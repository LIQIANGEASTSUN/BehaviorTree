using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 条件节点(叶节点)
    /// </summary>
    public abstract class NodeCondition : NodeLeaf
    {
        public NodeCondition() : base(NodeType.Condition)
        {
        }
    }
}