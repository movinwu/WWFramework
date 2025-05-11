/*------------------------------
 * 脚本名称: TabGroupState
 * 创建者: movin
 * 创建日期: 2025/04/05
------------------------------*/

using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 页签组状态
    /// </summary>
    public enum ETabGroupState : byte
    {
        /// <summary>
        /// 无状态
        /// </summary>
        None,
        
        /// <summary>
        /// 初始化中
        /// </summary>
        Initializing,
        
        /// <summary>
        /// 显示中(正在显示某一个页签)
        /// </summary>
        Showing,
        
        /// <summary>
        /// 切换中(由一个页签切换到另一个页签)
        /// </summary>
        Switching,
    }
}