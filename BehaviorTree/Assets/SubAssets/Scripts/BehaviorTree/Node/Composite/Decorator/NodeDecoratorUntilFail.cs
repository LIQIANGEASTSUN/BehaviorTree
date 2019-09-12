using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviorTree
{
    /// <summary>
    /// 直到为Fail修饰节点， 一直执行节点，直到返回结果为 Fail。
    /// </summary>
    public class NodeDecoratorUntilFail : NodeDecoratorUntil
    {
        public NodeDecoratorUntilFail() : base(NODE_TYPE.DECORATOR_UNTIL_FAIL)
        {
            SetDesiredResult(ResultType.Fail);
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


