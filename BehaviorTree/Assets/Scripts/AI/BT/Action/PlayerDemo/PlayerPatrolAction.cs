using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

/// <summary>
/// 行为节点：巡逻
/// </summary>
public class PlayerPatrolAction : ActionBase
{
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override ResultType DoAction()
    {
        return ResultType.Success;
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
