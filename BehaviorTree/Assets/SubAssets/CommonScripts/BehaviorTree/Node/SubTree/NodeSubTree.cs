using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 子树
    /// </summary>
    public class NodeSubTree : NodeComposite
    {
        private NodeBase lastRunningNode;
        public static string descript = "子树";

        public NodeSubTree() : base(NODE_TYPE.SUB_TREE)
        { }

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
            ResultType resultType = ResultType.Fail;
            if (nodeChildList.Count <= 0)
            {
                return ResultType.Fail;
            }

            NodeBase nodeBase = nodeChildList[0];
            nodeBase.Preposition();
            resultType = nodeBase.Execute();
            nodeBase.Postposition(resultType);

            NodeNotify.NotifyExecute(EntityId, NodeId, resultType, Time.realtimeSinceStartup);
            return resultType;
        }
    }

}
