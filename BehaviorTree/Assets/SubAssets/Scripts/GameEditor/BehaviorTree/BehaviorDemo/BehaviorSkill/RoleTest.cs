using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleTest : MonoBehaviour
{
    private Role _role;

    void Start()
    {
        _role = new Role();
        _role.Init();
    }

    // Update is called once per frame
    void Update()
    {
        _role.Update();
    }

}


public class Role : IAction
{
    private BehaviorAnalysis analysis = new BehaviorAnalysis();
    private NodeBase _rootNode = null;
    private List<NodeLeaf> _nodeLeafList = new List<NodeLeaf>();
    private IConditionCheck _iconditionCheck = null;

    public void Init()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/Generic");
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

    public void SetData(BehaviorTreeData behaviorTreeData)
    {
        BehaviorAnalysis analysis = new BehaviorAnalysis();
        _iconditionCheck = new ConditionCheck();
        _rootNode = analysis.Analysis(behaviorTreeData, this, ref _iconditionCheck, ref _nodeLeafList);
    }

    public void SetData(string content)
    {
        BehaviorAnalysis analysis = new BehaviorAnalysis();
        _iconditionCheck = new ConditionCheck();
        _rootNode = analysis.Analysis(content, this, ref _iconditionCheck, ref _nodeLeafList);
    }

    public ConditionCheck ConditionCheck
    {
        get { return (ConditionCheck)_iconditionCheck; }
    }

}