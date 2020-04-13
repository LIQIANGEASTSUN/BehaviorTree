using UnityEngine;
using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// 行为节点(叶节点)
    /// </summary>
    public abstract class NodeAction : NodeLeaf, IAction
    {
        protected IAction iAction;
        protected List<BehaviorParameter> _parameterList = new List<BehaviorParameter>();

        public NodeAction() : base(NODE_TYPE.ACTION)
        {
            SetIAction(this);
        }

        public void SetIAction(IAction iA)
        {
            iAction = iA;
        }

        public override ResultType Execute()
        {
            NodeNotify.NotifyExecute(NodeId, Time.realtimeSinceStartup);
            return iAction.DoAction();
        }

        public void SetParameters(List<BehaviorParameter> parameterList)
        {
            if (parameterList.Count > 0)
            {
                _parameterList.AddRange(parameterList);
            }
        }

        public abstract ResultType DoAction();

    }

}