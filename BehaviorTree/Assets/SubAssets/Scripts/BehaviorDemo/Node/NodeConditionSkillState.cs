using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeConditionSkillState : NodeCondition
{
    private static CustomIdentification _customIdentification = new CustomIdentification("技能状态节点", 20001, typeof(NodeConditionSkillState), NODE_TYPE.CONDITION);

    public NodeConditionSkillState()
    {

    }

    public override ResultType Execute()
    {
        bool result = _iconditionCheck.Condition(_parameterList);
        ResultType resultType = result ? ResultType.Success : ResultType.Fail;

        Debug.LogError("技能状态条件节点:" + NodeIndex + "   result:" + resultType);
        return resultType;
    }

    public static CustomIdentification CustomIdentification()
    {
        return _customIdentification;
    }
}
