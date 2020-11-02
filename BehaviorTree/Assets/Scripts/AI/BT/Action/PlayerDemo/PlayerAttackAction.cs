using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

/// <summary>
/// 行为节点：攻击
/// </summary>
public class PlayerAttackAction : ActionBase
{
    private float _attackInterval = 2;
    private float _lastAttackTime;
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override ResultType DoAction()
    {
        if (Time.realtimeSinceStartup - _lastAttackTime <= _attackInterval)
        {
            return ResultType.Running;
        }
        _lastAttackTime = Time.realtimeSinceStartup;

        BulletData bulletData = new BulletData();
        bulletData.startPos = _owner.Position;
        bulletData.target = _owner.Enemy.transform;
        bulletData.speed = 1;
        bulletData.damage = 5;
        BulletManager.GetInstance().AddBullet(bulletData);

        return ResultType.Success;
    }

    public override void OnExit()
    {
        base.OnExit();
    }


}
