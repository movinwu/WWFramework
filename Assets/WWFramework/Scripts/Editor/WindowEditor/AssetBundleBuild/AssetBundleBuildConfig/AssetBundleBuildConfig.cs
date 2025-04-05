/*------------------------------
 * 脚本名称: ResourcesConfig
 * 创建者: movin
 * 创建日期: 2025/04/05
------------------------------*/

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// AB构建配置,一个配置对应一个AB资源整包
    /// <para> 在编辑器中通过按钮创建和删除 </para>
    /// </summary>
    // [CreateAssetMenu(
    //     fileName = GlobalEditorStringDefine.AssetBundleBuildConfigName, 
    //     menuName = GlobalEditorStringDefine.AssetBundleBuildConfig,
    //     order = GlobalEditorPriorityDefine.AssetBundleBuildConfig)]
    public class AssetBundleBuildConfig : ScriptableObject, ITabContent
    {
        public ITab Tab { get; set; }
        public UniTask OnInit()
        {
            return UniTask.CompletedTask;
        }

        public UniTask OnShow()
        {
            return UniTask.CompletedTask;
        }

        public UniTask OnHide()
        {
            return UniTask.CompletedTask;
        }

        public UniTask OnDestroy()
        {
            return UniTask.CompletedTask;
        }

        public void OnDrawGUI(AssetBundleBuildTabContent content)
        {
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            
            // 第一行绘制按钮,向左移动,向右移动,移动到最前,移动到最后,删除
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Height(50));
            
            GUILayout.FlexibleSpace();// 最右边留白
            if (GUILayout.Button("向左移动", GUILayout.Width(100)))
            {
                content.MoveTab(true, false);
            }
            if (GUILayout.Button("向右移动", GUILayout.Width(100)))
            {
                content.MoveTab(false, false);
            }
            if (GUILayout.Button("移动到最前", GUILayout.Width(100)))
            {
                content.MoveTab(true, true);
            }
            if (GUILayout.Button("移动到最后", GUILayout.Width(100)))
            {
                content.MoveTab(false, true);
            }
            if (GUILayout.Button("删除", GUILayout.Width(100)))
            {
                content.RemoveTab();
            }
            
            GUILayout.EndHorizontal();
            
            // TODO
            
            GUILayout.EndVertical();
        }
    }
}