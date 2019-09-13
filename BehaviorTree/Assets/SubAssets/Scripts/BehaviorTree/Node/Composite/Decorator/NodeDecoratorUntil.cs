using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 修饰节点：一直执行节点，直到达到条件
    /// </summary>
    public abstract class NodeDecoratorUntil : NodeDecorator
    {
        private ResultType _desiredResult = ResultType.Fail;
        public NodeDecoratorUntil(NODE_TYPE nodeType) : base(nodeType)
        { }

        public override ResultType Execute()
        {
            NodeNotify.NotifyExecute(NodeId, Time.realtimeSinceStartup);

            NodeBase nodeRoot = nodeChildList[0];
            ResultType resultType = nodeRoot.Execute();
            if (resultType == _desiredResult)
            {
                return ResultType.Fail;
            }

            Update();

            return ResultType.Running;
        }

        public void SetDesiredResult(ResultType resultType)
        {
            _desiredResult = resultType;
        }

        public abstract void Update();

    }
}