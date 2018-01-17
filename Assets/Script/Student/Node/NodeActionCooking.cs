using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeActionCooking : NodeAction {
    private Student student;

    public override ResultType Execute()
    {
        if (student.FoodEnough())
        {
            return ResultType.Success;
        }

        student.Cooking(0.5f);
        return ResultType.Running;
    }

    public void SetStudent(Student student)
    {
        this.student = student;
    }
}
