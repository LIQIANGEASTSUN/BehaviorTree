using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

/// <summary>
/// 是否饿了条件节点
/// </summary>
public class NodeConditionHungry : NodeCondition {

    private Student student;

    public override ResultType Execute()
    {
        ResultType resultType = student.IsHungry() ? ResultType.Success : ResultType.Fail;
        return resultType;
    }

    public void SetStudent(Student student)
    {
        this.student = student;
    }
}
