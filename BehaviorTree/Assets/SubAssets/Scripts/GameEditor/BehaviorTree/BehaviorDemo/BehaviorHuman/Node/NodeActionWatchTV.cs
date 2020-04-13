using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeActionWatchTV : NodeAction
{

    public NodeActionWatchTV() : base()
    {

    }

    public override ResultType Execute()
    {
        base.Execute();

        if (null == HumanController.Instance || null == HumanController.Instance.Human)
        {
            return ResultType.Fail;
        }

        bool result = HumanController.Instance.Human.IsHungry();

        ResultType resultType = result ? ResultType.Running : ResultType.Success;
        return resultType;
    }

}
