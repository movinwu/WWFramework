/*------------------------------
 * 脚本名称: AssetBundleBuildProcedureFolder
 * 创建者: movin
 * 创建日期: 2025/04/09
------------------------------*/

using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// AB构建流程-相关文件夹检查
    /// </summary>
    public class AssetBundleBuildProcedureFolder : AssetBundleBuildProcedureBase
    {
        public AssetBundleBuildProcedureFolder(AssetBundleInfoConfig config) : base(config)
        {
        }

        protected override UniTask DoExecute()
        {
            // 清理AB包输出文件夹
            var outputDir = Config.BuildOutputDir;
            if (System.IO.Directory.Exists(outputDir))
            {
                System.IO.Directory.Delete(outputDir, true);
            }
            System.IO.Directory.CreateDirectory(outputDir);
            // 创建输出文件夹
            System.IO.Directory.CreateDirectory(Config.OriginalBuildOutputDir);
            System.IO.Directory.CreateDirectory(Config.Md5BuildOutputDir);
            System.IO.Directory.CreateDirectory(Config.FinalBuildOutputDir);
            // 清理StreamingAssets文件夹(需要输出到此文件夹下的话)
            if (Config.copyToStreamingAssets)
            {
                var path = Config.StreamingAssetsCopyDir;
                if (System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.Delete(path, true);
                }
                System.IO.Directory.CreateDirectory(path);
            }
            return base.DoExecute();
        }
    }
}