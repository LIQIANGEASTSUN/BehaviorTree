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
        /// 随机顺序节点
        /// </summary>
        [EnumAttirbute("随机顺序节点")]
        RANDOM_SEQUEUECE = 3,

        /// <summary>
        /// 随机权重节点
        /// </summary>
        [EnumAttirbute("随机权重节点")]
        RANDOM_PRIORITY = 4,

        /// <summary>
        /// 并行节点
        /// </summary>
        [EnumAttirbute("并行节点")]
        PARALLEL = 5,

        /// <summary>
        /// 并行选择节点
        /// </summary>
        [EnumAttirbute("并行选择节点")]
        PARALLEL_SELECT = 6,

        /// <summary>
        /// 并行执行所有节点
        /// </summary>
        [EnumAttirbute("并行执行所有节点")]
        PARALLEL_ALL = 7,

        /// <summary>
        /// IF 判断节点
        /// </summary>
        [EnumAttirbute("IF 判断节点")]
        IF_JUDEG = 8,

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
        [EnumAttirbute("修饰_返回Fail")]
        DECORATOR_RETURN_FAIL = 102,

        /// <summary>
        /// 修饰节点_返回Success
        /// </summary>
        [EnumAttirbute("修饰_返回Success")]
        DECORATOR_RETURN_SUCCESS = 103,

        /// <summary>
        /// 修饰节点_直到Fail
        /// </summary>
        [EnumAttirbute("修饰_直到Fail")]
        DECORATOR_UNTIL_FAIL = 104,

        /// <summary>
        /// 修饰节点_直到Success
        /// </summary>
        [EnumAttirbute("修饰_直到Success")]
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

        /// <summary>
        /// 子树
        /// </summary>
        [EnumAttirbute("子树")]
        SUB_TREE = 1000,
    }
}

/// <summary>
/// 节点状态
/// </summary>
public enum NODE_STATUS
{
    /// <summary>
    /// 准备状态
    /// </summary>
    READY = 0,

    /// <summary>
    /// 运行状态
    /// </summary>
    RUNNING = 1,
}

/// <summary>
/// 子树类型
/// </summary>
public enum SUB_TREE_TYPE
{
    /// <summary>
    /// 普通：可编辑子节点
    /// </summary>
    [EnumAttirbute("普通：可编辑子节点")]
    NORMAL = 0,

    /// <summary>
    /// 配置：单独的树配置，不可编辑子节点
    /// </summary>
    [EnumAttirbute("配置：读取配置文件")]
    CONFIG = 1,
}

/// <summary>
/// 判断节点类型
/// </summary>
public enum NodeIfJudgeEnum
{
    /// <summary>
    /// if 判断节点
    /// </summary>
    [EnumAttirbute("if 判断节点")]
    IF = 0,

    /// <summary>
    /// 执行节点
    /// </summary>
    [EnumAttirbute("执行节点")]
    ACTION = 1,
}