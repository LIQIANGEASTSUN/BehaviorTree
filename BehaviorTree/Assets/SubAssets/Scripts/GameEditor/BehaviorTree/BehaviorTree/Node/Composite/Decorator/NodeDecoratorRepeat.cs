using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 重复执行修饰节点 Repeater 重复执行子节点 N 次
    /// </summary>
    public class NodeDecoratorRepeat : NodeDecorator
    {
        private int _repeatCount = 0;
        private int _executeCount = 0;
        public NodeDecoratorRepeat() : base(NODE_TYPE.DECORATOR_REPEAT)
        { }

        public override ResultType Execute()
        {
            NodeNotify.NotifyExecute(NodeId, Time.realtimeSinceStartup);

            NodeBase nodeRoot = nodeChildList[0];
            ResultType resultType = nodeRoot.Execute();

            ++_executeCount;

            if (_executeCount < _repeatCount)
            {
                resultType = ResultType.Running;
            }
            else
            {
                resultType = ResultType.Fail;
            }

            return resultType;
        }

        public void ReStart()
        {
            _executeCount = 0;
        }

        public void SetRepeatCount(int value)
        {
            _repeatCount = value;
            ReStart();
        }

    }
}