/*------------------------------
 * 脚本名称: AssetBundleBuildProcedureCheck
 * 创建者: movin
 * 创建日期: 2025/04/09
------------------------------*/

using Cysharp.Threading.Tasks;
using UnityEditor;

namespace WWFramework
{
    /// <summary>
    /// AB包构建流程-参数检查
    /// </summary>
    public class AssetBundleBuildProcedureCheck : AssetBundleBuildProcedureBase
    {
        public AssetBundleBuildProcedureCheck(AssetBundleInfoConfig config) : base(config)
        {
        }

        protected override UniTask DoExecute()
        {
            // 指定版本号检查
            if (Config.versionType == EAssetBundleVersionType.Specific)
            {
                // 检查指定版本号
                if (string.IsNullOrEmpty(Config.specificVersion))
                {
                    throw new System.Exception("指定版本号不能为空");
                }
                var versions = Config.specificVersion.Split('.');
                if (versions.Length != 2)
                {
                    throw new System.Exception("指定版本号格式错误");
                }
                if (!int.TryParse(versions[0], out var major) || !int.TryParse(versions[1], out var minor))
                {
                    throw new System.Exception("指定版本号格式错误");
                }
                if (major < 0 || minor < 0)
                {
                    throw new System.Exception("指定版本号格式错误");
                }
            }
            // 上一次版本号检查
            if (Config.versionType == EAssetBundleVersionType.Increase)
            {
                // 检查上一次版本号
                if (string.IsNullOrEmpty(Config.lastVersion))
                {
                    throw new System.Exception("上一次版本号不能为空");
                }
                var versions = Config.lastVersion.Split('.');
                if (versions.Length != 2)
                {
                    throw new System.Exception("上一次版本号格式错误");
                }
                if (!int.TryParse(versions[0], out var major) || !int.TryParse(versions[1], out var minor))
                {
                    throw new System.Exception("上一次版本号格式错误");
                }
                if (major < 0 || minor < 0)
                {
                    throw new System.Exception("上一次版本号格式错误");
                }
            }
            // 打包平台检查
            if (Config.target == BuildTarget.NoTarget)
            {
                throw new System.Exception("打包平台不能为空");
            }
            // 打包选项检查
            if (Config.option == BuildAssetBundleOptions.None)
            {
                throw new System.Exception("打包选项不能为空");
            }
            // TODO 远端上传地址检查
            // 文件收集检查
            if (Config.fileCollectors.Count == 0)
            {
                throw new System.Exception("文件收集不能为空");
            }
            if (Config.fileCollectors.WhereArray(x => x.collectedFiles.Count != 0).Length == 0)
            {
                throw new System.Exception("没有收集到任何文件");
            }

            return base.DoExecute();
        }
    }
}