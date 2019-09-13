using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 修饰节点_返回Fail
    /// </summary>
    public class NodeDecoratorReturnFail : NodeDecoratorReturnConst
    {
        public NodeDecoratorReturnFail() : base(NODE_TYPE.DECORATOR_RETURN_FAIL)
        {
            SetConstResult(ResultType.Success);
        }

        public override ResultType Execute()
        {
            NodeNotify.NotifyExecute(NodeId, Time.realtimeSinceStartup);

            NodeBase nodeRoot = nodeChildList[0];
            ResultType resultType = nodeRoot.Execute();

            return ResultType.Fail;
        }

        public override void Update()
        {

        }
    }
}