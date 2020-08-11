using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

/// <summary>
/// 补充能量
/// </summary>
public class PlayerReplenishEnergyAction : ActionBase
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
