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
            NodeBase nodeRoot = nodeChildList[0];
            ResultType resultType = nodeRoot.Execute();
            if (resultType == ResultType.Fail)
            {
                return ResultType.Success;
            }
            else
            {
                return ResultType.Fail;
            }
        }
    }
}

