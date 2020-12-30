using System;

namespace BehaviorTree
{
    public interface ICustomIdentification<out T> where T : NodeLeaf, new()
    {
        T Create();

        string IdentificationName
        {
            get;
            set;
        }

        string Name
        {
            get;
            set;
        }

        NODE_TYPE NodeType
        {
            get;
            set;
        }
    }

    public class CustomIdentification<T> : ICustomIdentification<T> where T : NodeLeaf, new()
    {
        public CustomIdentification(string name, Type t)
        {
            Name = name;
            IdentificationName = t.Name;
            NodeType = (typeof(NodeAction).IsAssignableFrom(t)) ? NODE_TYPE.ACTION : NODE_TYPE.CONDITION;
        }

        public T Create()
        {
            return new T();
        }

        public string IdentificationName
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public NODE_TYPE NodeType
        {
            get;
            set;
        }
    }

    //public struct CustomIdentification
    //{
    //    public CustomIdentification(string name, Type t)
    //    {
    //        Name = name;
    //        IdentificationName = t.Name;
    //        ClassType = t;
    //        NodeType = (typeof(NodeAction).IsAssignableFrom(t)) ? NODE_TYPE.ACTION : NODE_TYPE.CONDITION;
    //    }

    //    public string Name
    //    {
    //        get;
    //        private set;
    //    }

    //    public string IdentificationName
    //    {
    //        get;
    //        private set;
    //    }

    //    public Type ClassType
    //    {
    //        get;
    //        private set;
    //    }

    //    public NODE_TYPE NodeType
    //    {
    //        get;
    //        private set;
    //    }

    //    public object Create()
    //    {
    //        object o = Activator.CreateInstance(ClassType);
    //        return o;
    //    }
    //}

}