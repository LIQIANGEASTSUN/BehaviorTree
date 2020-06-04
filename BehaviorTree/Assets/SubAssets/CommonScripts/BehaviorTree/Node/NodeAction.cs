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
            ResultType resultType = ResultType.Fail;
            if (!Application.isPlaying)
            {
                // 编辑器下预览用
                resultType = ResultType.Running;
            }
            else
            {
                resultType = iAction.DoAction();
            }

            NodeNotify.NotifyExecute(EntityId, NodeId, resultType, Time.realtimeSinceStartup);
            return resultType;
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