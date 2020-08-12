using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public interface IMove
{

    void Move(ref float speed, ref Vector3 position);

}

/// <summary>
/// 行为节点：移动
/// </summary>
public class PlayerMoveAction : ActionBase
{
    private IMove _iMove;

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override ResultType DoAction()
    {
        if (null == _iMove)
        {
            return ResultType.Fail;
        }



        return ResultType.Success;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private void GetIMove()
    {
        int targetType = 0;
        bool result = false;
        for (int i = 0; i < _parameterList.Count; ++i)
        {
            BehaviorParameter parameter = _parameterList[i];
            if (parameter.parameterName.CompareTo(BTConstant.TargetType) == 0)
            {
                targetType = parameter.intValue;
                result = true;
            }
        }

        if (!result)
        {
            return;
        }

        _iMove = _owner.GetIMove((TargetTypeEnum)targetType);
    }

}
