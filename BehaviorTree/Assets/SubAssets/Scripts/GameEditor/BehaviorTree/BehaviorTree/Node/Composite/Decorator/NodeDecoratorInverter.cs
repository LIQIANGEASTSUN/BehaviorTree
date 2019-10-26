using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 取反修饰节点 Inverter        对子节点执行结果取反
    /// </summary>
    public class NodeDecoratorInverter : NodeDecorator
    {
        public NodeDecoratorInverter() : base(NODE_TYPE.DECORATOR_INVERTER)
        { }
        
        public override ResultType Execute()
        {
            NodeNotify.NotifyExecute(NodeId, Time.realtimeSinceStartup);

            ResultType resultType = ResultType.Fail;

            NodeBase nodeRoot = nodeChildList[0];
            ResultType type = nodeRoot.Execute();
            if (type == ResultType.Fail)
            {
                resultType = ResultType.Success;
            }
            else
            {
                resultType = ResultType.Fail;
            }

            return resultType;
        }
    }
}

