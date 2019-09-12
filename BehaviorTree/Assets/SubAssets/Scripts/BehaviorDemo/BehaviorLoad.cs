using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class BehaviorLoad : MonoBehaviour
{
    private NodeBase _rootNode = null;
    private IConditionCheck iconditionCheck = null;
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
        iconditionCheck = new ConditionCheck();
        _rootNode = analysis.Analysis(textAsset.text, ref iconditionCheck);

        int a = 0;
    }

}
