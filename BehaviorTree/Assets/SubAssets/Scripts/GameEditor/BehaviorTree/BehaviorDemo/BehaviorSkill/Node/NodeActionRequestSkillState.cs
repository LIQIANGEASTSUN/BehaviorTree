using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeActionRequestSkillState : NodeAction
{
    private static CustomIdentification _customIdentification = new CustomIdentification("切换状态节点", IDENTIFICATION.SKILL_STATE_REQUEST, typeof(NodeActionRequestSkillState), NODE_TYPE.ACTION);

    public NodeActionRequestSkillState():base()
    {

    }

    public override ResultType Execute()
    {
        NodeNotify.NotifyExecute(NodeId, Time.realtimeSinceStartup);
        bool result = iAction.DoAction(NodeId, _parameterList);
        return result ? ResultType.Success : ResultType.Fail;
    }

    public static CustomIdentification CustomIdentification()
    {
        return _customIdentification;
    }
}
