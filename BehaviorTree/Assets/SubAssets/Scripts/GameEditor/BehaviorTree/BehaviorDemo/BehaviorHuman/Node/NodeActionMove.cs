using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeActionMove : NodeAction, IHuman
{
    private Human human = null;

    public NodeActionMove() : base()
    {
    }

    public override ResultType DoAction()
    {
        if (null == human)
        {
            return ResultType.Fail;
        }

        Vector3 targetPos = Vector3.zero;
        if (_parameterList.Count >= 0 && _parameterList[0].parameterName.CompareTo("MoveTarget") == 0)
        {
            BehaviorParameter parameter = _parameterList[0];
            if (parameter.intValue == 0)
            {
                targetPos = human.KitchenPos();
            }
            else if (parameter.intValue == 1)
            {
                targetPos = human.DiningTablePos();
            }
            else if (parameter.intValue == 2)
            {
                targetPos = human.TVPos();
            }
        }

        ResultType resultType = ResultType.Running;
        if (Vector3.Distance(human.Position(), targetPos) < 0.5f)
        {
            return ResultType.Success;
        }

        human.Translate(targetPos);
        return resultType;
    }

    public void SetHuman(Human human)
    {
        this.human = human;
    }
}
