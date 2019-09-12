using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class BehaviorLoad : MonoBehaviour
{
    private NodeBase _rootNode = null;
    private ConditionCheck _conditionCheck = null;
    // Start is called before the first frame update
    void Start()
    {
        Load();
    }

    // Update is called once per frame
    void Update()
    {
     
        if (Input.GetKeyDown(KeyCode.A))
        {
            _rootNode.Execute();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            BehaviorParameter parameter = new BehaviorParameter();
            parameter.parameterType = (int)BehaviorParameterType.Bool;
            parameter.parameterName = "DD";
            parameter.boolValue = false;
            parameter.compare = (int)BehaviorCompare.NOT_EQUAL;

            bool result = _conditionCheck.CompareParameter(parameter);
            Debug.LogError(result);
        }
    }

    private void Load()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/AbilityGeneric");

        BehaviorAnalysis analysis = new BehaviorAnalysis();

        _conditionCheck = new ConditionCheck();
        _rootNode = analysis.Analysis(textAsset.text, ref _conditionCheck);

        int a = 0;
    }

}
