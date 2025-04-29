/*------------------------------
 * 脚本名称: EAssetBundleLoadState
 * 创建者: movin
 * 创建日期: 2025/04/30
------------------------------*/

using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 加载状态
    /// </summary>
    public enum ELoadStatus : byte
    {
        /// <summary>
        /// 没有开始加载
        /// </summary>
        NotStart,
        
        /// <summary>
        /// 加载中
        /// </summary>
        Loading,
        
        /// <summary>
        /// 加载失败
        /// </summary>
        Faulted,
        
        /// <summary>
        /// 加载完成
        /// </summary>
        Loaded,
    }
}