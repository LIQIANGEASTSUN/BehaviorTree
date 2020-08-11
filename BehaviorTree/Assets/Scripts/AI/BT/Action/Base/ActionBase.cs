using System;
using System.Collections.Generic;
using BehaviorTree;

public abstract class ActionBase : NodeAction, IBTActionOwner
{
    protected BaseSprite _owner = null;

    public virtual void SetOwner(BaseSprite owner)
    {
        _owner = owner;
    }

    public virtual BaseSprite GetOwner()
    {
        return _owner;
    }

    public override ResultType DoAction()
    {
        return ResultType.Success;
    }

}
