using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 并行选择节点(组合节点)
    /// </summary>
    public class NodeParallelSelect : NodeComposite
    {
        public static string descript = "并行选择节点同时执行所有节点，如果有一个节点返回 Success，\n" +
                                        "则会停止掉其他所有子节点并返回 success只有当所有子节点全部\n" +
                                        "Fail 的时候才会返回 Fail其他情况向父节点返回 Running";


        private int _runningNode = 0;

        public NodeParallelSelect() : base(NODE_TYPE.PARALLEL_SELECT)
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
            int failCount = 0;
            for (int i = 0; i < nodeChildList.Count; ++i)
            {
                NodeBase nodeBase = nodeChildList[i];

                nodeBase.Preposition();
                resultType = nodeBase.Execute();
                nodeBase.Postposition(resultType);

                if (resultType == ResultType.Success)
                {
                    break;
                }

                if (resultType == ResultType.Running)
                {
                    _runningNode |= (1 << i);
                    continue;
                }

                if (resultType == ResultType.Fail)
                {
                    ++failCount;
                    continue;
                }
            }

            if (resultType != ResultType.Success)
            {
                resultType = (failCount >= nodeChildList.Count) ? ResultType.Fail : ResultType.Running;
            }

            NodeNotify.NotifyExecute(EntityId, NodeId, resultType, Time.realtimeSinceStartup);
            return resultType;
        }
    }
}
