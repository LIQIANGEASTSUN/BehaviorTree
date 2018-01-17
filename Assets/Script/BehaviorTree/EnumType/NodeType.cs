using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 行为树节点类型
    /// </summary>
    public enum NodeType
    {
        /// <summary>
        /// 选择节点
        /// </summary>
        Select = 0,

        /// <summary>
        /// 顺序节点
        /// </summary>
        Sequence = 1,

        /// <summary>
        /// 修饰节点
        /// </summary>
        Decorator = 2,

        /// <summary>
        /// 随机节点
        /// </summary>
        Random = 3,

        /// <summary>
        /// 并行节点
        /// </summary>
        Parallel = 4,

        /// <summary>
        /// 条件节点
        /// </summary>
        Condition = 5,

        /// <summary>
        /// 行为节点
        /// </summary>
        Action = 6,
    }
}