using BehaviorTree;

/// <summary>
/// 条件节点：是否有饭
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