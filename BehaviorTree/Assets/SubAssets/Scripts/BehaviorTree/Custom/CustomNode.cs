using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BehaviorTree
{

    public struct CustomIdentification
    {
        private string name;
        private int identification;
        private Type type;
        private NODE_TYPE nodeType;
        
        /// <summary>
        /// 条件节点从 20000 开始， 行为节点从 10000 开始
        /// </summary>
        /// <param name="name"></param>
        /// <param name="identification"></param>
        public CustomIdentification(string name, int identification, Type t, NODE_TYPE nodeType)
        {
            this.name = name;
            this.identification = identification;
            this.type = t;
            this.nodeType = nodeType;
        }

        public string Name
        {
            get { return name; }
        }
    
        public int Identification
        {
            get { return identification; }
        }

        public Type Type
        {
            get { return type; }
        }

        public NODE_TYPE NodeType
        {
            get { return nodeType; }
        }

        public bool Valid()
        {
            return identification > 0;
        }

        public object Create()
        {
            object o = Activator.CreateInstance(type);
            return o;
        }
    }

    /// <summary>
    /// 自定义节点
    /// 只有 条件节点、行为节点 需要自定义
    /// </summary>
    public class CustomNode
    {
        public readonly static CustomNode Instance = new CustomNode();

        private List<CustomIdentification> nodeList = new List<CustomIdentification>();

        public CustomNode()
        {
        }

        public object GetNode(int identification)
        {
            object obj = null;
            CustomIdentification info = GetIdentification(identification);
            if (info.Valid())
            {
                obj = info.Create();
            }
            return obj;
        }

        public CustomIdentification GetIdentification(int identification)
        {
            GetNodeList();

            for (int i = 0; i < nodeList.Count; ++i)
            {
                CustomIdentification info = nodeList[i];
                if (info.Identification == identification)
                {
                    return info;
                }
            }

            return new CustomIdentification();
        }

        public List<CustomIdentification> GetNodeList()
        {
            if (nodeList.Count > 0)
            {
                return nodeList;
            }


            #region Skill
            // 条件节点
            {
                CustomIdentification input = NodeConditionInput.CustomIdentification();
                nodeList.Add(input);

                CustomIdentification skillState = NodeConditionSkillState.CustomIdentification();
                nodeList.Add(skillState);

                CustomIdentification general = NodeConditionGeneral.CustomIdentification();
                nodeList.Add(general);
            }

            // 行为节点
            {
                CustomIdentification requestSkillState = NodeActionRequestSkillState.CustomIdentification();
                nodeList.Add(requestSkillState);
            }
            #endregion

            #region Human
            // 条件节点
            {
                CustomIdentification custom = NodeConditionCustom.CustomIdentification();
                nodeList.Add(custom);
            }

            // 行为节点
            {
                CustomIdentification cooking = NodeActionCooking.CustomIdentification();
                nodeList.Add(cooking);

                CustomIdentification eat = NodeActionEat.CustomIdentification();
                nodeList.Add(eat);

                CustomIdentification move = NodeActionMove.CustomIdentification();
                nodeList.Add(move);
            }
            #endregion

            HashSet<int> hash = new HashSet<int>();
            for (int i = 0; i < nodeList.Count; ++i)
            {
                CustomIdentification identificationi = nodeList[i];
                if (hash.Contains(identificationi.Identification))
                {
                    Debug.LogError("重复的 Identification:" + identificationi.Identification);
                    break;
                }
                hash.Add(identificationi.Identification);
            }

            return nodeList;
        }

    }

}



