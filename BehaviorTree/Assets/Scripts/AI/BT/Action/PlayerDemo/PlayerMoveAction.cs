using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

/// <summary>
/// 行为节点：移动
/// </summary>
public class PlayerMoveAction : ActionBase
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
