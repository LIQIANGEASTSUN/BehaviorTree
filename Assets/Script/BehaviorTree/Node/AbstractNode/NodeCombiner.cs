using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 组合节点
    /// </summary>
    public abstract class NodeCombiner : NodeRoot
    {
        protected List<NodeRoot> nodeChildList = new List<NodeRoot>();

        public NodeCombiner(NodeType nodeType) : base(nodeType)
        {

        }

        public void AddNode(NodeRoot nodeRoot)
        {
            int count = nodeChildList.Count;
            nodeRoot.SetIndex(count);
            nodeChildList.Add(nodeRoot);
        }
    }
}