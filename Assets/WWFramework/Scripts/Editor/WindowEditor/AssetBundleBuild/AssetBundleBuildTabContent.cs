/*------------------------------
 * 脚本名称: ResourcesTabContent
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
    /// AB构建页签内容
    /// </summary>
    public class AssetBundleBuildTabContent : GlobalEditorTabContentBase
    {
        /// <summary>
        /// 所有资源构建配置
        /// </summary>
        private List<ITab> _configTabs;

        /// <summary>
        /// 资源页签组
        /// </summary>
        private readonly TabGroup _tabGroup = new TabGroup();

        /// <summary>
        /// 资源页签组滚动条位置
        /// </summary>
        private Vector2 _scrollPosition;

        /// <summary>
        /// 存储页签名称的键
        /// </summary>
        private const string LastTabNameKey = "AssetBundleBuildTabContent_LastTabName";

        /// <summary>
        /// 新建配置名称
        /// </summary>
        private string _newConfigName = string.Empty;

        public override UniTask OnInit()
        {
            // 从指定路径下使用AssetDataBase读取所有AssetBundleBuildConfig
            _configTabs =
                AssetDatabaseEx.GetAllAssets<AssetBundleBuildConfig>(GlobalEditorStringDefine
                    .AssetBundleBuildConfigFolderPath)
                    .SelectList<AssetBundleBuildConfig, ITab>(asset => new AssetBundleBuildConfigTab(asset));

            if (_configTabs.Count > 0)
            {
                // 读取上次存储的页签名称
                string lastTabName = EditorPrefs.GetString(LastTabNameKey, _configTabs[0].Name);
            
                // 找到对应的页签下标
                int lastTabIndex = _configTabs.FindIndex(tab => tab.Name == lastTabName);
                _tabGroup.Init(_configTabs, lastTabIndex).Forget();
            }
            return UniTask.CompletedTask;
        }

        public override UniTask OnDestroy()
        {
            return UniTask.CompletedTask;
        }

        public override void OnGUI()
        {
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            // 第一行：创建配置区域
            GUILayout.BeginHorizontal();
            
            GUILayout.FlexibleSpace(); // 将控件推到右侧
            // 文本输入框
            _newConfigName = EditorGUILayout.TextField(_newConfigName, GUILayout.Width(200));
            // 创建按钮
            if (GUILayout.Button("新建配置", GUILayout.Width(120)))
            {
                if (string.IsNullOrEmpty(_newConfigName))
                {
                    // 弹窗提示
                    EditorUtility.DisplayDialog("错误", "请输入配置名称", "确定");
                }
                else
                {
                    // 创建并保存新配置
                    var config = ScriptableObject.CreateInstance<AssetBundleBuildConfig>();
                    string path =
                        $"{GlobalEditorStringDefine.AssetBundleBuildConfigFolderPath}/{_newConfigName}.asset";
                    AssetDatabase.CreateAsset(config, path);
                    AssetDatabase.SaveAssets();

                    _configTabs.Add(new AssetBundleBuildConfigTab(config));
                    
                    // 刷新显示,并选中新建的页签
                    _tabGroup.Init(_configTabs, _configTabs.Count - 1).Forget();
                    // 保存当前页签名称
                    EditorPrefs.SetString(LastTabNameKey, _configTabs[^1].Name);
                }
            }
            GUILayout.EndHorizontal();

            // 有配置文件且页签组已经初始化后再绘制页签
            if (_configTabs.Count != 0
                && (_tabGroup.State == ETabGroupState.Showing
                    || _tabGroup.State == ETabGroupState.Switching))
            {
                DrawTabs();
                
                DrawSelectedTabContent();
            }
            
            GUILayout.EndVertical();
        }

        /// <summary>
        /// 绘制页签
        /// </summary>
        private void DrawTabs()
        {
            // 第二行：Tab页签滚动区域
            EditorGUILayout.BeginHorizontal(GUILayout.Height(50), GUILayout.ExpandWidth(true));
            var curTab = _configTabs[_tabGroup.CurrentIndex];
            
            // 添加滚动视图
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, 
                GUILayout.ExpandWidth(true), 
                GUILayout.Height(30)); // 设置固定高度
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
            for (int i = 0; i < _configTabs.Count; i++)
            {
                var tab = _configTabs[i];
                var isSelected = tab == curTab;
                // 根据是否选中设置背景颜色
                GUI.backgroundColor = isSelected ? Color.green : Color.gray;
                EditorGUILayout.BeginHorizontal("box", GUILayout.ExpandHeight(true), GUILayout.Width(100));
                // 添加点击事件处理
                if (GUILayout.Button(_configTabs[i].Name, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    _tabGroup.SwitchTab(i).Forget();
                    // 保存当前页签名称
                    EditorPrefs.SetString(LastTabNameKey, _configTabs[i].Name);
                }
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 绘制选中的页签内容
        /// </summary>
        private void DrawSelectedTabContent()
        {
            GUI.backgroundColor = Color.gray;
            // 绘制当前选中Tab的具体内容
            if (_tabGroup.CurrentIndex >= 0 
                && _tabGroup.CurrentIndex < _configTabs.Count
                && _configTabs[_tabGroup.CurrentIndex] is AssetBundleBuildConfigTab content)
            {
                content.Config.OnDrawGUI(this);
            }
        }

        /// <summary>
        /// 移除页签
        /// </summary>
        /// <param name="tab"></param>
        public void RemoveTab()
        {
            // 删除页签对应配置
            AssetDatabase.DeleteAsset($"{GlobalEditorStringDefine.AssetBundleBuildConfigFolderPath}/{_configTabs[_tabGroup.CurrentIndex].Name}.asset");
            AssetDatabase.Refresh();
            _configTabs.RemoveAt(_tabGroup.CurrentIndex);
            if (_configTabs.Count > 0)
            {
                var index = Mathf.Clamp(_tabGroup.CurrentIndex, 0, _configTabs.Count - 1);
                _tabGroup.Init(_configTabs, index).Forget();
                // 保存当前页签名称
                EditorPrefs.SetString(LastTabNameKey, _configTabs[index].Name);
            }
        }

        /// <summary>
        /// 移动页签
        /// </summary>
        /// <param name="moveLeft">是否向左移动</param>
        /// <param name="moveFirst">是否移动到最前</param>
        public void MoveTab(bool moveLeft, bool moveFirst)
        {
            var index = _tabGroup.CurrentIndex;
            // 向左移动
            if (moveLeft)
            {
                // 不在最左边才能向左移动
                if (index > 0)
                {
                    // 如果移动到最前，则目标页签索引为0,否则为当前页签索引减1
                    int moveToIndex = 0;
                    if (!moveFirst)
                    {
                        moveToIndex = index - 1;
                    }
                    // 交换两个页签
                    _configTabs.Swap(index, moveToIndex);
                    // 刷新
                    _tabGroup.Init(_configTabs, moveToIndex).Forget();
                }
            }
            // 向右移动
            else
            {
                // 不在最右边才能向右移动
                if (index < _configTabs.Count - 1)
                {
                    // 如果移动到最前，则目标页签索引为列表长度减1,否则为当前页签索引加1
                    int moveToIndex = _configTabs.Count - 1;
                    if (!moveFirst)
                    {
                        moveToIndex = index + 1;
                    }
                    // 交换两个页签
                    _configTabs.Swap(index, moveToIndex);
                    // 刷新
                    _tabGroup.Init(_configTabs, moveToIndex).Forget();
                }
            }
        }
    }
}