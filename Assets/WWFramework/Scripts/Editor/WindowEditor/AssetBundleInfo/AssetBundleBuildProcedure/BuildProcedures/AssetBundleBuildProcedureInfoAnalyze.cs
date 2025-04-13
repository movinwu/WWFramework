/*------------------------------
 * 脚本名称: AssetBundleBuildProcedureInfoAnalysis
 * 创建者: movin
 * 创建日期: 2025/04/09
------------------------------*/

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// AB包构建流程-AB包信息分析
    /// </summary>
    public class AssetBundleBuildProcedureInfoAnalyze : AssetBundleBuildProcedureBase
    {
        public AssetBundleBuildProcedureInfoAnalyze(AssetBundleInfoConfig config) : base(config)
        {
        }

        protected override UniTask DoExecute()
        {
            if (Config.analyze)
            {
                Config.Analyzer.Analyze(Config.analyzeLimit);
            }
            return base.DoExecute();
        }
    }
}