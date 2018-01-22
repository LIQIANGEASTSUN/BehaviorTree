using UnityEngine;
using BehaviorTree;

/// <summary>
/// 行为节点：走向目标
/// </summary>
public class NodeActionMove : NodeAction {
    private Student student;
    // Student transform
    private Transform tr = null;
    // 朝目标移动
    private string targetName = string.Empty;

    private float speed = 10;

    public override ResultType Execute()
    {
        GameObject target = GetTarget();
        if (target == null)
        {
            return ResultType.Fail;
        }

        bool arrive = Move(target.transform.position);

        ResultType resultType = (arrive ? ResultType.Success : ResultType.Running);

        return resultType;
    }

    private bool Move(Vector3 targetPos)
    {
        Vector3 moveDir = targetPos - tr.position;
        moveDir = moveDir.normalized;

        tr.Translate(moveDir * Time.deltaTime * speed);

        Student.intervalTime = 0.3f;

        float distance = Vector3.Distance(tr.position, targetPos);

        return distance <= 1;
    }

    private GameObject GetTarget()
    {
        return GameObject.Find(targetName);
    }

    public void SetTr(Transform tr)
    {
        this.tr = tr;
    }

    public void SetTarget(string name)
    {
        targetName = name;
    }

    public void SetStudent(Student student)
    {
        this.student = student;
    }
}