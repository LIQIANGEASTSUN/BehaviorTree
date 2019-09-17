using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeActionWatchTV : NodeAction
{
    private static CustomIdentification _customIdentification = new CustomIdentification("行为-看电视", 11003, typeof(NodeActionWatchTV), NODE_TYPE.ACTION);

    public NodeActionWatchTV() : base()
    {

    }

    public override ResultType Execute()
    {
        NodeNotify.NotifyExecute(NodeId, Time.realtimeSinceStartup);
        if (null == HumanController.Instance || null == HumanController.Instance.Human)
        {
            return ResultType.Fail;
        }

        bool result = HumanController.Instance.Human.IsHungry();

        ResultType resultType = result ? ResultType.Running : ResultType.Success;
        return resultType;
    }

    public static CustomIdentification CustomIdentification()
    {
        return _customIdentification;
    }
}
