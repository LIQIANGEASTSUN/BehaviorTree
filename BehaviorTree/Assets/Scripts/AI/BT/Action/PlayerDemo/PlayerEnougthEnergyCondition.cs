using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

/// <summary>
/// 条件节点：能量是否足够
/// </summary>
public class PlayerEnougthEnergyCondition : ConditionBase
{
    private float _energyMin = 0;

    public override void OnEnter()
    {
        base.OnEnter();
        GetEnergyMin();
    }

    public override ResultType Condition()
    {
        ResultType resultType = ResultType.Fail;

        float energy = 0;
        bool result = _owner.BTBase.GetParameterValue(BTConstant.Energy, ref energy);
        if (!result)
        {
            return resultType;
        }

        resultType = energy > _energyMin ? ResultType.Success : ResultType.Fail;

        return resultType;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private void GetEnergyMin()
    {
        for (int i = 0; i < _parameterList.Count; ++i)
        {
            BehaviorParameter parameter = _parameterList[i];
            if (parameter.parameterName.CompareTo(BTConstant.EnergyMin) == 0)
            {
                _energyMin = parameter.floatValue;
            }
        }
    }

}
