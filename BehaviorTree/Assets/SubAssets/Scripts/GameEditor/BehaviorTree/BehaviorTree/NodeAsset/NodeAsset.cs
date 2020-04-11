using UnityEngine;
using BehaviorTree;
using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    public class BehaviorTreeData
    {
        public int rootNodeId = -1;
        public List<NodeValue> nodeList = new List<NodeValue>();
        public List<BehaviorParameter> parameterList = new List<BehaviorParameter>();
        public string descript = string.Empty;
    }

    public class NodeValue
    {
        public int id = 0;
        public bool isRootNode = false;                    // 根节点
        public int NodeType = (int)(NODE_TYPE.SELECT);     // 节点类型 // NODE_TYPE NodeType = NODE_TYPE.SELECT;
        public int parentNodeID = -1;                      // 父节点
        public List<int> childNodeList = new List<int>();  // 子节点集合
        public List<BehaviorParameter> parameterList = new List<BehaviorParameter>();
        public int repeatTimes = 0;
        public string nodeName = string.Empty;
        public int identification = -1;
        public string descript = string.Empty;
        public List<ConditionGroup> conditionGroupList = new List<ConditionGroup>();

        public RectT position = new RectT(0, 0, 100, 100); // 节点位置（编辑器显示使用）
    }

    public class ConditionGroup
    {
        public int index;
        public List<string> parameterList = new List<string>();
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
        /// Bool
        /// </summary>
        [EnumAttirbute("Bool")]
        Bool = 5,
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
        public int intValue = 0;
        public float floatValue = 0;
        public bool boolValue = true;
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
            intValue =  parameter.intValue;
            floatValue = parameter.floatValue;
            boolValue = parameter.boolValue;
            compare = parameter.compare;
        }

        public BehaviorCompare Compare(BehaviorParameter parameter)
        {
            BehaviorCompare behaviorCompare = BehaviorCompare.NOT_EQUAL;
            if (parameterType != parameter.parameterType)
            {
                Debug.LogError("parameter Type not Equal:" + parameter.parameterName + "    " + parameter.parameterType + "    " + parameterType);
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
            else
            {
                behaviorCompare = CompareBool(parameter);
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

        public BehaviorCompare CompareBool(BehaviorParameter parameter)
        {
            BehaviorCompare behaviorCompare = (this.boolValue == parameter.boolValue) ? BehaviorCompare.EQUALS : BehaviorCompare.NOT_EQUAL;
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

        }

        public RectT(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
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
