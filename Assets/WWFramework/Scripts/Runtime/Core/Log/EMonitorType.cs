/*------------------------------
 * 脚本名称: EMonitorType
 * 创建者: movin
 * 创建日期: 2025/03/22
------------------------------*/

namespace WWFramework
{
    /// <summary>
    /// 时间监控类型
    /// </summary>
    public enum EMonitorType : byte
    {
        /// <summary>
        /// C#系统时间监控
        /// </summary>
        System,
        
        /// <summary>
        /// unity真实时间监控
        /// </summary>
        UnityRealTime,
    }
}