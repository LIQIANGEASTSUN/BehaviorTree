using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

/// <summary>
/// 行为节点：攻击
/// </summary>
public class PlayerAttackAction : ActionBase
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
