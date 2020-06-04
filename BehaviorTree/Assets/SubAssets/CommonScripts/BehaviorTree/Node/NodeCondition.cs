using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 条件节点(叶节点)
    /// </summary>
    public abstract class NodeCondition : NodeLeaf, ICondition
    {
        private ICondition _iCondition;
        protected ConditionParameter conditionParameter = null;
        protected List<BehaviorParameter> _parameterList = new List<BehaviorParameter>();
        protected IConditionCheck _iconditionCheck = null;

        public NodeCondition() : base(NODE_TYPE.CONDITION)
        {
            SetCondition(this);
        }

        public void SetCondition(ICondition iCondition)
        {
            _iCondition = iCondition;
        }

        public override ResultType Execute()
        {
            ResultType resultType = _iCondition.Condition();
            NodeNotify.NotifyExecute(EntityId, NodeId, resultType, Time.realtimeSinceStartup);
            return resultType;
        }

        public void SetConditionCheck(IConditionCheck iConditionCheck)
        {
            _iconditionCheck = iConditionCheck;
        }

        public void SetData(List<BehaviorParameter> parameterList, List<ConditionGroup> conditionGroupList)
        {
            if (parameterList.Count > 0)
            {
                _parameterList.AddRange(parameterList);
            }

            conditionParameter = new ConditionParameter();
            conditionParameter.SetGroup(conditionGroupList, _parameterList);
        }

        public abstract ResultType Condition();
    }
}