using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 选择节点(组合节点)
    /// </summary>
    public class NodeSelect : NodeComposite
    {
        private NodeBase lastRunningNode;
        public static string descript = "选择节点依次遍历所有子节点，如果都返回 Fail，\n" +
                                        "则向父节点返回 Fail直到一个节点返回 Success \n" +
                                        "或者Running，停止后续节点的执行，向父节点返回\n" +
                                        " Success 或者 Running注意：如果节点返回 Running\n" +
                                        " 需要记住这个节点，下次直接从此节点开始执行\n";

        public NodeSelect():base(NODE_TYPE.SELECT)
        {    }

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

        /// <summary>
        /// NodeDescript.GetDescript(NODE_TYPE);
        /// </summary>
        public override ResultType Execute()
        {
            int index = 0;
            if (lastRunningNode != null)
            {
                index = lastRunningNode.NodeIndex;
            }
            lastRunningNode = null;

            ResultType resultType = ResultType.Fail;
            for (int i = index; i < nodeChildList.Count; ++i)
            {
                NodeBase nodeBase = nodeChildList[i];

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
    }
}

/*
 
    index = 1
    if != lastRunningNode null then
        index = lastRunningNode.index
    end

    lastRunningNode = null
    for i <- index to N do 
    
        Node node =  GetNode(i);

        result = node.execute()
        
        if result == fail then
           continue;
        end

        if result == success then
            return success
        end

        if result == running then
            lastRunningNode = node
            return running
        end

    end

    return fail

*/
