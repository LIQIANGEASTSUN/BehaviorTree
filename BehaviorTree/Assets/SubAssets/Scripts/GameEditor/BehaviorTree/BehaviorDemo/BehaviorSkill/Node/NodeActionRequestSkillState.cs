using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeActionRequestSkillState : NodeAction
{
    public NodeActionRequestSkillState():base()
    {

    }

    public override ResultType Execute()
    {
        NodeNotify.NotifyExecute(NodeId, Time.realtimeSinceStartup);
        bool result = iAction.DoAction(NodeId, _parameterList);
        return result ? ResultType.Success : ResultType.Fail;
    }

}
