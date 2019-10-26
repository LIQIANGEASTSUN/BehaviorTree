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
        public NodeRandom():base(NODE_TYPE.RANDOM)
        {   }

        public override ResultType Execute()
        {
            NodeNotify.NotifyExecute(NodeId, Time.realtimeSinceStartup);

            List<int> randomList = GetRandom(nodeChildList.Count);

            int index = -1;
            if (lastRunningNode != null)
            {
                index = lastRunningNode.NodeIndex;
            }
            lastRunningNode = null;

            ResultType resultType = ResultType.Fail;

            while((randomList.Count > 0))
            {
                if (index < 0)
                {
                    index = randomList[randomList.Count - 1];
                    randomList.RemoveAt(randomList.Count - 1);
                }
                NodeBase nodeRoot = nodeChildList[index];
                index = -1;

                resultType = nodeRoot.Execute();

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
                    lastRunningNode = nodeRoot;
                    break;
                }
            }

            return resultType;
        }

        private List<int> GetRandom(int count)
        {
            List<int> resultList = new List<int>(count);

            List<int> tempList = new List<int>();
            for (int i = 0; i < count; ++i)
            {
                tempList.Add(i);
            }

            System.Random random = new System.Random();
            while(tempList.Count > 0)
            {
                int index = random.Next(0, (tempList.Count - 1));
                resultList.Add(tempList[index]);
                tempList.RemoveAt(index);
            }

            return resultList;
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
