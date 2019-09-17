using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeActionEat : NodeAction
{
    private static CustomIdentification _customIdentification = new CustomIdentification("行为-吃饭", 11001, typeof(NodeActionEat), NODE_TYPE.ACTION);

    public NodeActionEat() : base()
    {

    }

    public override ResultType Execute()
    {
        NodeNotify.NotifyExecute(NodeId, Time.realtimeSinceStartup);

        if (null == HumanController.Instance)
        {
            return ResultType.Fail;
        }

        bool result = HumanController.Instance.Human.Eat();

        ResultType resultType = result ? ResultType.Running : ResultType.Success;
        return resultType;
    }

    public static CustomIdentification CustomIdentification()
    {
        return _customIdentification;
    }
}
