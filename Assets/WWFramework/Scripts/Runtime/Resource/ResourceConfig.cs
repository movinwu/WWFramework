/*------------------------------
 * 脚本名称: ResourceConfig
 * 创建者: movin
 * 创建日期: 2025/04/30
------------------------------*/

using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 资源配置
    /// </summary>
    public class ResourceConfig : ScriptableObject
    {
#if UNITY_EDITOR
        /// <summary>
        /// 额外shader变体材质路径
        /// </summary>
        [Header("额外shader变体材质路径")]
        public string extraShaderVariantMaterialPath = "Assets/TempMaterials";
#endif
        
        [Header("资源模式")]
        public EResourceMode resourceMode = EResourceMode.LocalAssetBundle;
        
        [Header("本地存放AB包文件夹路径")]
        public string abLocalPath = "LocalAssetBundle";
        
        /// <summary>
        /// AB包加载失败,重新加载次数
        /// </summary>
        [Header("AB包加载失败,重新加载次数")]
        public int abLoadRetryCount = 0;
        
        [Header("AB包释放延迟时间")]
        public float abReleaseDelayTime = 3;
        
        [Header("资源释放延迟时间")]
        public float assetReleaseDelayTime = 3;
    }
}