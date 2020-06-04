using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 并行节点(组合节点)
    /// </summary>
    public class NodeParallel : NodeComposite
    {
        public static string descript = "并行节点同时执行所有节点，直到一个节点返回 Fail 或\n" +
                                       "者全部节点都返回 Success才向父节点返回 Fail 或者 \n" +
                                        "Success，并终止执行其他节点其他情况向父节点返回 Running";


        private int _runningNode = 0;

        public NodeParallel():base(NODE_TYPE.PARALLEL)
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
            for (int i = 0; i < nodeChildList.Count; ++i)
            {
                NodeBase nodeBase = nodeChildList[i];

                nodeBase.Preposition();
                resultType = nodeBase.Execute();
                nodeBase.Postposition(resultType);

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
                    _runningNode |= (1 << i);
                    continue;
                }
            }

            if (resultType != ResultType.Fail)
            {
                resultType = (successCount >= nodeChildList.Count) ? ResultType.Success : ResultType.Running;
            }

            NodeNotify.NotifyExecute(EntityId, NodeId, resultType, Time.realtimeSinceStartup);
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
