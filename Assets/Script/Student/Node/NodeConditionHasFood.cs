using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

/// <summary>
/// 是否有饭(条件节点)
/// </summary>
public class NodeConditionHasFood : NodeCondition {
    private Student student;

    public override ResultType Execute()
    {
        ResultType resultType = student.HasFood() ? ResultType.Success : ResultType.Fail;
        return resultType;
    }

    public void SetStudent(Student student)
    {
        this.student = student;
    }
}
