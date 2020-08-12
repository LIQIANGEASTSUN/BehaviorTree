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
        bool result = SearchEnemy();
        if (!result)
        {
            return ResultType.Fail;
        }
        return ResultType.Success;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private bool SearchEnemy()
    {
        Npc enemy = _owner.Enemy;
        if (null != enemy)
        {
            return true;
        }

        GameObject npc = GameObject.Find("Npc");
        if (!npc)
        {
            return false; 
        }

        enemy = npc.GetComponent<Npc>();
        _owner.Enemy = enemy;

        return true;
    }
}
