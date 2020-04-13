using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeActionCooking : NodeAction, IHuman
{
    private Human human = null;

    public NodeActionCooking() : base()
    {
    }

    public override ResultType DoAction()
    {
        if (null == human)
        {
            return ResultType.Fail;
        }

        bool result = human.Cooking(0.8f);
        ResultType resultType = result ? ResultType.Running : ResultType.Success;
        return resultType;
    }

    public void SetHuman(Human human)
    {
        this.human = human;
    }
}
