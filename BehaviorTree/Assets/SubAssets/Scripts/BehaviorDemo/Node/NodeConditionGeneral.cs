using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeConditionGeneral : NodeCondition
{
    private static CustomIdentification _customIdentification = new CustomIdentification("通用条件节点", 20002, typeof(NodeConditionGeneral), NODE_TYPE.CONDITION);

    public NodeConditionGeneral()
    {

    }

    public override ResultType Execute()
    {
        bool result = _iconditionCheck.Condition(_parameterList);
        ResultType resultType = result ? ResultType.Success : ResultType.Fail;

        Debug.LogError("通用条件节点:" + NodeIndex + "   result:" + resultType);
        return resultType;
    }

    public static CustomIdentification CustomIdentification()
    {
        return _customIdentification;
    }
}