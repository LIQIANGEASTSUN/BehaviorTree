using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 随机节点(组合节点)
    /// </summary>
    public class NodeRandom : NodeCombiner
    {
        private NodeRoot lastRunningNode;
        public NodeRandom():base(NodeType.Random)
        {

        }

        public override ResultType Execute()
        {
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
                NodeRoot nodeRoot = nodeChildList[index];
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