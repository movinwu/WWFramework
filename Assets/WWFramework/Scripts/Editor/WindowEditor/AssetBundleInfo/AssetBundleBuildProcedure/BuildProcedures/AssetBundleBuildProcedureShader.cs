/*------------------------------
 * 脚本名称: AssetBundleBuildProcedureShader
 * 创建者: movin
 * 创建日期: 2025/04/09
------------------------------*/

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// AB包构建流程-shader变体
    /// </summary>
    public class AssetBundleBuildProcedureShader : AssetBundleBuildProcedureBase
    {
        public AssetBundleBuildProcedureShader(AssetBundleInfoConfig config) : base(config)
        {
        }
        
        protected override UniTask DoExecute()
        {
            return base.DoExecute();
        }
    }
}