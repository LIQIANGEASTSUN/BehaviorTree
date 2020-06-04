using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 随机权重节点(组合节点)
    /// </summary>
    public class NodeRandomPriority : NodeComposite
    {

        private NodeBase lastRunningNode;
        private int[] _priorityArr;
        private int _totalPriotity = 0;
        private System.Random random = null;

        public static string descript = "根据优先级权重随机执行节点，只要有一个节点返回成\n" +
                                        "功，它就会返回成功，不再执行后续节点如果所有节点\n" +
                                        "都返回 Fail，则它返回 Fail，否则返回 Running";

        public NodeRandomPriority() : base(NODE_TYPE.RANDOM_PRIORITY)
        {
            random = new System.Random();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            lastRunningNode = null;
        }

        public override void OnExit()
        {
            base.OnExit();

            if (null != lastRunningNode)
            {
                lastRunningNode.Postposition(ResultType.Fail);
                lastRunningNode = null;
            }
        }

        public override ResultType Execute()
        {
            int index = -1;
            if (lastRunningNode != null)
            {
                index = lastRunningNode.NodeIndex;
            }
            lastRunningNode = null;

            ResultType resultType = ResultType.Fail;

            for (int i = 0; i < nodeChildList.Count; ++i)
            {
                if (index < 0)
                {
                    index = GetRandom();
                }
                NodeBase nodeBase = nodeChildList[index];
                index = -1;


                nodeBase.Preposition();
                resultType = nodeBase.Execute();
                nodeBase.Postposition(resultType);

                if (resultType == ResultType.Fail)
                {
                    continue;
                }

                if (resultType == ResultType.Success)
                {
                    break;
                }

                if (resultType == ResultType.Running)
                {
                    lastRunningNode = nodeBase;
                    break;
                }
            }

            NodeNotify.NotifyExecute(EntityId, NodeId, resultType, Time.realtimeSinceStartup);
            return resultType;
        }

        private int GetRandom()
        {
            if (null == _priorityArr)
            {
                _priorityArr = new int[nodeChildList.Count];
                for (int i = 0; i < nodeChildList.Count; ++i)
                {
                    _totalPriotity += nodeChildList[i].Priority;
                    _priorityArr[i] = nodeChildList[i].Priority;
                }
            }

            int value = random.Next(0, _totalPriotity + 1);
            int tempPriority = 0;
            for (int i = 0; i < _priorityArr.Length; ++i)
            {
                tempPriority += _priorityArr[i];
                if (tempPriority >= value)
                {
                    return i;
                }
            }

            return 0;
        }

    }

}

