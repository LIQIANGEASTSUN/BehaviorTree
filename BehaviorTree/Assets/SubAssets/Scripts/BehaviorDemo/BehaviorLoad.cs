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

    }

    private void Load()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/AbilityGeneric");

        BehaviorAnalysis analysis = new BehaviorAnalysis();
        _rootNode = analysis.Analysis(textAsset.text);

        _conditionCheck = new ConditionCheck();

        int a = 0;
    }

}
