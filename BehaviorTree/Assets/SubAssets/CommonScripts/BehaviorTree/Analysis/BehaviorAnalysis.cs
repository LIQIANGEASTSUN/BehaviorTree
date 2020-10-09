using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

namespace BehaviorTree
{
    public delegate BehaviorTreeData LoadConfigInfoEvent(string fileName);

    public class BehaviorAnalysis
    {
        private LoadConfigInfoEvent _loadConfigInfoEvent;
        private Dictionary<string, BehaviorTreeData> _subTreeDataDic = new Dictionary<string, BehaviorTreeData>();

        public BehaviorAnalysis()
        {

        }

        public void SetLoadConfigEvent(LoadConfigInfoEvent loadEvent)
        {
            _loadConfigInfoEvent = loadEvent;
        }

        public NodeBase Analysis(BehaviorTreeData data, IConditionCheck iConditionCheck, Action<NodeAction> ActionNodeCallBack, Action<NodeCondition> ConditionNodeCallBack)
        {
            NodeBase rootNode = null;
            if (null == data)
            {
                //ProDebug.Logger.LogError("数据无效");
                return rootNode;
            }

            _subTreeDataDic.Clear();

            List<BehaviorParameter> parameteList = new List<BehaviorParameter>();
            parameteList.AddRange(data.parameterList);

            for (int i = 0; i < data.nodeList.Count; ++i)
            {
                NodeValue nodeValue = data.nodeList[i];
                if (nodeValue.NodeType == (int)NODE_TYPE.SUB_TREE && nodeValue.subTreeType == (int)SUB_TREE_TYPE.CONFIG)
                {
                    if (null != _loadConfigInfoEvent)
                    {
                        BehaviorTreeData subTreeData = _loadConfigInfoEvent(nodeValue.subTreeConfig);
                        if (null == subTreeData)
                        {
                            Debug.LogError("SubTreeData is null:" + nodeValue.subTreeConfig);
                            continue;
                        }
                        _subTreeDataDic[nodeValue.subTreeConfig] = subTreeData;

                        parameteList.AddRange(subTreeData.parameterList);
                    }
                }
            }

            iConditionCheck.AddParameter(parameteList);

            int entityId = NewEntityId;
            rootNode = Analysis(entityId, data, iConditionCheck, ActionNodeCallBack, ConditionNodeCallBack);

            _subTreeDataDic.Clear();
            return rootNode;
        }

