/*------------------------------
 * 脚本名称: LogConfig
 * 创建者: movin
 * 创建日期: 2025/04/30
------------------------------*/

using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 日志配置
    /// </summary>
    public class LogConfig : ScriptableObject
    {
        [Header("允许打印的日志类型")]
        public ELogType enableLogType = ELogType.Common;

        [Header("允许打印的日志级别(debug, warning, error)")]
        public bool enableLogDebug = true;
        public bool enableLogWarning = true;
        public bool enableLogError = true;
    }
}