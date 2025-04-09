/*------------------------------
 * 脚本名称: AssetBundleBuildProcedureRemoteFile
 * 创建者: movin
 * 创建日期: 2025/04/09
------------------------------*/

using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// AB包构建流程-远端文件上传(上传到正式CDN或测试CDN)
    /// </summary>
    public class AssetBundleBuildProcedureRemoteFile : AssetBundleBuildProcedureBase
    {
        public AssetBundleBuildProcedureRemoteFile(AssetBundleInfoConfig config) : base(config)
        {
        }
    }
}