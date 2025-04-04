/*------------------------------
 * 脚本名称: GameConfig
 * 创建者: movin
 * 创建日期: 2025/03/18
------------------------------*/

using UnityEngine;
using UnityEngine.Serialization;

namespace WWFramework
{
    /// <summary>
    /// 游戏配置类,全局游戏配置
    /// </summary>
    [CreateAssetMenu(fileName = GlobalEditorStringDefine.GlobalGameConfigName, menuName = GlobalEditorStringDefine.GlobalGameConfig, order = GlobalEditorPriorityDefine.GlobalGameConfig)]
    public class GameConfig : ScriptableObject
    {
        [Header("允许打印的日志类型")]
        public ELogType enableLogType = ELogType.Common;

        [Header("允许打印的日志级别(debug, warning, error)")]
        public bool enableLogDebug = true;
        public bool enableLogWarning = true;
        public bool enableLogError = true;
        
        [Header("网络配置")]
        public NetworkConfig networkConfig;
    }
}