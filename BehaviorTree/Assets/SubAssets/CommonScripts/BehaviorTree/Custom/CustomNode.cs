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
        private Dictionary<string, ICustomIdentification<NodeLeaf>> nodeDic = new Dictionary<string, ICustomIdentification<NodeLeaf>>();

        public CustomNode()
        {
            configBehaviorNode = new ConfigBehaviorNode();
            configBehaviorNode.AddEvent(AddIdentification);
            configBehaviorNode.Init();
        }

        public NodeLeaf GetNode(string identificationName)
        {
            ICustomIdentification<NodeLeaf> info = GetIdentification(identificationName);
            NodeLeaf obj = info.Create();
            return obj;
        }

        public ICustomIdentification<NodeLeaf> GetIdentification(string identificationName)
        {
            ICustomIdentification<NodeLeaf> info;
            if (nodeDic.TryGetValue(identificationName, out info))
            {
                return info;
            }

            return null;
        }

        private void AddIdentification(ICustomIdentification<NodeLeaf> customIdentification)
        {
            nodeDic[customIdentification.IdentificationName] = customIdentification;
        }

        public Dictionary<string, ICustomIdentification<NodeLeaf>> GetNodeDic()
        {
            return nodeDic;
        }

    }

}



