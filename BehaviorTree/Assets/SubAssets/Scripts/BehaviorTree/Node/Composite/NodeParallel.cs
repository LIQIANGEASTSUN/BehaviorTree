using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 并行节点(组合节点)
    /// </summary>
    public class NodeParallel : NodeComposite
    {
        public NodeParallel():base(NODE_TYPE.PARALLEL)
        { }

        /// <summary>
        /// 并行节点同时执行所有节点，直到一个节点返回 Fail 或者全部节点都返回 Success
        /// 才向父节点返回 Fail 或者 Success，并终止执行其他节点
        /// 其他情况向父节点返回 Running
        /// </summary>
        /// <returns></returns>
        public override ResultType Execute()
        {
            NodeNotify.NotifyExecute(NodeId, Time.realtimeSinceStartup);

            ResultType resultType = ResultType.Fail;

            int successCount = 0;
            for (int i = 0; i < nodeChildList.Count; ++i)
            {
                NodeBase nodeRoot = nodeChildList[i];
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

            if (resultType != ResultType.Fail)
            {
                resultType = (successCount >= nodeChildList.Count) ? ResultType.Success : ResultType.Running;
            }

            return resultType;
        }
    }
}


/*
    
    successCount = 0

    for i <- index to N do 
    
        Node node =  GetNode(i);

        result = node.execute()
        
        if result == fail then
           return fail;
        end

        if result == success then
            ++successCount
            continue
        end

        if result == running then
            continue
        end
    end

    if successCount >= childCount then
        return success
    end

    return running
*/
