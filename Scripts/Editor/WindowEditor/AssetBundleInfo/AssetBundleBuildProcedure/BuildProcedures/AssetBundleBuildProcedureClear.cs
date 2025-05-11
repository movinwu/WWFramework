/*------------------------------
 * 脚本名称: AssetBundleBuildProcedureClear
 * 创建者: movin
 * 创建日期: 2025/04/09
------------------------------*/

using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// AB构建流程-清理已有的打包信息
    /// </summary>
    public class AssetBundleBuildProcedureClear : AssetBundleBuildProcedureBase
    {
        public AssetBundleBuildProcedureClear(AssetBundleInfoConfig config) : base(config)
        {
        }

        protected override UniTask DoExecute()
        {
            // 清理项目中所有AB包构建信息
            var allAssetBundleNames = AssetDatabase.GetAllAssetBundleNames();
            allAssetBundleNames.ForEach(name => AssetDatabase.RemoveAssetBundleName(name, true));
            return base.DoExecute();
        }
    }
}