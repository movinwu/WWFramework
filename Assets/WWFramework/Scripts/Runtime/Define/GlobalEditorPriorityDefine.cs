/*------------------------------
 * 脚本名称: GlobalPriorityDefine
 * 创建者: movin
 * 创建日期: 2025/02/16
------------------------------*/

namespace WWFramework
{
    /// <summary>
    /// 全局编辑器优先级定义
    /// </summary>
    public class GlobalEditorPriorityDefine
    {
        #region ScriptableObject配置
        
        /// <summary>
        /// 资源包构建配置
        /// </summary>
        public const int AssetBundleInfoConfig = 1;

        /// <summary>
        /// 全局游戏配置
        /// </summary>
        public const int GlobalGameConfig = 2;

        /// <summary>
        /// 网络配置
        /// </summary>
        public const int NetworkConfig = 3;

        #endregion ScriptableObject配置
        
        #region 脚本模板

        /// <summary>
        /// 通用脚本模板
        /// </summary>
        public const int CommonScriptTemplate = 2;

        /// <summary>
        /// WWFramework脚本模板
        /// </summary>
        public const int WWFrameworkScriptTemplate = 3;
        
        #endregion 脚本模板

        #region 编辑器窗口

        /// <summary>
        /// 全局编辑器窗口
        /// </summary>
        public const int GlobalEditorWindow = 1;

        #endregion 编辑器窗口
    }
}