using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;



/// <summary>
/// 行为节点：移动
/// </summary>
public class PlayerMoveAction : ActionBase
{
    private BaseSprite _baseSprite;
    private IMove _iMove;
    private TargetTypeEnum _targetType = TargetTypeEnum.ENEMY;

    public override void OnEnter()
    {
        base.OnEnter();
        _baseSprite = _owner;
        GetIMove();

        string msg = string.Format("GoTo:{0}", System.Enum.GetName(typeof(TargetTypeEnum), _targetType));
        _baseSprite.SetText(msg);
    }

    public override ResultType DoAction()
    {
        if (null == _iMove)
        {
            return ResultType.Fail;
        }

        ResultType resultType = Move();

        return resultType;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private void GetIMove()
    {
        bool result = false;
        for (int i = 0; i < _parameterList.Count; ++i)
        {
            BehaviorParameter parameter = _parameterList[i];
            if (parameter.parameterName.CompareTo(BTConstant.TargetType) == 0)
            {
                _targetType = (TargetTypeEnum)parameter.intValue;
                result = true;
            }
        }

        if (!result)
        {
            return;
        }

        _iMove = _owner.GetIMove(_targetType);
    }

    private ResultType Move()
    {
        if (null == _iMove)
        {
            return ResultType.Fail;   
        }

        float speed = 0;
        Vector3 targetPos = Vector3.zero;
        float distance = 0;
        _iMove.Move(ref speed, ref targetPos, ref distance);
        if (Vector3.Distance(_baseSprite.Position, targetPos) <= distance)
        {
            return ResultType.Success;
        }

        _baseSprite.Position = Vector3.MoveTowards(_baseSprite.Position, targetPos, speed * Time.deltaTime);

        if (Vector3.Distance(_baseSprite.Position, targetPos) > distance)
        {
            return ResultType.Running;
        }

        return ResultType.Success;
    }

}
