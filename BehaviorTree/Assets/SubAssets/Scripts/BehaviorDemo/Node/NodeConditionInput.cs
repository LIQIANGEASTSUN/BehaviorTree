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
        bool result = _iconditionCheck.Condition(_parameterList);
        ResultType resultType = result ? ResultType.Success : ResultType.Fail;

        Debug.LogError("输入条件节点:" + NodeIndex + "   result:" + resultType);
        return resultType;
    }

    public static CustomIdentification CustomIdentification()
    {
        return _customIdentification;
    }
}
