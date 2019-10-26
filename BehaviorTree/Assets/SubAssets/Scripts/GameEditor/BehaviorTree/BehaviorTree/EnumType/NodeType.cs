namespace BehaviorTree
{
    /// <summary>
    /// 行为树节点类型
    /// </summary>
    public enum NODE_TYPE
    {
        /// <summary>
        /// 选择节点
        /// </summary>
        [EnumAttirbute("选择节点")]
        SELECT = 0,

        /// <summary>
        /// 顺序节点
        /// </summary>
        [EnumAttirbute("顺序节点")]
        SEQUENCE = 1,

        /// <summary>
        /// 随机节点
        /// </summary>
        [EnumAttirbute("随机节点")]
        RANDOM = 2,

        /// <summary>
        /// 并行节点
        /// </summary>
        [EnumAttirbute("并行节点")]
        PARALLEL = 3,

        /// <summary>
        /// 修饰节点_取反
        /// </summary>
        [EnumAttirbute("修饰节点_取反")]
        DECORATOR_INVERTER = 100,

        /// <summary>
        /// 修饰节点_重复
        /// </summary>
        [EnumAttirbute("修饰节点_重复")]
        DECORATOR_REPEAT = 101,

        /// <summary>
        /// 修饰节点_返回Fail
        /// </summary>
        [EnumAttirbute("修饰节点_返回Fail")]
        DECORATOR_RETURN_FAIL = 102,

        /// <summary>
        /// 修饰节点_返回Success
        /// </summary>
        [EnumAttirbute("修饰节点_返回Success")]
        DECORATOR_RETURN_SUCCESS = 103,

        /// <summary>
        /// 修饰节点_直到Fail
        /// </summary>
        [EnumAttirbute("修饰节点_直到Fail")]
        DECORATOR_UNTIL_FAIL = 104,

        /// <summary>
        /// 修饰节点_直到Success
        /// </summary>
        [EnumAttirbute("修饰节点_直到Success")]
        DECORATOR_UNTIL_SUCCESS = 105,

        /// <summary>
        /// 条件节点
        /// </summary>
        [EnumAttirbute("条件节点")]
        CONDITION = 200,

        /// <summary>
        /// 行为节点
        /// </summary>
        [EnumAttirbute("行为节点")]
        ACTION = 300,
    }
}