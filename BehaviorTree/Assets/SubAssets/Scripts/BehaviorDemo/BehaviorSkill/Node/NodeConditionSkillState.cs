using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeConditionSkillState : NodeCondition
{
    private static CustomIdentification _customIdentification = new CustomIdentification("技能状态节点", IDENTIFICATION.SKILL_STATE, typeof(NodeConditionSkillState), NODE_TYPE.CONDITION);

    public NodeConditionSkillState()
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
