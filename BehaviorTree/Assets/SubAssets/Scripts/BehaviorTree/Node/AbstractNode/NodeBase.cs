using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 节点超类
    /// </summary>
    public abstract class NodeBase
    {
        /// <summary>
        /// 节点类型
        /// </summary>
        protected NODE_TYPE nodeType;
        /// <summary>
        /// 节点序列
        /// </summary>
        private int nodeIndex;

        /// <summary>
        /// 节点Id
        /// </summary>
        private int nodeId;

        public NodeBase(NODE_TYPE nodeType)
        {
            this.nodeType = nodeType;
        }

        /// <summary>
        /// 执行节点抽象方法
        /// </summary>
        /// <returns>返回执行结果</returns>
        public abstract ResultType Execute();

        public int NodeIndex
        {
            get { return nodeIndex; }
            set { nodeIndex = value; }
        }

        public int NodeId
        {
            get { return nodeId; }
            set { nodeId = value; }
        }

    }
}