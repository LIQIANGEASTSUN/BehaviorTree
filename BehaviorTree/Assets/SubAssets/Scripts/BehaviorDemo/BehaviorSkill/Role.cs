using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role : MonoBehaviour
{

    private NodeBase _rootNode = null;
    private List<NodeLeaf> _nodeLeafList = new List<NodeLeaf>();
    private IConditionCheck _iconditionCheck = null;

    void Start()
    {
        ConditionCheck.SetParameter("IsHungry", false);

        TextAsset textAsset = Resources.Load<TextAsset>("Data/Human");
        SetData(textAsset.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetData(BehaviorTreeData behaviorTreeData)
    {
        BehaviorAnalysis analysis = new BehaviorAnalysis();
        _iconditionCheck = new ConditionCheck();
        _rootNode = analysis.Analysis(behaviorTreeData, ref _iconditionCheck, ref _nodeLeafList);
    }

    public void SetData(string content)
    {
        BehaviorAnalysis analysis = new BehaviorAnalysis();
        _iconditionCheck = new ConditionCheck();
        _rootNode = analysis.Analysis(content, ref _iconditionCheck, ref _nodeLeafList);
    }

    public ConditionCheck ConditionCheck
    {
        get { return (ConditionCheck)_iconditionCheck; }
    }

}
