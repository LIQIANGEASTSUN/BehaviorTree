using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class BehaviorTreeEntity
    {
        private NodeBase _rootNode;
        private IConditionCheck _iconditionCheck = null;

        public BehaviorTreeEntity(string content)
        {
            _iconditionCheck = new ConditionCheck();
            BehaviorAnalysis analysis = new BehaviorAnalysis();
            _rootNode = analysis.Analysis(content, null, _iconditionCheck);
        }

        public BehaviorTreeEntity(BehaviorTreeData data)
        {
            BehaviorAnalysis analysis = new BehaviorAnalysis();
            _rootNode = analysis.Analysis(data, null, _iconditionCheck);
        }

        public ConditionCheck ConditionCheck
        {
            get { return (ConditionCheck)_iconditionCheck; }
        }

        public void SetRootNode(NodeBase rootNode)
        {
            _rootNode = rootNode;
        }

        public void Execute()
        {
            if (null != _rootNode)
            {
                _rootNode.Execute();
            }
        }
    }

}
