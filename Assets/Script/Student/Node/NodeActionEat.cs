using BehaviorTree;

/// <summary>
/// 行为节点：吃饭
/// </summary>
public class NodeActionEat : NodeAction {

    private Student student;

    public override ResultType Execute()
    {
        // 吃饱了
        if (student.IsFull())
        {
            return ResultType.Success;
        }

        // 吃饭增加能量，减少饭
        student.AddEnergy(2);
        student.ChangeFood(-2f);

        Student.intervalTime = 1.5f;

        return ResultType.Running;
    }

    public void SetStudent(Student student)
    {
        this.student = student;
    }
}