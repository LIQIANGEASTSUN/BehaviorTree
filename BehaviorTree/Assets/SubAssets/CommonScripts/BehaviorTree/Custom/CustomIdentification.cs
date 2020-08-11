using System;

namespace BehaviorTree
{
    public struct CustomIdentification
    {
        /// <summary>
        /// 条件节点从 20000 开始， 行为节点从 10000 开始
        /// </summary>
        /// <param name="name"></param>
        /// <param name="identification"></param>
        public CustomIdentification(string name, Type t)
        {
            Name = name;
            IdentificationName = t.Name;
            ClassType = t;
            NodeType = (typeof(NodeAction).IsAssignableFrom(t)) ? NODE_TYPE.ACTION : NODE_TYPE.CONDITION;
        }

        public string Name
        {
            get;
            private set;
        }

        public string IdentificationName
        {
            get;
            private set;
        }

        public Type ClassType
        {
            get;
            private set;
        }

        public NODE_TYPE NodeType
        {
            get;
            private set;
        }

        public bool Valid()
        {
            return !string.IsNullOrEmpty(IdentificationName);
        }

        public object Create()
        {
            object o = Activator.CreateInstance(ClassType);
            return o;
        }
    }

}