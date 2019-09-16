using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeConditionInput : NodeCondition
{
    private static CustomIdentification _customIdentification = new CustomIdentification("输入节点", 20000, typeof(NodeConditionInput), NODE_TYPE.CONDITION);

    public NodeConditionInput()
    {

    }

    public override ResultType Execute()
    {
        NodeNotify.NotifyExecute(NodeId, Time.realtimeSinceStartup);

        bool result = _iconditionCheck.Condition(_parameterList);
        ResultType resultType = result ? ResultType.Success : ResultType.Fail;

        return resultType;
    }

    public static CustomIdentification CustomIdentification()
    {
        return _customIdentification;
    }
}
