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
        private List<NodeCondition> _conditionNodeList = new List<NodeCondition>();
        private int _entityId;
        private static int _currentDebugEntityId;

        public BehaviorTreeEntity(BehaviorTreeData data, LoadConfigInfoEvent loadEvent)
        {
            _iconditionCheck = new ConditionCheck();
            BehaviorAnalysis analysis = new BehaviorAnalysis();
            analysis.SetLoadConfigEvent(loadEvent);
            _rootNode = analysis.Analysis(data, _iconditionCheck, AddActionNode, AddConditionNode);
            if (null != _rootNode)
            {
                _entityId = _rootNode.EntityId;
            }
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

        public List<NodeCondition> ConditionNodeList
        {
            get
            {
                return _conditionNodeList;
            }
        }

        public int EntityId
        {
            get { return _entityId; }
        }

        private void AddActionNode(NodeAction nodeAction)
        {
            _actionNodeList.Add(nodeAction);
        }

        private void AddConditionNode(NodeCondition nodeCondition)
        {
            _conditionNodeList.Add(nodeCondition);
        }

        public void Execute()
        {
            if (null != _rootNode)
            {
                _rootNode.Preposition();
                ResultType resultType = _rootNode.Execute();
                _rootNode.Postposition(resultType);
            }
        }

        public void Clear()
        {
            ConditionCheck.InitParmeter();
        }

        public static int CurrentDebugEntityId
        {
            get { return _currentDebugEntityId; }
            set { _currentDebugEntityId = value; }
        }

    }

}
