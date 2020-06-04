using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeConditionCustom : NodeCondition
{

    public NodeConditionCustom()
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override ResultType Condition()
    {
        bool result = _iconditionCheck.Condition(conditionParameter);
        ResultType resultType = result ? ResultType.Success : ResultType.Fail;
        return resultType;
    }

}