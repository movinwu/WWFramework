/*------------------------------
 * 脚本名称: ELogType
 * 创建者: movin
 * 创建日期: 2025/03/07
------------------------------*/

using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 日志类型枚举
    /// </summary>
    [System.Flags]
    public enum ELogType
    {
        /// <summary>
        /// 通用日志
        /// </summary>
        [InspectorName("通用日志")]
        Common = 1 << 0,
        
        /// <summary>
        /// 资源日志
        /// </summary>
        [InspectorName("资源日志")]
        Resource = 1 << 1,
        
        /// <summary>
        /// 配置日志
        /// </summary>
        [InspectorName("配置日志")]
        Config = 1 << 2,
        
        /// <summary>
        /// 数据表日志
        /// </summary>
        [InspectorName("数据表日志")]
        DataTable = 1 << 3,
        
        /// <summary>
        /// 网络日志
        /// </summary>
        [InspectorName("网络日志")]
        Network = 1 << 4,
        
        /// <summary>
        /// 事件日志
        /// </summary>
        [InspectorName("事件日志")]
        Event = 1 << 5,
    }
}