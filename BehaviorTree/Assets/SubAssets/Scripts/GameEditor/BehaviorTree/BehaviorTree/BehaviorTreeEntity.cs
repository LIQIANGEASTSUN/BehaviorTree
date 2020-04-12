using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class BehaviorTreeEntity
    {

        private NodeBase _rootNode;


        public BehaviorTreeEntity()
        {

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
