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
        public static string descript = "取反修饰节点 Inverter    对子节点执行结果取反";

        private int _runningNode = 0;

        public NodeDecoratorInverter() : base(NODE_TYPE.DECORATOR_INVERTER)
        { }

        public override void OnEnter()
        {
            base.OnEnter();
            _runningNode = 0;
        }

        public override void OnExit()
        {
            base.OnExit();

            for (int i = 0; i < nodeChildList.Count; ++i)
            {
                int value = (1 << i);
                if ((_runningNode & value) > 0)
                {
                    NodeBase nodeBase = nodeChildList[i];
                    nodeBase.Postposition(ResultType.Fail);
                }
            }
        }

        public override ResultType Execute()
        {
            ResultType resultType = ResultType.Fail;
            for (int i = 0; i < nodeChildList.Count; ++i)
            {
                NodeBase nodeBase = nodeChildList[i];

                nodeBase.Preposition();
                resultType = nodeBase.Execute();
                nodeBase.Postposition(resultType);

                if (resultType == ResultType.Running)
                {
                    _runningNode |= (1 << i);
                }
            }

            if (resultType == ResultType.Fail)
            {
                return ResultType.Success;
            }
            else if (resultType == ResultType.Success)
            {
                return ResultType.Fail;
            }

            NodeNotify.NotifyExecute(EntityId, NodeId, resultType, Time.realtimeSinceStartup);
            return resultType;
        }
    }
}

