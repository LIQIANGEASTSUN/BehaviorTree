using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

/// <summary>
/// 行为节点：巡逻
/// </summary>
public class PlayerPatrolAction : ActionBase
{
    private float _lastTime;
    private float _interval = 3;

    public override void OnEnter()
    {
        base.OnEnter();

        string msg = "Patrol";
        _owner.SetText(msg);
    }

    public override ResultType DoAction()
    {
        if (Time.realtimeSinceStartup - _lastTime > _interval)
        {
            _owner.ChangePatrolPos();
            _lastTime = Time.realtimeSinceStartup;
        }

        _owner.SubEnergy(0.01f);
        return ResultType.Success;
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
