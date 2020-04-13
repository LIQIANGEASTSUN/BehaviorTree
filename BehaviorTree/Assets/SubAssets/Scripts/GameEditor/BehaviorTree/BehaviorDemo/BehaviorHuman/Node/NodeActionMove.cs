using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeActionMove : NodeAction
{

    public NodeActionMove() : base()
    {

    }

    public override ResultType Execute()
    {
        base.Execute();

        if (null == HumanController.Instance)
        {
            return ResultType.Success;
        }

        Vector3 targetPos = Vector3.zero;
        if (_parameterList.Count >= 0 && _parameterList[0].parameterName.CompareTo("MoveTarget") == 0)
        {
            BehaviorParameter parameter = _parameterList[0];
            if (parameter.intValue == 0)
            {
                targetPos = HumanController.Instance.Human.KitchenPos();
            }
            else if (parameter.intValue == 1)
            {
                targetPos = HumanController.Instance.Human.DiningTablePos();
            }
            else if (parameter.intValue == 2)
            {
                targetPos = HumanController.Instance.Human.TVPos();
            }
        }

        ResultType resultType = ResultType.Running;
        if (Vector3.Distance(HumanController.Instance.Human.Position(), targetPos) < 0.5f)
        {
            return ResultType.Success;
        }

        HumanController.Instance.Human.Translate(targetPos);
        return resultType;
    }
}
