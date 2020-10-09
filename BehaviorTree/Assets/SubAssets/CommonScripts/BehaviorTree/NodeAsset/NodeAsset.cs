using UnityEngine;
using BehaviorTree;
using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    public class BehaviorTreeData
    {
        public string fileName = string.Empty;
        public int rootNodeId = -1;
        public List<NodeValue> nodeList = new List<NodeValue>();
        public List<BehaviorParameter> parameterList = new List<BehaviorParameter>();
        public string descript = string.Empty;
    }

    public class NodeValue
    {
        public string identificationName = string.Empty;
        public int id = 0;
        public bool isRootNode = false;                    // 根节点
        public int NodeType = (int)(NODE_TYPE.SELECT);     // 节点类型 // NODE_TYPE NodeType = NODE_TYPE.SELECT;
        public int priority = 1;                          // 权重
        public int parentNodeID = -1;                      // 父节点
        public List<int> childNodeList = new List<int>();  // 子节点集合
        public List<BehaviorParameter> parameterList = new List<BehaviorParameter>();
        public int repeatTimes = 0;
        public string nodeName = string.Empty;
        public string descript = string.Empty;
        public string function = string.Empty;
        public List<ConditionGroup> conditionGroupList = new List<ConditionGroup>();

        #region IfJudgeData
        public List<IfJudgeData> ifJudgeDataList = new List<IfJudgeData>();
        #endregion

        #region SubTree
        public int parentSubTreeNodeId = -1;
        public bool subTreeEntry = false;
        public int subTreeType = (int)SUB_TREE_TYPE.NORMAL;
        public string subTreeConfig = string.Empty;
        #endregion

        #region 编辑器用
        public RectT position = new RectT(); // 节点位置（编辑器显示使用）
        public bool moveWithChild = false;  // 同步移动子节点
        #endregion

        public NodeValue Clone()
        {
            NodeValue nodeValue = new NodeValue();

            nodeValue.identificationName = identificationName;
            nodeValue.id = this.id;
            nodeValue.isRootNode = isRootNode;                    // 根节点
            nodeValue.NodeType = NodeType;     // 节点类型 // NODE_TYPE NodeType = NODE_TYPE.SELECT;
            nodeValue.priority = priority;                          // 权重
            nodeValue.parentNodeID = parentNodeID;                      // 父节点
            nodeValue.childNodeList.AddRange(childNodeList);  // 子节点集合

            for (int i = 0; i < parameterList.Count; ++i)
            {
                nodeValue.parameterList.Add(parameterList[i].Clone());
            }

            nodeValue.repeatTimes = repeatTimes;
            nodeValue.nodeName = nodeName;
            nodeValue.descript = descript;
            nodeValue.function = function;

            for (int i = 0; i < conditionGroupList.Count; ++i)
            {
                nodeValue.conditionGroupList.Add(conditionGroupList[i].Clone());
            }

            for (int i = 0; i < ifJudgeDataList.Count; ++i)
            {
                nodeValue.ifJudgeDataList.Add(ifJudgeDataList[i].Clone());
            }

            #region SubTree
            nodeValue.parentSubTreeNodeId = parentSubTreeNodeId;
            nodeValue.subTreeEntry = subTreeEntry;
            nodeValue.subTreeType = subTreeType;
            nodeValue.subTreeConfig = subTreeConfig;
            #endregion

            #region 编辑器用
            nodeValue.position = position.Clone(); // 节点位置（编辑器显示使用）
            nodeValue.moveWithChild = moveWithChild;  // 同步移动子节点
            #endregion
            return nodeValue;
        }
    }

    public class ConditionGroup
    {
        public int index;
        public List<string> parameterList = new List<string>();

        public ConditionGroup Clone()
        {
            ConditionGroup group = new ConditionGroup();
            group.index = this.index;
            group.parameterList.AddRange(parameterList);
            return group;
        }
    }

    public class IfJudgeData
    {
        public int nodeId;
        public int ifJudegType = (int)NodeIfJudgeEnum.IF;
        public int ifResult = (int)ResultType.Fail;

        public IfJudgeData Clone()
        {
            IfJudgeData ifJudgeData = new IfJudgeData();
            ifJudgeData.nodeId = nodeId;
            ifJudgeData.ifJudegType = ifJudegType;
            ifJudgeData.ifResult = ifResult;
            return ifJudgeData;
        }
    }

    public enum BehaviorParameterType
    {
        /// <summary>
        /// Float
        /// </summary>
        [EnumAttirbute("Float")]
        Float = 0,

        /// <summary>
        /// Int
        /// </summary>
        [EnumAttirbute("Int")]
        Int = 2,

        /// <summary>
        /// Long
        /// </summary>
        [EnumAttirbute("Long")]
        Long = 3,

        /// <summary>
        /// Bool
        /// </summary>
        [EnumAttirbute("Bool")]
        Bool = 5,

        /// <summary>
        /// String
        /// </summary>
        [EnumAttirbute("String")]
        String = 10,
    }

    public enum BehaviorCompare
    {
        INVALID = 0,
        /// <summary>
        /// 大于
        /// </summary>
        GREATER = 1 << 0,

        /// <summary>
        /// 小于
        /// </summary>
        LESS = 1 << 1,

        /// <summary>
        /// 等于
        /// </summary>
        EQUALS = 1 << 2,

        /// <summary>
        /// 不等于
        /// </summary>
        NOT_EQUAL = 1 << 3,

        /// <summary>
        /// 大于等于
        /// </summary>
        GREATER_EQUALS = 1 << 4,

        /// <summary>
        /// 小于等于
        /// </summary>
        LESS_EQUAL = 1 << 5,
    }

    [SerializeField]
    [Serializable]
    public class BehaviorParameter
    {
        public int parameterType = 0;
        public string parameterName = string.Empty;
        public string CNName = string.Empty;
        public int index;
        public int intValue = 0;
        public long longValue = 0;
        public float floatValue = 0;
        public bool boolValue = false;
        public string stringValue = string.Empty;
        public int compare;

        public BehaviorParameter Clone()
        {
            BehaviorParameter newParameter = new BehaviorParameter();
            newParameter.CloneFrom(this);
            return newParameter;
        }

        public void CloneFrom(BehaviorParameter parameter)
        {
            parameterType = parameter.parameterType;
            parameterName = parameter.parameterName;
            CNName = parameter.CNName;
            index = parameter.index;
            intValue =  parameter.intValue;
            longValue = parameter.longValue;
            floatValue = parameter.floatValue;
            boolValue = parameter.boolValue;
            stringValue = parameter.stringValue;
            compare = parameter.compare;
        }

        public BehaviorCompare Compare(BehaviorParameter parameter)
        {
            BehaviorCompare behaviorCompare = BehaviorCompare.NOT_EQUAL;
            if (parameterType != parameter.parameterType)
            {
                //ProDebug.Logger.LogError("parameter Type not Equal:" + parameter.parameterName + "    " + parameter.parameterType + "    " + parameterType);
                return behaviorCompare;
            }

            if (parameterType == (int)BehaviorParameterType.Float)
            {
                behaviorCompare = CompareFloat(parameter);
            }
            else if (parameterType == (int)BehaviorParameterType.Int)
            {
                behaviorCompare = CompareInt(parameter);
            }
            else if (parameterType == (int)BehaviorParameterType.Long)
            {
                behaviorCompare = CompareLong(parameter);
            }
            else if (parameterType == (int)BehaviorParameterType.Bool)
            {
                behaviorCompare = CompareBool(parameter);
            }
            else if (parameterType == (int)BehaviorParameterType.String)
            {
                behaviorCompare = CompareString(parameter);
            }

            return behaviorCompare;
        }

        public BehaviorCompare CompareFloat(BehaviorParameter parameter)
        {
            BehaviorCompare BehaviorCompare = BehaviorCompare.INVALID;
            if (this.floatValue > parameter.floatValue)
            {
                BehaviorCompare |= BehaviorCompare.GREATER;
            }

            if (this.floatValue < parameter.floatValue)
            {
                BehaviorCompare |= BehaviorCompare.LESS;
            }

            return BehaviorCompare;
        }

        public BehaviorCompare CompareInt(BehaviorParameter parameter)
        {
            BehaviorCompare behaviorCompare = BehaviorCompare.INVALID;
            behaviorCompare = CompareFloat(parameter);

            if (this.intValue > parameter.intValue)
            {
                behaviorCompare |= BehaviorCompare.GREATER;
            }

            if (this.intValue < parameter.intValue)
            {
                behaviorCompare |= BehaviorCompare.LESS;
            }

            if (this.intValue == parameter.intValue)
            {
                behaviorCompare |= BehaviorCompare.EQUALS;
            }

            if (this.intValue != parameter.intValue)
            {
                behaviorCompare |= BehaviorCompare.NOT_EQUAL;
            }

            if (this.intValue >= parameter.intValue)
            {
                behaviorCompare |= BehaviorCompare.GREATER_EQUALS;
            }

            if (this.intValue <= parameter.intValue)
            {
                behaviorCompare |= BehaviorCompare.LESS_EQUAL;
            }

            return behaviorCompare;
        }

        public BehaviorCompare CompareLong(BehaviorParameter parameter)
        {
            BehaviorCompare behaviorCompare = BehaviorCompare.INVALID;
            behaviorCompare = CompareFloat(parameter);

            if (this.longValue > parameter.longValue)
            {
                behaviorCompare |= BehaviorCompare.GREATER;
            }

            if (this.longValue < parameter.longValue)
            {
                behaviorCompare |= BehaviorCompare.LESS;
            }

            if (this.longValue == parameter.longValue)
            {
                behaviorCompare |= BehaviorCompare.EQUALS;
            }

            if (this.longValue != parameter.longValue)
            {
                behaviorCompare |= BehaviorCompare.NOT_EQUAL;
            }

            if (this.longValue >= parameter.longValue)
            {
                behaviorCompare |= BehaviorCompare.GREATER_EQUALS;
            }

            if (this.longValue <= parameter.longValue)
            {
                behaviorCompare |= BehaviorCompare.LESS_EQUAL;
            }

            return behaviorCompare;
        }

        public BehaviorCompare CompareBool(BehaviorParameter parameter)
        {
            BehaviorCompare behaviorCompare = (this.boolValue == parameter.boolValue) ? BehaviorCompare.EQUALS : BehaviorCompare.NOT_EQUAL;
            return behaviorCompare;
        }

        public BehaviorCompare CompareString(BehaviorParameter parameter)
        {
            BehaviorCompare behaviorCompare = (this.stringValue.CompareTo(parameter.stringValue) == 0) ? BehaviorCompare.EQUALS : BehaviorCompare.NOT_EQUAL;
            return behaviorCompare;
        }
    }
    
    public class RectT
    {
        public float x;
        public float y;
        public float width;
        public float height;

        public RectT()
        {
            this.x = 0;
            this.y = 0;
            this.width = 120;
            this.height = 60;
        }

        public RectT(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public RectT Clone()
        {
            RectT rectT = new RectT();
            rectT.x = this.x;
            rectT.y = this.y;
            rectT.width = this.width;
            rectT.height = this.height;
            return rectT;
        }
    }

    public class RectTool
    {
        public static Rect RectTToRect(RectT rectT)
        {
            return new Rect(rectT.x, rectT.y, rectT.width, rectT.height);
        }

        public static RectT RectToRectT(Rect rect)
        {
            return new RectT(rect.x, rect.y, rect.width, rect.height);
        }
    }

}
