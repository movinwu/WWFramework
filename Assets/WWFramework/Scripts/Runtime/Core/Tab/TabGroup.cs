/*------------------------------
 * 脚本名称: TabGroup
 * 创建者: movin
 * 创建日期: 2025/03/18
------------------------------*/

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 标签组,处理一组标签切换等逻辑
    /// </summary>
    public class TabGroup
    {
        /// <summary>
        /// 标签组下的所有标签
        /// </summary>
        private readonly List<ITab> _tabs = new();

        /// <summary>
        /// 新的标签组
        /// </summary>
        private List<ITab> _newTabs;
        
        /// <summary>
        /// 当前标签索引
        /// </summary>
        public int CurrentIndex { get; private set; } = -1;
        
        /// <summary>
        /// 新标签下标
        /// </summary>
        private int _newIndex = -1;
        
        /// <summary>
        /// 当前标签
        /// </summary>
        public ITab CurrentTab => CurrentIndex >= 0 && CurrentIndex < _tabs.Count ? _tabs[CurrentIndex] : null;

        /// <summary>
        /// 标签切换中
        /// </summary>
        private bool _isTabSwitching = false;

        /// <summary>
        /// 重新排布标签委托
        /// </summary>
        public System.Action<List<ITab>> LayoutTabs;

        /// <summary>
        /// 初始化标签组
        /// </summary>
        /// <param name="tabs"></param>
        /// <param name="initIndex"></param>
        public async UniTask Init(List<ITab> tabs, int initIndex)
        {
            if (null == tabs || tabs.Count == 0)
            {
                Log.LogWarning(sb =>
                {
                    sb.AppendLine(nameof(TabGroup));
                    sb.Append(':');
                    sb.Append(nameof(Init));
                    sb.Append(':');
                    sb.Append(nameof(tabs));
                    sb.Append("为空");
                });
                return;
            }

            var newIndex = Mathf.Clamp(initIndex, 0, tabs.Count - 1);
            if (newIndex != initIndex)
            {
                Log.LogWarning(sb =>
                {
                    sb.AppendLine(nameof(TabGroup));
                    sb.Append(':');
                    sb.Append(nameof(Init));
                    sb.Append(':');
                    sb.Append(nameof(initIndex));
                    sb.Append("超出范围,已修正为:");
                    sb.AppendLine(initIndex.ToString());
                });
            }

            _newTabs = tabs;
            _newIndex = newIndex;
            await SwitchTab();
        }
        
        /// <summary>
        /// 切换标签
        /// </summary>
        /// <param name="index"></param>
        public async UniTask SwitchTab(int index)
        {
            if (null == _tabs || _tabs.Count == 0)
            {
                Log.LogWarning(sb =>
                {
                    sb.AppendLine(nameof(TabGroup));
                    sb.Append(':');
                    sb.Append(nameof(SwitchTab));
                    sb.Append(':');
                    sb.Append(nameof(_tabs));
                    sb.Append("为空");
                });
                return;
            }
            if (index < 0 || index >= _tabs.Count)
            {
                Log.LogWarning(sb =>
                    {
                        sb.AppendLine(nameof(TabGroup));
                        sb.Append(':');
                        sb.Append(nameof(SwitchTab));
                        sb.Append(':');
                        sb.Append(nameof(index));
                        sb.Append("超出范围");
                    });
                return;
            }
            _newIndex = index;
            await SwitchTab();
        }

        /// <summary>
        /// 重建tab,可以新增\删除或者修改tab次序等
        /// </summary>
        /// <param name="rebuildAction"></param>
        public async UniTask ReBuildTabs(System.Action<List<ITab>> rebuildAction)
        {
            if (null == rebuildAction)
            {
                Log.LogWarning(sb =>
                {
                    sb.AppendLine(nameof(TabGroup));
                    sb.Append(':');
                    sb.Append(nameof(ReBuildTabs));
                    sb.Append(':');
                    sb.Append(nameof(rebuildAction));
                    sb.Append("为空");
                });
                return;
            }

            if (null == _tabs)
            {
                Log.LogWarning(sb =>
                {
                    sb.AppendLine(nameof(TabGroup));
                    sb.Append(':');
                    sb.Append(nameof(ReBuildTabs));
                    sb.Append(':');
                    sb.Append(nameof(_tabs));
                    sb.Append("为空");
                });
                return;
            }
            
            // 记录当前tab
            var oldTab = CurrentTab;
            // 重构tab
            rebuildAction(_tabs);
            // 找到新的当前tab的下标
            var newIndex = -1;
            for (int i = 0; i < _tabs.Count; i++)
            {
                if (_tabs[i] == oldTab)
                {
                    newIndex = i;
                    break;
                }
            }
            // 切换页签
            _newIndex = newIndex;
            await SwitchTab();
        }
        
        /// <summary>
        /// 切换标签
        /// <para> 统一的切换页签逻辑,包括页签初始化的逻辑,只走一个函数,避免异步出现问题 </para>
        /// </summary>
        private async UniTask SwitchTab()
        {
            // 标签切换函数同一时间只能最多有一个在执行
            if (_isTabSwitching)
            {
                return;
            }
            
            _isTabSwitching = true;
            
            // 有新的标签需要展示
            if (null != _newTabs)
            {
                // 当前有正在隐藏的标签,先隐藏
                if (CurrentIndex >= 0 && CurrentIndex < _tabs.Count)
                {
                    var tabHideTask = _tabs[CurrentIndex].OnHide();
                    var contentHideTask = null == _tabs[CurrentIndex].Content ? UniTask.CompletedTask : _tabs[CurrentIndex].Content.OnHide();
                    await UniTask.WhenAll(tabHideTask, contentHideTask);
                }
                // 集中销毁所有旧标签
                var destroyTasks = new UniTask[_tabs.Count * 2 + _newTabs.Count];
                for (int i = 0; i < _tabs.Count; i++)
                {
                    var tab = _tabs[i];
                    destroyTasks[i * 2] = tab.OnDestroy();
                    destroyTasks[i * 2 + 1] = tab.Content?.OnDestroy() ?? UniTask.CompletedTask;
                }
                // 初始化新标签和旧标签销毁同时进行
                for (int i = 0; i < _newTabs.Count; i++)
                {
                    var tab = _newTabs[i];
                    tab.Index = i;
                    tab.Group = this;
                    destroyTasks[_tabs.Count * 2 + i] = tab.OnInit();
                }
                await UniTask.WhenAll(destroyTasks);
                // 新标签初始化完成后,进行标签排布
                LayoutTabs?.Invoke(_newTabs);
                // 重新排布后,执行onlayout
                await UniTask.WhenAll(_newTabs.SelectArray(tab => tab.OnLayout()));
                _tabs.Clear();
                _newTabs.ForEach(tab => _tabs.Add(tab));
                _newTabs = null;
                CurrentIndex = -1;
            }
            
            // 当前正在展示的标签和新标签下标不同
            if (CurrentIndex != _newIndex)
            {
                // 记录新下标
                var newIndex = _newIndex;
                // 旧标签隐藏
                if (CurrentIndex >= 0 && CurrentIndex < _tabs.Count)
                {
                    var tabHideTask = _tabs[CurrentIndex].OnHide();
                    var contentHideTask = null == _tabs[CurrentIndex].Content ? UniTask.CompletedTask : _tabs[CurrentIndex].Content.OnHide();
                    await UniTask.WhenAll(tabHideTask, contentHideTask);
                }
                // 展示新标签
                if (newIndex >= 0 && newIndex < _tabs.Count)
                {
                    // content 是否创建
                    if (null == _tabs[newIndex].Content)
                    {
                        _tabs[newIndex].Content = await _tabs[newIndex].CreateContent();
                        _tabs[newIndex].Content.Tab = _tabs[newIndex];
                        await _tabs[newIndex].Content.OnInit();
                        
                    }
                    var tabShowTask = _tabs[newIndex].OnShow();
                    var contentShowTask = null == _tabs[newIndex].Content ? UniTask.CompletedTask : _tabs[newIndex].Content.OnShow();
                    await UniTask.WhenAll(tabShowTask, contentShowTask);
                }
                // 更新当前标签下标
                CurrentIndex = newIndex;
                // 递归查看标签是否进行了切换
                _isTabSwitching = false;
                await SwitchTab();
            }
            
            _isTabSwitching = false;
        }
    }
}