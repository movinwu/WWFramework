/*------------------------------
 * 脚本名称: ECollectorBuildType
 * 创建者: movin
 * 创建日期: 2025/04/06
------------------------------*/

using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 收集器构建类型
    /// </summary>
    public enum ECollectorInfoType
    {
        /// <summary>
        /// 整个文件夹打成一个AB包
        /// </summary>
        [InspectorName("整个文件夹打成一个AB包")]
        WholeFolder,
        
        /// <summary>
        /// 每个文件单独ab包
        /// </summary>
        [InspectorName("每个文件单独ab包")]
        EachFile,
        
        /// <summary>
        /// 不打包
        /// </summary>
        [InspectorName("直接拷贝文件")]
        CopyFiles,
    }
}