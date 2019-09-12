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
        return ResultType.Fail;
    }

    public static CustomIdentification CustomIdentification()
    {
        return _customIdentification;
    }
}
