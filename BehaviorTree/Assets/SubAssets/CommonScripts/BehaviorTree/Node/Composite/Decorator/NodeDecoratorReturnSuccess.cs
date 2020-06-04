using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 修饰节点_返回Success
    /// </summary>
    public class NodeDecoratorReturnSuccess : NodeDecoratorReturnConst
    {
        public static string descript = "修饰节点_返回Success 无论执行结果如何都返回 Success";
        private int _runningNode = 0;

        public NodeDecoratorReturnSuccess() : base(NODE_TYPE.DECORATOR_RETURN_SUCCESS)
        {
            SetConstResult(ResultType.Success);
        }

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

            NodeNotify.NotifyExecute(EntityId, NodeId, ResultType.Success, Time.realtimeSinceStartup);

            return ResultType.Success;
        }

        public override void Update()
        {
            
        }
    }
}