/*------------------------------
 * 脚本名称: GlobalEditorWindow
 * 创建者: movin
 * 创建日期: 2025/04/05
------------------------------*/

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 全局编辑窗口
    /// </summary>
    public class GlobalEditorWindow : EditorWindow
    {
        /// <summary>
        /// 全局标签组
        /// </summary>
        private readonly TabGroup _tabGroup = new TabGroup();
        
        /// <summary>
        /// 所有页签
        /// </summary>
        private readonly List<GlobalEditorTabBase> _tabs = new List<GlobalEditorTabBase>()
        {
            new AssetBundleBuildTab(),
        };
        
        /// <summary>
        /// 滚动视图位置
        /// </summary>
        private Vector2 _scrollPosition;
        
        /// <summary>
        /// 存储页签名称的键
        /// </summary>
        private const string LastTabNameKey = "GlobalEditorWindow_LastTabName";

        // 添加菜单项
        [MenuItem(
            GlobalEditorStringDefine.GlobalEditorWindow, 
            priority = GlobalEditorPriorityDefine.GlobalEditorWindow)]
        private static void ShowWindow()
        {
            var window = GetWindow<GlobalEditorWindow>();
            window.titleContent = new GUIContent("Global Editor");
            window.minSize = new Vector2(300, 200);
            window.Init();
        }

        private void Init()
        {
            // 检验页签列表
            if (null == _tabs || _tabs.Count == 0)
            {
                Log.LogWarning(sb =>
                {
                    sb.AppendLine("全局编辑器页签列表为空,请先添加页签");
                });
                return;
            }
            
            // 读取上次存储的页签名称
            string lastTabName = EditorPrefs.GetString(LastTabNameKey, _tabs[0].Name);
            
            // 找到对应的页签下标
            int lastTabIndex = _tabs.FindIndex(tab => tab.Name == lastTabName);

            _tabGroup.Init(_tabs.SelectList(x => (ITab)x), lastTabIndex).Forget();
        }

        private void OnGUI()
        {
            // 没有显示标签组时，不显示窗口
            if (_tabGroup.State == ETabGroupState.None || _tabGroup.State == ETabGroupState.Initializing)
            {
                return;
            }

            // 使用水平布局
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            
            // 左侧显示所有tab
            DrawTabs();
            
            // 右侧显示当前选中的tab内容
            DrawSelectedTabContent();
            
            EditorGUILayout.EndHorizontal();
        }

        private void DrawTabs()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(150), GUILayout.ExpandHeight(true));
            var curTab = _tabs[_tabGroup.CurrentIndex];
            
            // 添加滚动视图
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.ExpandHeight(true));
            for (int i = 0; i < _tabs.Count; i++)
            {
                var tab = _tabs[i];
                var isSelected = tab == curTab;
                // 根据是否选中设置背景颜色
                GUI.backgroundColor = isSelected ? Color.green : Color.gray;
                EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true), GUILayout.Height(30));
                // 添加点击事件处理
                if (GUILayout.Button(_tabs[i].Name, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    _tabGroup.SwitchTab(i).Forget();
                    // 保存当前页签名称
                    EditorPrefs.SetString(LastTabNameKey, _tabs[i].Name);
                }
                tab.OnGUI(isSelected);
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void DrawSelectedTabContent()
        {
            GUI.backgroundColor = Color.gray;
            EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            var curTab = _tabs[_tabGroup.CurrentIndex];
            if (curTab.Content is GlobalEditorTabContentBase content)
            {
                content.OnDrawGUI();
            }
            EditorGUILayout.EndVertical();
        }
    }
}
