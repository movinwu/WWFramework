/*------------------------------
 * 脚本名称: ScriptTemplatePostProcessor
 * 创建者: movin
 * 创建日期: 2025/02/16
------------------------------*/

using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;

namespace WWFramework
{
    /// <summary>
    /// 通过自定义模板创建脚本的后处理
    /// </summary>
    public class ScriptTemplatePostProcessor : AssetModificationProcessor
    {
        private static void OnWillCreateAsset(string path)
        {
            // 只处理 C# 脚本
            if (!path.EndsWith(".cs.meta")) return;

            // 获取脚本路径
            string scriptPath = path.Replace(".meta", "");
            string scriptContent = File.ReadAllText(scriptPath);

            // 替换占位符
            scriptContent = Regex.Replace(scriptContent, "#CREATIONDATE#", System.DateTime.Now.ToString("yyyy/MM/dd"));

            // 写回文件
            File.WriteAllText(scriptPath, scriptContent);
            AssetDatabase.Refresh();
        }
    }
}