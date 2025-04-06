/*------------------------------
 * 脚本名称: ECollectorBuildType
 * 创建者: movin
 * 创建日期: 2025/04/06
------------------------------*/

namespace WWFramework
{
    /// <summary>
    /// 收集器构建类型
    /// </summary>
    public enum ECollectorBuildType
    {
        /// <summary>
        /// 单个包
        /// </summary>
        OneBundle,
        
        /// <summary>
        /// 每个文件单独ab包
        /// </summary>
        EachFile,
        
        /// <summary>
        /// 不打包
        /// </summary>
        NotBuild,
    }
}