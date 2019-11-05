using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeConditionCustom : NodeCondition
{
    private static CustomIdentification _customIdentification = new CustomIdentification("通用条件节点", IDENTIFICATION.COMMON_CONDITION, typeof(NodeConditionCustom), NODE_TYPE.CONDITION);

    public NodeConditionCustom()
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