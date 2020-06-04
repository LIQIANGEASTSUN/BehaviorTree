using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 并行执行所以节点
    /// </summary>
    public class NodeParallelAll : NodeComposite
    {
        public static string descript = "并行执行所有节点同时执行所有节点，全部节点都返回\n" +
                                        "Success才向父节点返回 Success, 全部节点都返回 Fail \n" +
                                        "向父节点返回 Fail，否则返回 Running";


        private int _runningNode = 0;

        public NodeParallelAll() : base(NODE_TYPE.PARALLEL_ALL)
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

            int successCount = 0;
            int failCount = 0;
            for (int i = 0; i < nodeChildList.Count; ++i)
            {
                NodeBase nodeBase = nodeChildList[i];

                nodeBase.Preposition();
                resultType = nodeBase.Execute();
                nodeBase.Postposition(resultType);

                if (resultType == ResultType.Fail)
                {
                    ++failCount;
                    continue;
                }

                if (resultType == ResultType.Success)
                {
                    ++successCount;
                    continue;
                }

                if (resultType == ResultType.Running)
                {
                    _runningNode |= (1 << i);
                    continue;
                }
            }

            if (successCount >= nodeChildList.Count)
            {
                resultType = ResultType.Success;
            }
            else if (failCount >= nodeChildList.Count)
            {
                resultType = ResultType.Fail;
            }
            else
            {
                resultType = ResultType.Running;
            }

            NodeNotify.NotifyExecute(EntityId, NodeId, resultType, Time.realtimeSinceStartup);
            return resultType;
        }
    }
}



