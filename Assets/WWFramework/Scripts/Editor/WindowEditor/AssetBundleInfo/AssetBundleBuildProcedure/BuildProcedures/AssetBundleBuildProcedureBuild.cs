/*------------------------------
 * 脚本名称: AssetBundleBuildProcedureBuild
 * 创建者: movin
 * 创建日期: 2025/04/09
------------------------------*/

using Cysharp.Threading.Tasks;
using UnityEditor;

namespace WWFramework
{
    /// <summary>
    /// AB包构建流程-正式打包AB包
    /// </summary>
    public class AssetBundleBuildProcedureBuild : AssetBundleBuildProcedureBase
    {
        public AssetBundleBuildProcedureBuild(AssetBundleInfoConfig config) : base(config)
        {
        }

        protected override async UniTask DoExecute()
        {
            Config.AssetBundleManifest = BuildPipeline.BuildAssetBundles(Config.OriginalBuildOutputDir, Config.Analyzer.GetBuildArray(Config.analyzeLimit), Config.option, Config.target);
            // 等待直到打包完成
            await UniTask.WaitUntil(() => !EditorApplication.isCompiling);
        }
    }
}