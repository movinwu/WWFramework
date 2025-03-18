/*------------------------------
 * 脚本名称: GameConfig
 * 创建者: movin
 * 创建日期: 2025/03/18
------------------------------*/

using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 游戏配置类,全局游戏配置
    /// </summary>
    [CreateAssetMenu(fileName = GlobalEditorStringDefine.GlobalGameConfigName, menuName = GlobalEditorStringDefine.GlobalGameConfig, order = GlobalEditorPriorityDefine.GlobalGameConfig)]
    public class GameConfig : ScriptableObject
    {
        [Header("允许打印的日志类型")]
        public ELogType EnableLogType = ELogType.Common;

        [Header("允许打印的日志级别(debug, warning, error)")]
        public bool EnableLogDebug = true;
        public bool EnableLogWarning = true;
        public bool EnableLogError = true;
    }
}