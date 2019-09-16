using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeActionMove : NodeAction
{
    private static CustomIdentification _customIdentification = new CustomIdentification("行为-移动到目标", 11002, typeof(NodeActionMove), NODE_TYPE.ACTION);

    public NodeActionMove() : base()
    {

    }

    public override ResultType Execute()
    {
        NodeNotify.NotifyExecute(NodeId, Time.realtimeSinceStartup);

        Vector3 targetPos = Vector3.zero;
        if (_parameterList.Count >= 0)
        {
            BehaviorParameter parameter = _parameterList[0];
            if (parameter.parameterName.CompareTo("MoveTarget") == 0 && parameter.intValue == 0)
            {
                targetPos = HumanController.Instance.Human.KitchenPos();
            }
            else if (parameter.parameterName.CompareTo("MoveTarget") == 0 && parameter.intValue == 1)
            {
                targetPos = HumanController.Instance.Human.DiningTablePos();
            }
        }

        HumanController.Instance.Human.Translate(targetPos);

        ResultType resultType = ResultType.Running;
        if (Vector3.Distance(HumanController.Instance.Human.Position(), targetPos) < 0.5f)
        {
            return ResultType.Success;
        }

        return resultType;
    }

    public static CustomIdentification CustomIdentification()
    {
        return _customIdentification;
    }
}
