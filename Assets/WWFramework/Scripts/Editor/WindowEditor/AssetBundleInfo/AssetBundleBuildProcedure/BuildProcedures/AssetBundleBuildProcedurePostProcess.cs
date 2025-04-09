/*------------------------------
 * 脚本名称: AssetBundleBuildProcedureAfter
 * 创建者: movin
 * 创建日期: 2025/04/09
------------------------------*/

using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// AB包构建流程-打包后处理(包文件加密,不打包文件拷贝等)
    /// </summary>
    public class AssetBundleBuildProcedurePostProcess : AssetBundleBuildProcedureBase
    {
        public AssetBundleBuildProcedurePostProcess(AssetBundleInfoConfig config) : base(config)
        {
        }
    }
}