using BehaviorTree;

/// <summary>
/// 条件节点：是否饿了
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