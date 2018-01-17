using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeActionEat : NodeAction {

    private Student student;

    public override ResultType Execute()
    {
        if (student.IsFull())
        {
            return ResultType.Success;
        }

        student.AddEnergy(0.5f);
        student.ChangeFood(-0.2f);
        return ResultType.Running;
    }

    public void SetStudent(Student student)
    {
        this.student = student;
    }
}
