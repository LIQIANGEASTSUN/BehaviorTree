using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{

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

        public object GetNode(IDENTIFICATION identification)
        {
            object obj = null;
            CustomIdentification info = GetIdentification(identification);
            if (info.Valid())
            {
                obj = info.Create();
            }
            return obj;
        }

        public CustomIdentification GetIdentification(IDENTIFICATION identification)
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
                CustomIdentification custom = NodeConditionCustom.CustomIdentification();
                nodeList.Add(custom);
            }

            // 行为节点
            {
                CustomIdentification requestSkillState = NodeActionRequestSkillState.CustomIdentification();
                nodeList.Add(requestSkillState);
            }
            #endregion

            HashSet<IDENTIFICATION> hash = new HashSet<IDENTIFICATION>();
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



