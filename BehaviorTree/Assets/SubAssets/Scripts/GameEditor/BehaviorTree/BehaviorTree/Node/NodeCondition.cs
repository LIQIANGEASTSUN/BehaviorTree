using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// 条件节点(叶节点)
    /// </summary>
    public class NodeCondition : NodeLeaf
    {
        protected ConditionParameter conditionParameter = null;
        protected List<BehaviorParameter> _parameterList = new List<BehaviorParameter>();
        protected IConditionCheck _iconditionCheck = null;

        public NodeCondition() : base(NODE_TYPE.CONDITION)
        {
        }

        public override ResultType Execute()
        {
            return ResultType.Success;
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

    }
}