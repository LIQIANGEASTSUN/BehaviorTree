using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 随机节点(组合节点)
    /// </summary>
    public class NodeRandom : NodeComposite
    {
        private NodeBase lastRunningNode;
        private int[] idArr = null;
        private int _randomCount = 0;
        private System.Random random;

        public static string descript = "随机执行节点，只要有一个节点返回成功，它就会返\n" +
                                        "回成功，不再执行后续节点如果所有节点都返回 Fail，\n" +
                                        "则它返回 Fail，否则返回 Running";

        public NodeRandom():base(NODE_TYPE.RANDOM)
        {
            random = new System.Random();
        }

        public override void OnEnter()
        {
            base.OnEnter();
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


/*

    RandomArr 一个随机数组

    index = 1
    if != lastRunningNode null then
        index = lastRunningNode.index

        将 index 添加到随机数组的第一位
    end

    lastRunningNode = null
    for i <- 1 to N do 
            
        index = RandomArr[i]
        Node node =  GetNode(index);

        result = node.execute()
        
        if result == fail then
           continue;
        end

        if result == success then
             return result
        end

        if result == running then
            lastRunningNode = node
            return running
        end

    end

    return fail



*/
