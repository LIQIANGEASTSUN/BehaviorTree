using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

/// <summary>
/// 补充能量
/// </summary>
public class PlayerReplenishEnergyAction : ActionBase
{
    private BaseSprite _baseSprite;
    public override void OnEnter()
    {
        base.OnEnter();
        _baseSprite = _owner;
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
