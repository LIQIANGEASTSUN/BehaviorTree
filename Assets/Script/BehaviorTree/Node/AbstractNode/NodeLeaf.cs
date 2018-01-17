using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 叶节点
    /// </summary>
    public abstract class NodeLeaf : NodeRoot
    {
        public NodeLeaf(NodeType nodeType):base(nodeType)
        {

        }
    }
}