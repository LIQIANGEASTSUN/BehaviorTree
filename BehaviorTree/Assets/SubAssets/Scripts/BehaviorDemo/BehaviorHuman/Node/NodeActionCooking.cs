using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeActionCooking : NodeAction
{
    private static CustomIdentification _customIdentification = new CustomIdentification("行为-做饭", 11000, typeof(NodeActionCooking), NODE_TYPE.ACTION);

    public NodeActionCooking() : base()
    {

    }

    public override ResultType Execute()
    {
        NodeNotify.NotifyExecute(NodeId, Time.realtimeSinceStartup);

        HumanController.Instance.Human.AddFood(1);

        ResultType resultType = ResultType.Running;
        if (HumanController.Instance.Human.FoodEnougth())
        {
            resultType = ResultType.Success;
        }

        return resultType;
    }

    public static CustomIdentification CustomIdentification()
    {
        return _customIdentification;
    }
}
