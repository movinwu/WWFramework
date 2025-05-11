/*------------------------------
 * 脚本名称: ScriptTemplate
 * 创建者: movin
 * 创建日期: 2025/02/16
------------------------------*/

using UnityEditor;

namespace WWFramework
{
    /// <summary>
    /// Unity脚本模板创建
    /// </summary>
    public class ScriptTemplate
    {
        [MenuItem(GlobalEditorStringDefine.CommonScriptTemplate, false, GlobalEditorPriorityDefine.CommonScriptTemplate)]
        private static void CreateCommonScript()
        {
            // 调用 Unity 的内置方法创建脚本
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                "Assets/WWFramework/Scripts/Editor/ScriptTemplate/Templates/CommonScriptTemplate.cs.txt",
                "NewCommonScript.cs");
        }
    
        [MenuItem(GlobalEditorStringDefine.WWFrameworkScriptTemplate, false, GlobalEditorPriorityDefine.WWFrameworkScriptTemplate)]
        private static void CreateWWFrameworkScript()
        {
            // 调用 Unity 的内置方法创建脚本
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                "Assets/WWFramework/Scripts/Editor/ScriptTemplate/Templates/WWFrameworkScriptTemplate.cs.txt",
                "NewWWFrameworkScript.cs");
        }
    }
}