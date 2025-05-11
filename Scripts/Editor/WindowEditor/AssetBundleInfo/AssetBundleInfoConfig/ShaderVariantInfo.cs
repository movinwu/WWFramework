/*------------------------------
 * 脚本名称: ShaderVariantInfo
 * 创建者: movin
 * 创建日期: 2025/04/22
------------------------------*/

using System;
using System.Collections.Generic;

namespace WWFramework
{
    /// <summary>
    /// shader变体信息
    /// </summary>
    [Serializable]
    public class ShaderVariantInfo
    {
        /// <summary>
        /// shader路径
        /// </summary>
        public string ShaderPath = string.Empty;

        /// <summary>
        /// 变体列表
        /// </summary>
        public List<string> ShaderVariants = new List<string>();
    }
}