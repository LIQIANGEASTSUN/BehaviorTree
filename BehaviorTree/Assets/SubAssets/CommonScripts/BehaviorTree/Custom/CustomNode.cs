using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{

    /// <summary>
    /// 自定义节点
    /// 只有 条件节点、行为节点 需要自定义
    /// 感觉叫CustomNodes更直观一点
    /// 这里存储所有自定义的节点类型，供解析和编辑器使用
    /// </summary>
    public class CustomNode
    {
        public readonly static CustomNode Instance = new CustomNode();

        private ConfigBehaviorNode configBehaviorNode = null;
        private HashSet<int> hash = new HashSet<int>();
        private List<CustomIdentification> nodeList = new List<CustomIdentification>();

        public CustomNode()
        {
            configBehaviorNode = new ConfigBehaviorNode();
            configBehaviorNode.AddEvent(AddIdentification);
            configBehaviorNode.Init();
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
            for (int i = 0; i < nodeList.Count; ++i)
            {
                CustomIdentification info = nodeList[i];
                if (info.Identification == (int)identification)
                {
                    return info;
                }
            }

            return new CustomIdentification();
        }

        private void AddIdentification(CustomIdentification customIdentification)
        {
            if (hash.Contains(customIdentification.Identification))
            {
                //ProDebug.Logger.LogError("重复的 Identification:" + customIdentification.Identification);
                return;
            }
            hash.Add(customIdentification.Identification);

            nodeList.Add(customIdentification);
        }

        public List<CustomIdentification> GetNodeList()
        {
            return nodeList;
        }

    }

}



