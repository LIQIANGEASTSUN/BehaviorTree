using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class BehaviorTreeEntity
    {
        private NodeBase _rootNode;
        private IConditionCheck _iconditionCheck = null;
        private List<NodeAction> _actionNodeList = new List<NodeAction>();

        public BehaviorTreeEntity(string content)
        {
            _iconditionCheck = new ConditionCheck();
            BehaviorAnalysis analysis = new BehaviorAnalysis();
            _rootNode = analysis.Analysis(content, _iconditionCheck, AddActionNode);
        }

        public BehaviorTreeEntity(BehaviorTreeData data)
        {
            _iconditionCheck = new ConditionCheck();
            BehaviorAnalysis analysis = new BehaviorAnalysis();
            _rootNode = analysis.Analysis(data, _iconditionCheck, AddActionNode);
        }

        public ConditionCheck ConditionCheck
        {
            get { return (ConditionCheck)_iconditionCheck; }
        }

        public List<NodeAction> ActionNodeList
        {
            get
            {
                return _actionNodeList;
            }
        }

        private void AddActionNode(NodeAction nodeAction)
        {
            _actionNodeList.Add(nodeAction);
        }

        public void Execute()
        {
            if (null != _rootNode)
            {
                _rootNode.Execute();
            }
        }

        public void Clear()
        {
            ConditionCheck.InitParmeter();
        }
    }

}
