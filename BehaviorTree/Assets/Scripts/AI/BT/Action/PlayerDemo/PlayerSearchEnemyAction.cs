using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

/// <summary>
/// 行为节点：搜索敌人
/// </summary>
public class PlayerSearchEnemyAction : ActionBase
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
