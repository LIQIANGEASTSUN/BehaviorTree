using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// if 判断节点
    /// </summary>
    public class NodeIfJudge : NodeComposite
    {
        private NodeBase lastRunningNode;
        private List<IfJudgeData> _ifJudgeDataList;
        public static string descript = "if判断节点,最多只能有三个子节点(A、B、C)，第\n" +
                                        "一个节点A判断(只能返回Success、Fail),B、C节\n" +
                                        "点分别配置执行条件（Success、Fail），当A 返回\n" +
                                        "Success 执行 B节点、当A 返回 Fail 时，执行 C 节点";

        public NodeIfJudge() : base(NODE_TYPE.IF_JUDEG)
        { }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();

            if (null != lastRunningNode)
            {
                lastRunningNode.Postposition(ResultType.Fail);
                lastRunningNode = null;
            }
        }

        /// <summary>
        /// NodeDescript.GetDescript(NODE_TYPE);
        /// </summary>
        public override ResultType Execute()
        {
            if (nodeChildList.Count <= 0)
            {
                return ResultType.Fail;
            }

            ResultType resultType = ResultType.Fail;
            // ifNode
            {
                NodeBase ifNode = nodeChildList[0];
                resultType = ExecuteNode(ifNode);
                if (resultType == ResultType.Running)
                {
                    return ResultType.Fail;
                }

            }

            {
                NodeBase nodeBase = GetBaseNode(resultType);
                if (null != lastRunningNode && lastRunningNode.NodeId != nodeBase.NodeId)
                {
                    lastRunningNode.Postposition(ResultType.Fail);
                    lastRunningNode = null;
                }
                resultType = ExecuteNode(nodeBase);
            }

            return resultType;
        }

        private ResultType ExecuteNode(NodeBase nodeBase)
        {
            ResultType resultType = ResultType.Fail;
            if (null != nodeBase)
            {
                nodeBase.Preposition();
                resultType = nodeBase.Execute();
                nodeBase.Postposition(resultType);
            }

            if (resultType == ResultType.Running)
            {
                lastRunningNode = nodeBase;
            }

            NodeNotify.NotifyExecute(EntityId, NodeId, resultType, Time.realtimeSinceStartup);
            return resultType;
        }

        public void SetData(List<IfJudgeData> ifJudgeDataList)
        {
            _ifJudgeDataList = ifJudgeDataList;
            
        }

        private IfJudgeData GetData(int nodeId)
        {
            for (int i = 0; i < _ifJudgeDataList.Count; ++i)
            {
                IfJudgeData data = _ifJudgeDataList[i];
                if (data.nodeId == nodeId)
                {
                    return data;
                }
            }
            return null;
        }

        private NodeBase GetBaseNode(ResultType resultType)
        {
            for (int i = 0; i < nodeChildList.Count; ++i)
            {
                NodeBase node = nodeChildList[i];
                IfJudgeData judgeData = GetData(node.NodeId);
                if (judgeData.ifJudegType == (int)NodeIfJudgeEnum.IF)
                {
                    continue;
                }

                if (judgeData.ifResult == (int)resultType)
                {
                    return node;
                }
            }

            return null;
        }

    }
}


