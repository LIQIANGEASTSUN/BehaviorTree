using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeActionEat : NodeAction, IHuman
{
    private Human human = null;

    public NodeActionEat() : base()
    {
    }

    public override ResultType DoAction()
    {
        if (null == human)
        {
            return ResultType.Fail;
        }

        bool result = human.Eat();
        ResultType resultType = result ? ResultType.Running : ResultType.Success;
        return resultType;
    }

    public void SetHuman(Human human)
    {
        this.human = human;
    }
}
