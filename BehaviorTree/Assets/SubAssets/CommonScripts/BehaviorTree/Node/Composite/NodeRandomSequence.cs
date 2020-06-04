using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{

    /// <summary>
    /// 随机顺序节点(组合节点)
    /// </summary>
    public class NodeRandomSequence : NodeComposite
    {
        private NodeBase lastRunningNode;
        private int[] idArr = null;
        private int _randomCount = 0;
        private System.Random random;

        public static string descript = "随机执行节点，只要有一个节点返回 Fail，它就会返回\n" +
                                        "Fail，不再执行后续节点如果所有节点都返回 Success，\n" +
                                        "则它返回 Success，否则返回 Running";

        public NodeRandomSequence() : base(NODE_TYPE.RANDOM_SEQUEUECE)
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
            _randomCount = 0;

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
                    break;
                }

                if (resultType == ResultType.Success)
                {
                    continue;
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
            if (null == idArr)
            {
                idArr = new int[nodeChildList.Count];
                for (int i = 0; i < idArr.Length; ++i)
                {
                    idArr[i] = i;
                }
            }

            int count = idArr.Length - 1;
            int index = random.Next(0, idArr.Length - _randomCount);
            int value = idArr[index];
            idArr[index] = idArr[count - _randomCount];
            ++_randomCount;

            return value;
        }
    }

}

