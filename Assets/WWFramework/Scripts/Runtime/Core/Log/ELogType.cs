/*------------------------------
 * 脚本名称: ELogType
 * 创建者: movin
 * 创建日期: 2025/03/07
------------------------------*/

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
        Common = 1 << 0,
        
        /// <summary>
        /// 资源日志
        /// </summary>
        Resource = 1 << 1,
        
        /// <summary>
        /// 配置日志
        /// </summary>
        Config = 1 << 2,
        
        /// <summary>
        /// 数据表日志
        /// </summary>
        DataTable = 1 << 3,
        
        /// <summary>
        /// 网络日志
        /// </summary>
        Network = 1 << 4,
    }
}