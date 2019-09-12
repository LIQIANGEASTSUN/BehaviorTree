using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{

    public class BehaviorRunTime
    {
        private NodeBase _rootNode = null;
        private IConditionCheck iconditionCheck = null;

        public BehaviorRunTime()
        {

        }

        public void Reset(BehaviorTreeData behaviorTreeData)
        {
            BehaviorAnalysis analysis = new BehaviorAnalysis();
            iconditionCheck = new ConditionCheck();
            _rootNode = analysis.Analysis(behaviorTreeData, ref iconditionCheck);
        }

        public void Execute()
        {
            _rootNode.Execute();
        }

    }

}
