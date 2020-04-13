using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeActionWatchTV : NodeAction, IHuman
{
    private Human human = null;
    public NodeActionWatchTV() : base()
    {
    }

    public override ResultType DoAction()
    {
        if (null == human)
        {
            return ResultType.Fail;
        }

        bool result = human.IsHungry();
        ResultType resultType = result ? ResultType.Running : ResultType.Success;
        return resultType;
    }

    public void SetHuman(Human human)
    {
        this.human = human;
    }

}
