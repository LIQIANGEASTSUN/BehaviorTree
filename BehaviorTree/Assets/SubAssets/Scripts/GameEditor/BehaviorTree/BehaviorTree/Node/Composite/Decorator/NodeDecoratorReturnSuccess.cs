using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 修饰节点_返回Success
    /// </summary>
    public class NodeDecoratorReturnSuccess : NodeDecoratorReturnConst
    {
        public NodeDecoratorReturnSuccess() : base(NODE_TYPE.DECORATOR_RETURN_SUCCESS)
        {
            SetConstResult(ResultType.Success);
        }

        public override ResultType Execute()
        {
            NodeNotify.NotifyExecute(NodeId, Time.realtimeSinceStartup);

            NodeBase nodeRoot = nodeChildList[0];
            ResultType resultType = nodeRoot.Execute();

            return ResultType.Success;
        }

        public override void Update()
        {
            
        }
    }
}