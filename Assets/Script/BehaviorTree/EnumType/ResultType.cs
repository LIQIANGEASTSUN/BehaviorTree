namespace BehaviorTree
{
    /// <summary>
    /// 节点执行结果
    /// </summary>
    public enum ResultType
    {
        /// <summary>
        /// 失败
        /// </summary>
        Fail        = 0,

        /// <summary>
        /// 成功
        /// </summary>
        Success     = 1,

        /// <summary>
        /// 执行中
        /// </summary>
        Running     = 2,
    }
}