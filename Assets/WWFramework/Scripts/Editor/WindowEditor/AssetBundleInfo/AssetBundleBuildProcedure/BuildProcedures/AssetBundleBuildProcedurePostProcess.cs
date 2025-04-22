/*------------------------------
 * 脚本名称: AssetBundleBuildProcedureAfter
 * 创建者: movin
 * 创建日期: 2025/04/09
------------------------------*/

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;

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

        protected override UniTask DoExecute()
        {
            // 根据清单文件生成文件信息字符串,保存在文档中
            var allBundles = Config.assetBundleManifest.GetAllAssetBundles()
                .SelectList(x => $"{x};{Config.assetBundleManifest.GetAssetBundleHash(x).ToString()}");
            StringBuilder sb = new StringBuilder();
            allBundles.ForEach(x => sb.AppendLine(x));
            var fileName = Path.Combine(Config.BuildOutputDir, Config.Version.Replace(".", "_"));
            File.WriteAllText(fileName, sb.ToString());
            // 查看是否有之前版本的包信息文件
            var versions = Config.Version.Split('.');
            if (int.TryParse(versions[0], out var majorVersion)
                && int.TryParse(versions[1], out var minorVersion))
            {
                for (int lastVersion = minorVersion - 1; lastVersion >= 0; lastVersion--)
                {
                    var lastVersionFile = Path.Combine(
                        Config.GetVersionPath($"{majorVersion}.{lastVersion}", Config.target),
                        $"{majorVersion}_{lastVersion}").Replace('\\', '/');
                    // 有之前版本ab文件信息,构建升级文件信息
                    if (File.Exists(lastVersionFile))
                    {
                        // 按行读取文件信息,并保存到一个hash表中
                        var lastVersionFiles = File.ReadAllLines(lastVersionFile).ToHashSet();
                        sb.Clear();
                        // 构建文件变化列表
                        foreach (var bundleName in allBundles)
                        {
                            if (!lastVersionFiles.Remove(bundleName))
                            {
                                sb.AppendLine($"+{bundleName}");
                            }
                        }
                        foreach (var bundleName in lastVersionFiles)
                        {
                            sb.AppendLine($"-{bundleName}");
                        }
                        // 保存到文档中
                        var changeFileName = Path.Combine(Config.BuildOutputDir, $"{majorVersion}_{lastVersion}-{Config.Version.Replace(".", "_")}").Replace('\\', '/');
                        File.WriteAllText(changeFileName, sb.ToString());
                    }
                }
            }
            return base.DoExecute();
        }
    }
}