using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeActionMove : NodeAction
{
    private static CustomIdentification _customIdentification = new CustomIdentification("行为-移动到目标", 11002, typeof(NodeActionMove), NODE_TYPE.ACTION);

    public NodeActionMove() : base()
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
