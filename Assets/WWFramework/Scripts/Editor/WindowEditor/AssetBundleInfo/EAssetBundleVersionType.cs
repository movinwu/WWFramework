/*------------------------------
 * 脚本名称: EAssetBundleVersionType
 * 创建者: movin
 * 创建日期: 2025/04/08
------------------------------*/

using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// ab包版本类型
    /// </summary>
    public enum EAssetBundleVersionType : byte
    {
        /// <summary>
        /// 不指定版本(不打包资源)
        /// </summary>
        [InspectorName("不指定版本(不打包资源)")]
        None,
        
        /// <summary>
        /// 递增版本
        /// </summary>
        [InspectorName("递增版本")]
        Increase,
        
        /// <summary>
        /// 指定版本
        /// </summary>
        [InspectorName("指定版本")]
        Specific,
    }
}