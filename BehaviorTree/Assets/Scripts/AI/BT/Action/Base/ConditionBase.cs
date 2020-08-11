using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public abstract class ConditionBase : NodeCondition, IBTActionOwner
{
    protected BaseSprite _owner = null;

    public BaseSprite GetOwner()
    {
        return _owner;
    }

    public void SetOwner(BaseSprite owner)
    {
        _owner = owner;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override ResultType Condition()
    {
        return ResultType.Success;
    }
}
