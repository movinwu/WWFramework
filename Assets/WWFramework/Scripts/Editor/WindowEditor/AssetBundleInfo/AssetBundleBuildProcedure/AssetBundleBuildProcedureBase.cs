/*------------------------------
 * 脚本名称: AssetBundleBuildProcedureBase
 * 创建者: movin
 * 创建日期: 2025/04/09
------------------------------*/

namespace WWFramework
{
    /// <summary>
    /// AB包构建流程基类
    /// </summary>
    public class AssetBundleBuildProcedureBase : ProcedureBase
    {
        /// <summary>
        /// 构建信息基类
        /// </summary>
        protected AssetBundleInfoConfig Config { get; private set; }

        public AssetBundleBuildProcedureBase(AssetBundleInfoConfig config)
        {
            this.Config = config;
        }
    }
}