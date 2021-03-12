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
        private static BehaviorAnalysis _instance;
        private static object lockObj = new object();
        public static BehaviorAnalysis GetInstance()
        {
            lock (lockObj)
            {
                if (null == _instance)
                {
                    _instance = new BehaviorAnalysis();
                }
            }
            return _instance;
        }

        private LoadConfigInfoEvent _loadConfigInfoEvent;

        public void SetLoadConfigEvent(LoadConfigInfoEvent loadEvent)
        {
            _loadConfigInfoEvent = loadEvent;
        }

        public NodeBase Analysis(long aiFunction, BehaviorTreeData data, IConditionCheck iConditionCheck, Action<int> InvalidSubTreeCallBack)
        {
            int entityId = NewEntityId;
            NodeBase rootNode = AnalysisTree(entityId, aiFunction, data, iConditionCheck, InvalidSubTreeCallBack);
            return rootNode;
        }

        private NodeBase AnalysisTree(int entityId, long aiFunction, BehaviorTreeData data, IConditionCheck iConditionCheck, Action<int> InvalidSubTreeCallBack)
        {
            NodeBase rootNode = null;
            if (null == data || data.rootNodeId < 0)
            {
                ////ProDebug.Logger.LogError("数据无效");
                return rootNode;
            }

            iConditionCheck.AddParameter(data.parameterList);

            rootNode = AnalysisNode(entityId, aiFunction, data, data.rootNodeId, iConditionCheck, InvalidSubTreeCallBack);

            return rootNode;
        }

        private NodeBase AnalysisNode(int entityId, long aiFunction, BehaviorTreeData data, int nodeId, IConditionCheck iConditionCheck, Action<int> InvalidSubTreeCallBack)
        {
            NodeValue nodeValue = null;
            if (!data.nodeDic.TryGetValue(nodeId, out nodeValue))
            {
                return null;
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.SUB_TREE && nodeValue.subTreeValue > 0)
            {
                if ((aiFunction & nodeValue.subTreeValue) <= 0)
                {
                    InvalidSubTreeCallBack?.Invoke(nodeValue.id);
                    return null;
                }
            }

            UnityEngine.Profiling.Profiler.BeginSample("AnalysisNode CreateNode");
            NodeBase nodeBase = AnalysisNode(nodeValue, iConditionCheck);
            nodeBase.NodeId = nodeValue.id;
            nodeBase.EntityId = entityId;
            nodeBase.Priority = nodeValue.priority;
            UnityEngine.Profiling.Profiler.EndSample();

            if (nodeValue.NodeType == (int)NODE_TYPE.SUB_TREE && nodeValue.subTreeType == (int)SUB_TREE_TYPE.CONFIG)
            {
                if (null == _loadConfigInfoEvent)
                {
                    int a = 0;
                }
                BehaviorTreeData subTreeData = _loadConfigInfoEvent(nodeValue.subTreeConfig);
                if (null != subTreeData)
                {
                    NodeBase subTreeNode = AnalysisTree(entityId, aiFunction, subTreeData, iConditionCheck, InvalidSubTreeCallBack);
                    NodeComposite composite = (NodeComposite)(nodeBase);
                    composite.AddNode(subTreeNode);
                }
            }

            UnityEngine.Profiling.Profiler.BeginSample("AnalysisNode IsLeafNode");
            if (!IsLeafNode(nodeValue.NodeType))
            {
                NodeComposite composite = (NodeComposite)nodeBase;
                for (int i = 0; i < nodeValue.childNodeList.Count; ++i)
                {
                    int childNodeId = nodeValue.childNodeList[i];
                    NodeBase childNode = AnalysisNode(entityId, aiFunction, data, childNodeId, iConditionCheck, InvalidSubTreeCallBack);
                    if (null != childNode)
                    {
                        composite.AddNode(childNode);
                    }
                }
            }
            UnityEngine.Profiling.Profiler.EndSample();

            return nodeBase;
        }

        private bool IsLeafNode(int type)
        {
            return (type == (int)NODE_TYPE.ACTION) || (type == (int)NODE_TYPE.CONDITION);
        }

        private NodeBase AnalysisNode(NodeValue nodeValue, IConditionCheck iConditionCheck)
        {
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
                return GetSubTree();
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.SELECT)  // 选择节点
            {
                return GetSelect();
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.SEQUENCE)  // 顺序节点
            {
                return GetSequence();
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.PARALLEL)  // 并行节点
            {
                return GetParallel();
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.PARALLEL_SELECT)// 并行选择节点
            {
                return GetParallelSelect();
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.PARALLEL_ALL)   // 并行执行所有节点
            {
                return GetParallelAll();
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.IF_JUDEG)       // 判断节点
            {
                return GetIfJudge(nodeValue);
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.RANDOM)  // 随机节点
            {
                return GetRandom();
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.RANDOM_SEQUEUECE)// 随机顺序节点
            {
                return GetRandomSequence();
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.RANDOM_PRIORITY) // 随机权重节点
            {
                return GetRandomPriority();
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.DECORATOR_INVERTER)  // 修饰节点_取反
            {
                return GetInverter();
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.DECORATOR_REPEAT)  // 修饰节点_重复
            {
                return GetRepeat(nodeValue);
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.DECORATOR_RETURN_FAIL)  // 修饰节点_返回Fail
            {
                return GetReturenFail();
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.DECORATOR_RETURN_SUCCESS)  // 修饰节点_返回Success
            {
                return GetReturnSuccess();
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.DECORATOR_UNTIL_FAIL)  // 修饰节点_直到Fail
            {
                return GetUntilFail();
            }

            if (nodeValue.NodeType == (int)NODE_TYPE.DECORATOR_UNTIL_SUCCESS)  // 修饰节点_直到Success
            {
                return GetUntilSuccess();
            }

            return null;
        }

        private NodeSelect GetSelect()
        {
            NodeSelect nodeSelect = new NodeSelect();
            return nodeSelect;
        }

        private NodeSequence GetSequence()
        {
            NodeSequence nodeSequence = new NodeSequence();
            return nodeSequence;
        }

        private NodeRandom GetRandom()
        {
            NodeRandom nodeRandom = new NodeRandom();
            return nodeRandom;
        }

        private NodeRandomSequence GetRandomSequence()
        {
            NodeRandomSequence randomSequence = new NodeRandomSequence();
            return randomSequence;
        }

        private NodeParallel GetParallel()
        {
            NodeParallel nodeParallel = new NodeParallel();
            return nodeParallel;
        }

        private NodeParallelSelect GetParallelSelect()
        {
            NodeParallelSelect nodeParallelSelect = new NodeParallelSelect();
            return nodeParallelSelect;
        }

        private NodeParallelAll GetParallelAll()
        {
            NodeParallelAll nodeParallelAll = new NodeParallelAll();
            return nodeParallelAll;
        }
       
        private NodeIfJudge GetIfJudge(NodeValue nodeValue)
        {
            NodeIfJudge ifJudge = new NodeIfJudge();
            ifJudge.SetData(nodeValue.ifJudgeDataList);
            return ifJudge;
        }

        private NodeRandomPriority GetRandomPriority()
        {
            NodeRandomPriority nodeRandomPriority = new NodeRandomPriority();
            return nodeRandomPriority;
        }

        private NodeDecoratorInverter GetInverter()
        {
            NodeDecoratorInverter inverter = new NodeDecoratorInverter();
            return inverter;
        }

        private NodeDecoratorRepeat GetRepeat(NodeValue nodeValue)
        {
            NodeDecoratorRepeat repeat = new NodeDecoratorRepeat();
            repeat.SetRepeatCount(nodeValue.repeatTimes);
            return repeat;
        }

        private NodeDecoratorReturnFail GetReturenFail()
        {
            NodeDecoratorReturnFail returnFail = new NodeDecoratorReturnFail();
            return returnFail;
        }

        private NodeDecoratorReturnSuccess GetReturnSuccess()
        {
            NodeDecoratorReturnSuccess returnSuccess = new NodeDecoratorReturnSuccess();
            return returnSuccess;
        }

        private NodeDecoratorUntilFail GetUntilFail()
        {
            NodeDecoratorUntilFail untilFail = new NodeDecoratorUntilFail();
            return untilFail;
        }

        private NodeDecoratorUntilSuccess GetUntilSuccess()
        {
            NodeDecoratorUntilSuccess untilSuccess = new NodeDecoratorUntilSuccess();
            return untilSuccess;
        }

        private NodeCondition GetCondition(NodeValue nodeValue, IConditionCheck iConditionCheck)
        {
            UnityEngine.Profiling.Profiler.BeginSample("GetCondition1");
            NodeCondition condition = (NodeCondition)CustomNode.Instance.GetNode(nodeValue.identificationName);
            UnityEngine.Profiling.Profiler.EndSample();

            condition.SetData(nodeValue.parameterList, nodeValue.conditionGroupList);
            condition.SetConditionCheck(iConditionCheck);

            return condition;
        }

        private NodeAction GetAction(NodeValue nodeValue)
        {
            UnityEngine.Profiling.Profiler.BeginSample("GetAction");

            NodeAction action = (NodeAction)CustomNode.Instance.GetNode(nodeValue.identificationName);
            action.SetParameters(nodeValue.parameterList);
            UnityEngine.Profiling.Profiler.EndSample();

            return action;
        }

        private NodeSubTree GetSubTree()
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

