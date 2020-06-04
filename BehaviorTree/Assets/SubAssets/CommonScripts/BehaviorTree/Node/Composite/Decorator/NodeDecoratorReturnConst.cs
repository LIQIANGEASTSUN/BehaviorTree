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
        private int _runningNode = 0;

        public NodeDecoratorReturnConst(NODE_TYPE nodeType) : base(nodeType)
        { }

        public override void OnEnter()
        {
            base.OnEnter();
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

            NodeNotify.NotifyExecute(EntityId, NodeId, _constResult, Time.realtimeSinceStartup);

            return _constResult;
        }

        public void SetConstResult(ResultType resultType)
        {
            _constResult = resultType;
        }

        public abstract void Update();
    }
}