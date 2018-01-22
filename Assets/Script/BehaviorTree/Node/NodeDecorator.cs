namespace BehaviorTree
{
    /// <summary>
    /// 修饰节点(组合节点)
    /// </summary>
    public class NodeDecorator : NodeCombiner
    {
        /// <summary>
        /// 希望的结果
        /// </summary>
        private ResultType untilResultType = ResultType.Fail;
        public NodeDecorator():base(NodeType.Decorator)
        {  }

        /// <summary>
        /// 修饰节点只有一个子节点，执行子节点直到 执行结果 = untilResultType,将 结果返回给父节点
        /// 如果执行结果 != untilResultType 则返回 Running
        /// </summary>
        /// <returns></returns>
        public override ResultType Execute()
        {
            NodeRoot nodeRoot = nodeChildList[0];
            ResultType resultType = nodeRoot.Execute();
            if (resultType != untilResultType)
            {
                return ResultType.Running;
            }

            return resultType;
        }

        public void SetResultType(ResultType resultType)
        {
            untilResultType = resultType;
        }
    }
}

/*
     defaultResult = 期望结果

     do
     Node node = GetChild(0)
     result = node.Execute();
    
     if result != defaultResult then
         return running
     end

     return defaultResult
     end

*/