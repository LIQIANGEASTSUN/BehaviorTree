using BehaviorTree;

/// <summary>
/// 行为节点：做饭
/// </summary>
public class NodeActionCooking : NodeAction {
    private Student student;

    public override ResultType Execute()
    {
        // 食物足够了
        if (student.FoodEnough()) 
        {
            return ResultType.Success;
        }

        Student.intervalTime = 1.5f;

        student.Cooking(1f);
        return ResultType.Running;
    }

    public void SetStudent(Student student)
    {
        this.student = student;
    }
}