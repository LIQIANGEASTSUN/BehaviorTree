namespace BehaviorTree
{
    /// <summary>
    /// 修饰节点(组合节点)
    /// </summary>
    public abstract class NodeDecorator : NodeComposite
    {
        public NodeDecorator(NODE_TYPE nodeType) :base(nodeType)
        {  }

        /// <summary>
        /// 修饰节点不能独立存在，其作用为对子节点进行修饰，以得到我们所希望的结果
        /// 修饰节点常用的几个类型如下：
        /// Inverter        对子节点执行结果取反
        /// Repeater        重复执行子节点 N 次
        /// Return Failure  执行到此节点时返回失败
        /// Return Success  执行到此节点时返回成功
        /// Unitl Failure   直到失败，一直执行子节点
        /// Until Success   直到成功，一直执行子节点
        /// </summary>
        /// <returns></returns>
        public override ResultType Execute()
        {
            return ResultType.Success;
        }
    }
}

/*
     // Unitl Failure、Until Success 伪代码如下
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
