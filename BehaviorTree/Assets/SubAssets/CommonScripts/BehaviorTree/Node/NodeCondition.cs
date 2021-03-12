using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 条件节点(叶节点)
    /// </summary>
    public class NodeCondition : NodeLeaf, ICondition
    {
        private ICondition _iCondition;
        protected ConditionParameter conditionParameter = null;
        protected List<BehaviorParameter> _parameterList;
        private List<ConditionGroup> _conditionGroupList;
        protected IConditionCheck _iconditionCheck = null;

        public NodeCondition() : base()
        {
            SetNodeType(NODE_TYPE.CONDITION);
            SetCondition(this);
            conditionParameter = new ConditionParameter();
        }

        public void SetCondition(ICondition iCondition)
        {
            _iCondition = iCondition;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            conditionParameter.Init(_conditionGroupList, _parameterList);
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
            _parameterList = parameterList;
            _conditionGroupList = conditionGroupList;
        }

        public virtual ResultType Condition()
        {
            return ResultType.Success;
        }
    }
}