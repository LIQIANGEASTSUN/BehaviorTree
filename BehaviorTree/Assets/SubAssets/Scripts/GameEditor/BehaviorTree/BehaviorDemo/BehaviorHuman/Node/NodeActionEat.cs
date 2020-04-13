using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeActionEat : NodeAction
{

    public NodeActionEat() : base()
    {

    }

    public override ResultType Execute()
    {
        base.Execute();

        if (null == HumanController.Instance)
        {
            return ResultType.Fail;
        }

        bool result = HumanController.Instance.Human.Eat();

        ResultType resultType = result ? ResultType.Running : ResultType.Success;
        return resultType;
    }

}
