using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// 行为节点(叶节点)
    /// </summary>
    public abstract class NodeAction : NodeLeaf
    {
        protected IAction iAction;
        protected List<BehaviorParameter> _parameterList = new List<BehaviorParameter>();

        public NodeAction() : base(NODE_TYPE.ACTION)
        {
        }

        public void SetIAction(IAction iA)
        {
            iAction = iA;
        }

        public void SetParameters(List<BehaviorParameter> parameterList)
        {
            if (parameterList.Count > 0)
            {
                _parameterList.AddRange(parameterList);
            }
        }



    }

}