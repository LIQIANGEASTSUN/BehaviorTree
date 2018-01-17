using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 并行节点(组合节点)
    /// </summary>
    public class NodeParallel : NodeCombiner
    {

        public NodeParallel():base(NodeType.Parallel)
        {

        }

        /// <summary>
        /// 并行节点同时执行所有节点，直到一个节点返回 Fail 或者全部节点都返回 Success
        /// 才向父节点返回 Fail 或者 Success，并终止执行其他节点
        /// 其他情况向父节点返回 Running
        /// </summary>
        /// <returns></returns>
        public override ResultType Execute()
        {
            ResultType resultType = ResultType.Fail;

            int successCount = 0;
            for (int i = 0; i < nodeChildList.Count; ++i)
            {
                NodeRoot nodeRoot = nodeChildList[i];
                resultType = nodeRoot.Execute();

                if (resultType == ResultType.Fail)
                {
                    break;
                }

                if (resultType == ResultType.Success)
                {
                    ++successCount;
                    continue;
                }

                if (resultType == ResultType.Running)
                {
                    continue;
                }
            }

            if (resultType == ResultType.Fail)
            {
                return resultType;
            }
            else if (resultType == ResultType.Success)
            {
                resultType = (successCount >= nodeChildList.Count) ? resultType : ResultType.Running;
                return resultType;
            }

            return ResultType.Running;
        }
    }
}