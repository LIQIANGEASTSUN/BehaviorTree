using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 修饰节点_返回 固定结果
    /// </summary>
    public abstract class NodeDecoratorReturnConst : NodeDecorator
    {
        private ResultType _constResult = ResultType.Fail;
        public NodeDecoratorReturnConst(NODE_TYPE nodeType) : base(nodeType)
        { }

        public override ResultType Execute()
        {
            NodeNotify.NotifyExecute(NodeId, Time.realtimeSinceStartup);

            NodeBase nodeRoot = nodeChildList[0];
            ResultType resultType = nodeRoot.Execute();

            return _constResult;
        }

        public void SetConstResult(ResultType resultType)
        {
            _constResult = resultType;
        }

        public abstract void Update();
    }
}