        private NodeBase Analysis(int entityId, BehaviorTreeData data, IConditionCheck iConditionCheck, Action<NodeAction> ActionNodeCallBack, Action<NodeCondition> ConditionNodeCallBack)
        {
            NodeBase rootNode = null;

            if (null == data)
            {
                //ProDebug.Logger.LogError("数据无效");
                return rootNode;
            }

            if (data.rootNodeId < 0)
            {
                //ProDebug.Logger.LogError("没有跟节点");
                return rootNode;
            }

            Dictionary<int, NodeBase> compositeDic = new Dictionary<int, NodeBase>();
            Dictionary<int, NodeBase> allNodeDic = new Dictionary<int, NodeBase>();
            Dictionary<int, List<int>> childDic = new Dictionary<int, List<int>>();
            for (int i = 0; i < data.nodeList.Count; ++i)
            {
                NodeValue nodeValue = data.nodeList[i];
                NodeBase nodeBase = AnalysisNode(nodeValue, iConditionCheck);
                nodeBase.NodeId = nodeValue.id;
                nodeBase.EntityId = entityId;
                nodeBase.Priority = nodeValue.priority;

                if (nodeValue.NodeType == (int)NODE_TYPE.SUB_TREE && nodeValue.subTreeType == (int)SUB_TREE_TYPE.CONFIG)
                {
                    BehaviorTreeData subTreeData = null;
                    if (_subTreeDataDic.TryGetValue(nodeValue.subTreeConfig, out subTreeData))
                    {
                        NodeBase subTreeNode = Analysis(entityId, subTreeData, iConditionCheck, ActionNodeCallBack, ConditionNodeCallBack);
                        NodeComposite composite = (NodeComposite)(nodeBase);
                        composite.AddNode(subTreeNode);
                    }
                }

                if (!IsLeafNode(nodeValue.NodeType))
                {
                    compositeDic[nodeValue.id] = nodeBase;
                    childDic[nodeValue.id] = nodeValue.childNodeList;

                    if (data.rootNodeId == nodeValue.id)
                    {
                        rootNode = nodeBase;
                    }
                }

                if (nodeValue.NodeType == (int)NODE_TYPE.ACTION && null != ActionNodeCallBack)
                {
                    ActionNodeCallBack((NodeAction)nodeBase);
                }

                if (nodeValue.NodeType == (int)NODE_TYPE.CONDITION && null != ConditionNodeCallBack)
                {
                    ConditionNodeCallBack((NodeCondition)nodeBase);
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
                        //ProDebug.Logger.LogError("null node :" + nodeId);
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

        private NodeBase AnalysisNode(NodeValue nodeValue, IConditionCheck iConditionCheck)
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

            if (nodeValue.NodeType == (int)NODE_TYPE.RANDOM_SEQUEUECE)// 随机顺序节点
            {
                return GetRandomSequence(nodeValue);
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.RANDOM_PRIORITY) // 随机权重节点
            {
                return GetRandomPriority(nodeValue);
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.PARALLEL)  // 并行节点
            {
                return GetParallel(nodeValue);
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.PARALLEL_SELECT)// 并行选择节点
            {
                return GetParallelSelect(nodeValue);
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.PARALLEL_ALL)   // 并行执行所有节点
            {
                return GetParallelAll(nodeValue);
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.IF_JUDEG)       // 判断节点
            {
                return GetIfJudge(nodeValue);
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
                return GetAction(nodeValue);
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.SUB_TREE) // 子树
            {
                return GetSubTree(nodeValue);
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

        public NodeRandomSequence GetRandomSequence(NodeValue nodeValue)
        {
            NodeRandomSequence randomSequence = new NodeRandomSequence();
            return randomSequence;
        }

        public NodeParallel GetParallel(NodeValue nodeValue)
        {
            NodeParallel nodeParallel = new NodeParallel();
            return nodeParallel;
        }

        public NodeParallelSelect GetParallelSelect(NodeValue nodeValue)
        {
            NodeParallelSelect nodeParallelSelect = new NodeParallelSelect();
            return nodeParallelSelect;
        }

        public NodeParallelAll GetParallelAll(NodeValue nodeValue)
        {
            NodeParallelAll nodeParallelAll = new NodeParallelAll();
            return nodeParallelAll;
        }
       
        private NodeIfJudge GetIfJudge(NodeValue nodeValue)
        {
            NodeIfJudge ifJudge = new NodeIfJudge();
            ifJudge.SetData(nodeValue.ifJudgeDataList.ToArray());
            return ifJudge;
        }

        public NodeRandomPriority GetRandomPriority(NodeValue nodeValue)
        {
            NodeRandomPriority nodeRandomPriority = new NodeRandomPriority();
            return nodeRandomPriority;
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
            NodeCondition condition = (NodeCondition)CustomNode.Instance.GetNode(nodeValue.identificationName);
            condition.SetData(nodeValue.parameterList, nodeValue.conditionGroupList);
            condition.SetConditionCheck(iConditionCheck);
            return condition;
        }

        public NodeAction GetAction(NodeValue nodeValue)
        {
            NodeAction action = (NodeAction)CustomNode.Instance.GetNode(nodeValue.identificationName);
            if (null == action)
            {
                Debug.LogError(nodeValue.id);
            }
            action.SetParameters(nodeValue.parameterList);
            return action;
        }

        public NodeSubTree GetSubTree(NodeValue nodeValue)
        {
            NodeSubTree subTree = new NodeSubTree();
            return subTree;
        }

        private static int _entityId = 0;
        private int NewEntityId
        {
            get { return ++_entityId; }
        }

    }

}

