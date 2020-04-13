using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeActionCooking : NodeAction
{

    public NodeActionCooking() : base()
    {

    }

    public override ResultType Execute()
    {
        base.Execute();

        bool result = HumanController.Instance.Human.Cooking(0.8f);

        ResultType resultType = result ? ResultType.Running : ResultType.Success;
        return resultType;
    }

}
