/*------------------------------
 * 脚本名称: ResourceType
 * 创建者: movin
 * 创建日期: 2025/05/01
------------------------------*/

using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 资源模式
    /// </summary>
    public enum EResourceMode
    {
        /// <summary>
        /// 本地AB模式
        /// <para> ab包直接放在StreamingAssets目录下 </para>
        /// </summary>
        [InspectorName("本地AB模式")] LocalAssetBundle,

        /// <summary>
        /// 远端AB模式
        /// <para> 从远端下载ab包并放在PresidentPath目录下 </para>
        /// </summary>
        [InspectorName("远端AB模式")] RemoteAssetBundle,

#if UNITY_EDITOR
        /// <summary>
        /// 开发者模式
        /// <para> 开发时使用,直接使用AssetDatabase加载资源 </para>
        /// </summary>
        [InspectorName("开发模式")] Development,
#endif
    }
}