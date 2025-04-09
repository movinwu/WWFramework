/*------------------------------
 * 脚本名称: AssetBundleInfoConfig
 * 创建者: movin
 * 创建日期: 2025/04/05
------------------------------*/

using System;
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
    //     fileName = GlobalEditorStringDefine.AssetBundleInfoConfigName, 
    //     menuName = GlobalEditorStringDefine.AssetBundleInfoConfig,
    //     order = GlobalEditorPriorityDefine.AssetBundleInfoConfig)]
    public class AssetBundleInfoConfig : ScriptableObject, ITabContent
    {
        /// <summary>
        /// 打包平台
        /// </summary>
        [ReadOnly]
        public BuildTarget target = BuildTarget.Android;
        
        /// <summary>
        /// 打包选项
        /// </summary>
        [ReadOnly]
        public BuildAssetBundleOptions option = BuildAssetBundleOptions.ChunkBasedCompression;

        /// <summary>
        /// 输出目录
        /// </summary>
        [ReadOnly]
        public string outputDir = string.Empty;
        
        /// <summary>
        /// 所有包信息收集器
        /// </summary>
        [ReadOnly]
        public List<AssetBundleInfoCollector> fileCollectors = new List<AssetBundleInfoCollector>();
        
        /// <summary>
        /// 版本类型
        /// </summary>
        [ReadOnly]
        public EAssetBundleVersionType versionType = EAssetBundleVersionType.None;

        /// <summary>
        /// 版本号
        /// </summary>
        [ReadOnly]
        public string version = "0.1";

        /// <summary>
        /// 上次打包版本号
        /// </summary>
        [ReadOnly] 
        public string lastVersion = string.Empty;

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
        
        /// <summary>
        /// 是否有修改
        /// </summary>
        private bool _dirty = false;

        public ITab Tab { get; set; }
        public UniTask OnInit()
        {
            this._selectedIndexSaveKey = $"AssetBundleInfoConfig_{Tab.Name}_SelectedIndex";
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

        public UniTask OnRelease()
        {
            return UniTask.CompletedTask;
        }

        /// <summary>
        /// 绘制配置
        /// </summary>
        /// <param name="content"></param>
        public void OnDrawGUI(AssetBundleInfoTabContent content)
        {
            if (_dirty)
            {
                _dirty = false;
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssetIfDirty(this);
                AssetDatabase.Refresh();
            }
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            
            // 绘制打包平台
            var newTarget = (BuildTarget) EditorGUILayout.EnumPopup("打包平台", target);
            if (newTarget != target)
            {
                target = newTarget;
                _dirty = true;
            }
            // 绘制打包选项
            var newOption = (BuildAssetBundleOptions) EditorGUILayout.EnumFlagsField("打包选项", option);
            if (newOption != option)
            {
                option = newOption;
                _dirty = true;
            }
            // 绘制输出目录
            EditorGUILayout.LabelField("文件夹路径", GUILayout.Width(150));
            if (GUILayout.Button(outputDir, GUILayout.ExpandWidth(true)))
            {
                string selectedPath = EditorUtility.OpenFolderPanel("选择输出目录",
                    Application.dataPath, "");
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    // 保存绝对路径
                    outputDir = selectedPath;
                    _dirty = true;
                }
            }
            // 绘制版本类型
            var newVersionType = (EAssetBundleVersionType) EditorGUILayout.EnumPopup("版本类型", versionType);
            if (newVersionType != versionType)
            {
                versionType = newVersionType;
                _dirty = true;
            }
            if (versionType == EAssetBundleVersionType.Specific)
            {
                // 绘制版本号
                var newVersion = EditorGUILayout.TextField("版本号", version);
                if (newVersion != version)
                {
                    version = newVersion;
                    _dirty = true;
                }
            }
            else if (versionType == EAssetBundleVersionType.Increase)
            {
                GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Height(20));
                // 绘制递增版本号
                EditorGUILayout.LabelField("递增版本号", GUILayout.Width(150));
                EditorGUILayout.LabelField(string.IsNullOrEmpty(lastVersion) ? "0.1" : lastVersion, GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();
            }
            
            // 绘制按钮,向左移动,向右移动,移动到最前,移动到最后,删除
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
            
            // 绘制+按钮和-按钮,增加一条新收集器和移除当前选中的收集器
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Height(20));
            
            GUILayout.FlexibleSpace();// 最右边留白
            if (GUILayout.Button("+", GUILayout.Width(50)))
            {
                var newCollector = new AssetBundleInfoCollector();
                fileCollectors.Add(newCollector);
                _curSelectedIndex = fileCollectors.Count - 1;
                EditorPrefs.SetInt(_selectedIndexSaveKey, _curSelectedIndex);
            }
            if (GUILayout.Button("-", GUILayout.Width(50)))
            {
                if (_curSelectedIndex >= 0 && _curSelectedIndex < fileCollectors.Count)
                {
                    fileCollectors.RemoveAt(_curSelectedIndex);
                    if (_curSelectedIndex >= fileCollectors.Count)
                    {
                        _curSelectedIndex = fileCollectors.Count - 1;
                    }
                    EditorPrefs.SetInt(_selectedIndexSaveKey, _curSelectedIndex);
                }
            }
            
            GUILayout.EndHorizontal();
            
            // 使用滚动视图绘制所有收集器
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            for (int i = 0; i < fileCollectors.Count; i++)
            {
                var collector = fileCollectors[i];
                
                // 绘制收集器
                collector.OnDrawGUI(this, _curSelectedIndex == i);
            }
            EditorGUILayout.EndScrollView();
            
            // 打包按钮
            if (GUILayout.Button("打包资源"))
            {
                AssetBundleBuild().Forget();
            }
            
            GUILayout.EndVertical();
        }

        /// <summary>
        /// 切换选中的收集器
        /// </summary>
        /// <param name="collector"></param>
        public void ChangeSelect(AssetBundleInfoCollector collector)
        {
            fileCollectors.BreakForEach(x =>
            {
                if (x == collector)
                {
                    _curSelectedIndex = fileCollectors.IndexOf(x);
                    EditorPrefs.SetInt(_selectedIndexSaveKey, _curSelectedIndex);
                    return true;
                }
                return false;
            });
        }

        /// <summary>
        /// 构建ab包
        /// </summary>
        public async UniTaskVoid AssetBundleBuild()
        {
            // 打包使用顺序流程执行
            var buildProcedure = new SequenceProcedure();
            // 检查相关参数
            buildProcedure.AddProcedure(new AssetBundleBuildProcedureCheck(this));
            // 数据表等重新生成
            buildProcedure.AddProcedure(new AssetBundleBuildProcedureDataTable(this));
            // 清理原有打包信息
            buildProcedure.AddProcedure(new AssetBundleBuildProcedureClear(this));
            // 相关文件夹检查
            buildProcedure.AddProcedure(new AssetBundleBuildProcedureFolder(this));
            // 相关文件检查
            buildProcedure.AddProcedure(new AssetBundleBuildProcedureFile(this));
            // shader收集及shader变体生成
            buildProcedure.AddProcedure(new AssetBundleBuildProcedureShader(this));
            // 收集所有要打包文件信息
            buildProcedure.AddProcedure(new AssetBundleBuildProcedureInfoCollect(this));
            // 分析所有要打包文件信息
            buildProcedure.AddProcedure(new AssetBundleBuildProcedureInfoAnalyze(this));
            // 正式打包
            buildProcedure.AddProcedure(new AssetBundleBuildProcedureBuild(this));
            // 打包后处理
            buildProcedure.AddProcedure(new AssetBundleBuildProcedurePostProcess(this));
            // 本地文件拷贝
            buildProcedure.AddProcedure(new AssetBundleBuildProcedureLocalFile(this));
            // 远端文件拷贝
            buildProcedure.AddProcedure(new AssetBundleBuildProcedureRemoteFile(this));
            
            // 正式执行
            try
            {
                await buildProcedure.Execute();
            }
            catch (Exception e)
            {
                Log.LogError(x =>
                {
                    x.Append("打包出错, 当前打包流程:");
                    x.Append(buildProcedure.CurrentExecutingProcedure.GetType());
                    x.Append("错误信息:");
                    x.Append(e.Message);
                    x.Append(e.StackTrace);
                });
            }
        }
    }
}