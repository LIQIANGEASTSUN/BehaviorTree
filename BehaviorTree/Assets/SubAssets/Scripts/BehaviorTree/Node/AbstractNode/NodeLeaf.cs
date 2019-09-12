using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// 叶节点
    /// </summary>
    [System.Serializable]
    public abstract class NodeLeaf : NodeBase
    {
        protected List<BehaviorParameter> _parameterList = new List<BehaviorParameter>();
        public NodeLeaf(NODE_TYPE nodeType):base(nodeType)
        { }

        public void SetParameters(List<BehaviorParameter> parameterList)
        {
            if (parameterList.Count > 0)
            {
                _parameterList.AddRange(parameterList);
            }
        }
    }
}