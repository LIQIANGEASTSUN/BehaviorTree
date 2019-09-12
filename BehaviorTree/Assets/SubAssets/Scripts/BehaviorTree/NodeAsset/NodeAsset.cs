using UnityEngine;
using BehaviorTree;
using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    public class GlobalParameter
    {
        public List<BehaviorParameter> parameterList = new List<BehaviorParameter>();
    }

    public class BehaviorTreeData
    {
        public int rootNodeId = -1;
        public List<NodeValue> nodeList = new List<NodeValue>();
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

        public RectT position = new RectT(0, 0, 100, 100); // 节点位置（编辑器显示使用）
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
        Greater = 0,
        Less = 1,
        Equals = 2,
        NotEqual = 3,
    }

    public class BehaviorParameter
    {
        public int parameterType = 0;
        public string parameterName = string.Empty;
        public int intValue = 0;
        public float floatValue = 0;
        public bool boolValue = true;
        public int compare;

        public BehaviorParameter Clone()
        {
            BehaviorParameter newParameter = new BehaviorParameter();
            Clone(newParameter);
            return newParameter;
        }

        public void Clone(BehaviorParameter parameter)
        {
            parameter.parameterType = parameterType;
            parameter.parameterName = parameterName;
            parameter.intValue = intValue;
            parameter.floatValue = floatValue;
            parameter.boolValue = boolValue;
            parameter.compare = compare;
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
