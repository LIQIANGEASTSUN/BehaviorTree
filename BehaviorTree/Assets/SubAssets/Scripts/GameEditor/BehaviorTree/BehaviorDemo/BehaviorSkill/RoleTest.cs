using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleTest : MonoBehaviour
{
    public static RoleTest Instance = null;
    private RolePlayer _role;
    public List<BehaviorParameter> parameterList = new List<BehaviorParameter>();
    void Start()
    {
        Instance = this;

        _role = new RolePlayer();
        _role.Init();

        UpdateCondition();
    }

    // Update is called once per frame
    void Update()
    {
        _role.Update();

        if (Input.GetKeyDown(KeyCode.A))
        {
            _role.GeneralMsg(0);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            _role.GeneralMsg(2);
        }
    }

    public void UpdateCondition()
    {
        parameterList = _role.ConditionCheck.GetAllParameter();
    }

}


public class RolePlayer : IAction
{
    private BehaviorAnalysis analysis = null;
    private NodeBase _rootNode = null;
    private IConditionCheck _iconditionCheck = null;

    public void Init()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Generic");
        SetData(textAsset.text);
    }

    // Update is called once per frame
    public void Update()
    {
        if (null != _rootNode)
        {
            _rootNode.Execute();
        }
    }

    public void GeneralMsg(int type)
    {
        _iconditionCheck.SetParameter("GenericBtn", type);
    }

    public void SetData(BehaviorTreeData behaviorTreeData)
    {
        BehaviorAnalysis analysis = new BehaviorAnalysis();
        _iconditionCheck = new ConditionCheck();
        _rootNode = analysis.Analysis(behaviorTreeData, this, _iconditionCheck);
    }

    public void SetData(string content)
    {
        BehaviorAnalysis analysis = new BehaviorAnalysis();
        _iconditionCheck = new ConditionCheck();
        _rootNode = analysis.Analysis(content, this, _iconditionCheck);
    }

    public bool DoAction(int nodeId, List<BehaviorParameter> parameterList)
    {
        bool result = true;
        for (int i = 0; i < parameterList.Count; ++i)
        {
            bool value = DoAction(nodeId, parameterList[i]);
            if (!value)
            {
                result = false;
            }
        }

        return result;
    }

    public bool DoAction(int nodeId, BehaviorParameter parameter)
    {
        Debug.LogError("Execute:" + parameter.parameterName + "   bool:" + parameter.boolValue + "    float:" + parameter.floatValue + "    int:" + parameter.intValue);

        if (parameter.parameterName.CompareTo("Skill_Change_State") == 0)
        {
            ConditionCheck.SetParameter("Skill_State", parameter.intValue);
        }

        RoleTest.Instance.UpdateCondition();
        return true;
    }

    public ConditionCheck ConditionCheck
    {
        get { return (ConditionCheck)_iconditionCheck; }
    }

}