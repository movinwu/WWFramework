/*------------------------------
 * 脚本名称: AssetBundleBuildProcedureInfoCollect
 * 创建者: movin
 * 创建日期: 2025/04/09
------------------------------*/

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// AB构建流程-AB信息收集
    /// </summary>
    public class AssetBundleBuildProcedureInfoCollect : AssetBundleBuildProcedureBase
    {
        public AssetBundleBuildProcedureInfoCollect(AssetBundleInfoConfig config) : base(config)
        {
        }

        protected override UniTask DoExecute()
        {
            Config.Analyzer = new AssetBundleBuildAnalyzer(Config.fileCollectors);
            return base.DoExecute();
        }
    }
}