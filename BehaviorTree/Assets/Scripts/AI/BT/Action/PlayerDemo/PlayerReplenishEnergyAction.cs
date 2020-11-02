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

        string msg = "ReplenishEnergy";
        _owner.SetText(msg);
    }

    public override ResultType DoAction()
    {
        float value = EnergyStation.GetInstance().Execute();
        bool isDone = _baseSprite.ReplenishEnergy(value);

        ResultType resultType = isDone ? ResultType.Success : ResultType.Running;

        return resultType;
    }

    public override void OnExit()
    {
        base.OnExit();
        _baseSprite.BTBase.UpdateParameter(BTConstant.Energy, _baseSprite.Energy());
    }
}
