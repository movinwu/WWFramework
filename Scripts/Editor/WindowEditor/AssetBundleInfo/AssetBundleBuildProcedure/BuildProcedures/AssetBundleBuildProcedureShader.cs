/*------------------------------
 * 脚本名称: AssetBundleBuildProcedureShader
 * 创建者: movin
 * 创建日期: 2025/04/09
------------------------------*/

using System.IO;
using Cysharp.Threading.Tasks;
using UnityEditor;
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
            // 每个指定的额外shader变体信息,都生成一个不打包的临时材质
            var tempMaterialFolderPath = GameEntry.GlobalGameConfig.resourceConfig.extraShaderVariantMaterialPath;
            // 使用C#文件操作直接删除文件夹下的所有文件,不考虑资源依赖关系
            var directoryPath =
                $"{Application.dataPath.Substring(0, Application.dataPath.Length - 6)}{tempMaterialFolderPath}";
            // 删除临时文件夹下所有文件
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
            Directory.CreateDirectory(directoryPath);
            
            // 创建材质
            foreach (var shaderVariantInfo in Config.extraShaderVariant)
            {
                var shader = AssetDatabase.LoadAssetAtPath<Shader>(shaderVariantInfo.ShaderPath);
                if (null == shader)
                {
                    continue;
                }
                var material = new Material(shader);
                // 材质变体设置
                material.shaderKeywords = shaderVariantInfo.ShaderVariants.ToArray();
                // 材质保存路径
                var materialPath = $"{tempMaterialFolderPath}/{Path.GetFileNameWithoutExtension(shaderVariantInfo.ShaderPath)}_{string.Join("_", shaderVariantInfo.ShaderVariants)}_{shaderVariantInfo.GetHashCode()}.mat";
                // 保存材质
                AssetDatabase.CreateAsset(material, materialPath);
            }
            return UniTask.CompletedTask;
        }
    }
}