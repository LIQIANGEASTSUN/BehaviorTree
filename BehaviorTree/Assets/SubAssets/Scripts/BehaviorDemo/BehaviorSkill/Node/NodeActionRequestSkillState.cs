using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeActionRequestSkillState : NodeAction
{
    private static CustomIdentification _customIdentification = new CustomIdentification("切换状态节点", 10000, typeof(NodeActionRequestSkillState), NODE_TYPE.ACTION);

    public NodeActionRequestSkillState():base()
    {

    }

    public override ResultType Execute()
    {
        NodeNotify.NotifyExecute(NodeId, Time.realtimeSinceStartup);
        return ResultType.Fail;
    }

    public static CustomIdentification CustomIdentification()
    {
        return _customIdentification;
    }
}
