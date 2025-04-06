/*------------------------------
 * 脚本名称: AssetBundleBuildConfig
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
    /// AB构建配置,一个配置对应一个AB资源整包
    /// <para> 在编辑器中通过按钮创建和删除 </para>
    /// </summary>
    // [CreateAssetMenu(
    //     fileName = GlobalEditorStringDefine.AssetBundleBuildConfigName, 
    //     menuName = GlobalEditorStringDefine.AssetBundleBuildConfig,
    //     order = GlobalEditorPriorityDefine.AssetBundleBuildConfig)]
    public class AssetBundleBuildConfig : ScriptableObject, ITabContent
    {
        /// <summary>
        /// 所有包收集器
        /// </summary>
        [ReadOnly]
        public List<AssetBundleBuildCollector> FileCollectors = new List<AssetBundleBuildCollector>();

        /// <summary>
        /// 当前选中的下标
        /// </summary>
        private int _curSelectedIndex;

        /// <summary>
        /// 选中的下标保存的key
        /// </summary>
        private string _selectedIndexSaveKey;

        /// <summary>
        /// 滚动视图位置
        /// </summary>
        private Vector2 _scrollPosition;

        public ITab Tab { get; set; }
        public UniTask OnInit()
        {
            this._selectedIndexSaveKey = $"AssetBundleBuildConfig_{Tab.Name}_SelectedIndex";
            this._curSelectedIndex = EditorPrefs.GetInt(_selectedIndexSaveKey, 0);
            
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

        /// <summary>
        /// 绘制配置
        /// </summary>
        /// <param name="content"></param>
        public void OnDrawGUI(AssetBundleBuildTabContent content)
        {
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            
            // 第一行绘制按钮,向左移动,向右移动,移动到最前,移动到最后,删除
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Height(20));
            
            GUILayout.FlexibleSpace();// 最右边留白
            if (GUILayout.Button("←", GUILayout.Width(50)))
            {
                content.MoveTab(true, false);
            }
            if (GUILayout.Button("→", GUILayout.Width(50)))
            {
                content.MoveTab(false, false);
            }
            if (GUILayout.Button("|←", GUILayout.Width(50)))
            {
                content.MoveTab(true, true);
            }
            if (GUILayout.Button("→|", GUILayout.Width(50)))
            {
                content.MoveTab(false, true);
            }
            
            GUILayout.EndHorizontal();
            
            // 第二行行绘制+按钮和-按钮,增加一条新收集器和移除当前选中的收集器
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Height(20));
            
            GUILayout.FlexibleSpace();// 最右边留白
            if (GUILayout.Button("+", GUILayout.Width(50)))
            {
                var newCollector = new AssetBundleBuildCollector();
                FileCollectors.Add(newCollector);
                _curSelectedIndex = FileCollectors.Count - 1;
                EditorPrefs.SetInt(_selectedIndexSaveKey, _curSelectedIndex);
            }
            if (GUILayout.Button("-", GUILayout.Width(50)))
            {
                if (_curSelectedIndex >= 0 && _curSelectedIndex < FileCollectors.Count)
                {
                    FileCollectors.RemoveAt(_curSelectedIndex);
                    if (_curSelectedIndex >= FileCollectors.Count)
                    {
                        _curSelectedIndex = FileCollectors.Count - 1;
                    }
                    EditorPrefs.SetInt(_selectedIndexSaveKey, _curSelectedIndex);
                }
            }
            
            GUILayout.EndHorizontal();
            
            // 使用滚动视图绘制所有收集器
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            for (int i = 0; i < FileCollectors.Count; i++)
            {
                var collector = FileCollectors[i];
                
                // 绘制收集器
                collector.OnDrawGUI(this, _curSelectedIndex == i);
            }
            EditorGUILayout.EndScrollView();
            
            GUILayout.EndVertical();
        }

        /// <summary>
        /// 切换选中的收集器
        /// </summary>
        /// <param name="collector"></param>
        public void ChangeSelect(AssetBundleBuildCollector collector)
        {
            FileCollectors.BreakForEach(x =>
            {
                if (x == collector)
                {
                    _curSelectedIndex = FileCollectors.IndexOf(x);
                    EditorPrefs.SetInt(_selectedIndexSaveKey, _curSelectedIndex);
                    return true;
                }
                return false;
            });
        }
    }
}