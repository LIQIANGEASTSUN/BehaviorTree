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
        NodeNotify.NotifyExecute(NodeId, Time.realtimeSinceStartup);

        bool result = HumanController.Instance.Human.Cooking(0.8f);

        ResultType resultType = result ? ResultType.Running : ResultType.Success;
        return resultType;
    }

}
