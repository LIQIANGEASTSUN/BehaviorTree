using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeActionCooking : NodeAction
{
    private static CustomIdentification _customIdentification = new CustomIdentification("行为-做饭", IDENTIFICATION.COOKING, typeof(NodeActionCooking), NODE_TYPE.ACTION);

    public NodeActionCooking() : base()
    {

    }

    public override ResultType Execute()
    {
        NodeNotify.NotifyExecute(NodeId, Time.realtimeSinceStartup);

        bool result = HumanController.Instance.Human.Cooking(0.3f);

        ResultType resultType = result ? ResultType.Running : ResultType.Success;
        return resultType;
    }

    public static CustomIdentification CustomIdentification()
    {
        return _customIdentification;
    }
}
