using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 直到为Success修饰节点， 一直执行节点，直到返回结果为 Success。
    /// </summary>
    public class NodeDecoratorUntilSuccess : NodeDecoratorUntil
    {
        public NodeDecoratorUntilSuccess() : base(NODE_TYPE.DECORATOR_UNTIL_SUCCESS)
        {
            SetDesiredResult(ResultType.Success);
        }

        public override ResultType Execute()
        {
            return base.Execute();
        }

        public override void Update()
        {
            
        }
    }
}