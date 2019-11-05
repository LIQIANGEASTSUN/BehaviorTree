using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;


namespace BehaviorTree
{

    public class BehaviorAnalysis
    {
        public BehaviorAnalysis()
        {

        }

        public NodeBase Analysis(string content, IAction iAction, IConditionCheck iConditionCheck)
        {
            BehaviorTreeData behaviorTreeData = JsonMapper.ToObject<BehaviorTreeData>(content);
            if (null == behaviorTreeData)
            {
                Debug.LogError("behaviorTreeData is null");
                return null;
            }

            iConditionCheck.AddParameter(behaviorTreeData.parameterList);
            return Analysis(behaviorTreeData, iAction, iConditionCheck);
        }

        public NodeBase Analysis(BehaviorTreeData data, IAction iAction, IConditionCheck iConditionCheck)
        {
            NodeBase rootNode = null;

            if (null == data)
            {
                Debug.LogError("数据无效");
                return rootNode;
            }

            if (data.rootNodeId < 0)
            {
                Debug.LogError("没有跟节点");
                return rootNode;
            }

            iConditionCheck.AddParameter(data.parameterList);

            Dictionary<int, NodeBase> compositeDic = new Dictionary<int, NodeBase>();
            Dictionary<int, NodeBase> allNodeDic = new Dictionary<int, NodeBase>();
            Dictionary<int, List<int>> childDic = new Dictionary<int, List<int>>();
            for (int i = 0; i < data.nodeList.Count; ++i)
            {
                NodeValue nodeValue = data.nodeList[i];
                NodeBase nodeBase = AnalysisNode(nodeValue, iAction, iConditionCheck);
                nodeBase.NodeId = nodeValue.id;

                if (!IsLeafNode(nodeValue.NodeType))
                {
                    compositeDic[nodeValue.id] = nodeBase;
                    childDic[nodeValue.id] = nodeValue.childNodeList;

                    if (data.rootNodeId == nodeValue.id)
                    {
                        rootNode = nodeBase;
                    }
                }

                //if (nodeValue.NodeType == (int)NODE_TYPE.CONDITION     // 条件节点
                //    || (nodeValue.NodeType == (int)NODE_TYPE.ACTION))  // 行为节点
                //{
                //    nodeLeafList.Add((NodeLeaf)nodeBase);
                //}

                if (null == nodeBase)
                {
                    Debug.LogError("AllNODE:" + nodeValue.id + "     " + (null != nodeBase));
                }
                allNodeDic[nodeValue.id] = nodeBase;
            }

            foreach (var kv in compositeDic)
            {
                int id = kv.Key;
                NodeComposite composite = (NodeComposite)(kv.Value);

                List<int> childList = childDic[id];
                for (int i = 0; i < childList.Count; ++i)
                {
                    int nodeId = childList[i];
                    NodeBase childNode = allNodeDic[nodeId];
                    if (null == childNode)
                    {
                        Debug.LogError("null node :" + nodeId);
                        continue;
                    }
                    composite.AddNode(childNode);
                }
            }

            return rootNode;
        }

        private bool IsLeafNode(int type)
        {
            return (type == (int)NODE_TYPE.ACTION) || (type == (int)NODE_TYPE.CONDITION);
        }

        private NodeBase AnalysisNode(NodeValue nodeValue, IAction iAction, IConditionCheck iConditionCheck)
        {
            NodeBase node = null;
            if (nodeValue.NodeType == (int)NODE_TYPE.SELECT)  // 选择节点
            {
                return GetSelect(nodeValue);
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.SEQUENCE)  // 顺序节点
            {
                return GetSequence(nodeValue);
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.RANDOM)  // 随机节点
            {
                return GetRandom(nodeValue);
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.PARALLEL)  // 并行节点
            {
                return GetParallel(nodeValue);
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.DECORATOR_INVERTER)  // 修饰节点_取反
            {
                return GetInverter(nodeValue);
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.DECORATOR_REPEAT)  // 修饰节点_重复
            {
                return GetRepeat(nodeValue);
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.DECORATOR_RETURN_FAIL)  // 修饰节点_返回Fail
            {
                return GetReturenFail(nodeValue);
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.DECORATOR_RETURN_SUCCESS)  // 修饰节点_返回Success
            {
                return GetReturnSuccess(nodeValue);
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.DECORATOR_UNTIL_FAIL)  // 修饰节点_直到Fail
            {
                return GetUntilFail(nodeValue);
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.DECORATOR_UNTIL_SUCCESS)  // 修饰节点_直到Success
            {
                return GetUntilSuccess(nodeValue);
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.CONDITION)  // 条件节点
            {
                return GetCondition(nodeValue, iConditionCheck);
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.ACTION)  // 行为节点
            {
                return GetAction(nodeValue, iAction);
            }

            return node;
        }

        public NodeSelect GetSelect(NodeValue nodeValue)
        {
            NodeSelect nodeSelect = new NodeSelect();
            return nodeSelect;
        }

        public NodeSequence GetSequence(NodeValue nodeValue)
        {
            NodeSequence nodeSequence = new NodeSequence();
            return nodeSequence;
        }

        public NodeRandom GetRandom(NodeValue nodeValue)
        {
            NodeRandom nodeRandom = new NodeRandom();
            return nodeRandom;
        }

        public NodeParallel GetParallel(NodeValue nodeValue)
        {
            NodeParallel nodeParallel = new NodeParallel();
            return nodeParallel;
        }

        public NodeDecoratorInverter GetInverter(NodeValue nodeValue)
        {
            NodeDecoratorInverter inverter = new NodeDecoratorInverter();
            return inverter;
        }

        public NodeDecoratorRepeat GetRepeat(NodeValue nodeValue)
        {
            NodeDecoratorRepeat repeat = new NodeDecoratorRepeat();
            repeat.SetRepeatCount(nodeValue.repeatTimes);
            return repeat;
        }

        public NodeDecoratorReturnFail GetReturenFail(NodeValue nodeValue)
        {
            NodeDecoratorReturnFail returnFail = new NodeDecoratorReturnFail();
            return returnFail;
        }

        public NodeDecoratorReturnSuccess GetReturnSuccess(NodeValue nodeValue)
        {
            NodeDecoratorReturnSuccess returnSuccess = new NodeDecoratorReturnSuccess();
            return returnSuccess;
        }

        public NodeDecoratorUntilFail GetUntilFail(NodeValue nodeValue)
        {
            NodeDecoratorUntilFail untilFail = new NodeDecoratorUntilFail();
            return untilFail;
        }

        public NodeDecoratorUntilSuccess GetUntilSuccess(NodeValue nodeValue)
        {
            NodeDecoratorUntilSuccess untilSuccess = new NodeDecoratorUntilSuccess();
            return untilSuccess;
        }

        public NodeCondition GetCondition(NodeValue nodeValue, IConditionCheck iConditionCheck)
        {
            NodeCondition condition = (NodeCondition)CustomNode.Instance.GetNode((IDENTIFICATION)nodeValue.identification);
            condition.SetParameters(nodeValue.parameterList);
            condition.SetConditionCheck(iConditionCheck);
            return condition;
        }

        public NodeAction GetAction(NodeValue nodeValue, IAction iAction)
        {
            NodeAction action = (NodeAction)CustomNode.Instance.GetNode((IDENTIFICATION)nodeValue.identification);
            action.SetIAction(iAction);
            action.SetParameters(nodeValue.parameterList);
            return action;
        }

    }

}

