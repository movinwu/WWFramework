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
    }
}