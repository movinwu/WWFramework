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
        [ReadOnly] public BuildTarget target = BuildTarget.Android;

        /// <summary>
        /// 打包选项
        /// </summary>
        [ReadOnly] public BuildAssetBundleOptions option = BuildAssetBundleOptions.ChunkBasedCompression;

        /// <summary>
        /// 输出目录
        /// </summary>
        [ReadOnly, SerializeField] private string outputDir = _defaultOutputDir;

        /// <summary>
        /// 默认输出目录
        /// </summary>
        private static string _defaultOutputDir = string.Empty;

        /// <summary>
        /// 所有包信息收集器
        /// </summary>
        [ReadOnly] public List<AssetBundleInfoCollector> fileCollectors = new List<AssetBundleInfoCollector>();

        /// <summary>
        /// 版本类型
        /// </summary>
        [ReadOnly] public EAssetBundleVersionType versionType = EAssetBundleVersionType.None;

        /// <summary>
        /// 指定版本号
        /// </summary>
        [ReadOnly] public string specificVersion = "0.1";

        /// <summary>
        /// 上次打包版本号
        /// </summary>
        [ReadOnly] public string lastVersion = "0.0";

        /// <summary>
        /// 包文件是否复制到StreamingAssets下
        /// </summary>
        [ReadOnly] public bool copyToStreamingAssets = false;

        /// <summary>
        /// 远程上传地址
        /// </summary>
        [ReadOnly] public string uploadRemoteUrl = string.Empty;

        /// <summary>
        /// 是否引用分析
        /// </summary>
        [ReadOnly] public bool analyze = false;

        /// <summary>
        /// 引用分析阈值(被引用次数大于等于这个值,会单独打包)
        /// </summary>
        [ReadOnly] public int analyzeLimit = 1;

        /// <summary>
        /// 包分析器
        /// </summary>
        [HideInInspector] public AssetBundleBuildAnalyzer Analyzer;

        /// <summary>
        /// 额外shader变体信息(自动收集shader变体会遗漏的各种变体)
        /// </summary>
        [ReadOnly] public List<ShaderVariantInfo> extraShaderVariant = new List<ShaderVariantInfo>();

        /// <summary>
        /// 打包后的包清单文件
        /// </summary>
        [HideInInspector] public AssetBundleManifest assetBundleManifest;

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

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version
        {
            get
            {
                switch (versionType)
                {
                    case EAssetBundleVersionType.Specific:
                        return specificVersion;
                    case EAssetBundleVersionType.Increase:
                        if (!string.IsNullOrEmpty(lastVersion))
                        {
                            var lastVersions = lastVersion.Split('.');
                            if (lastVersions.Length == 2
                                && int.TryParse(lastVersions[0], out var major)
                                && major >= 0
                                && int.TryParse(lastVersions[1], out var minor)
                                && minor >= 0)
                            {
                                return $"{major}.{minor + 1}";
                            }
                        }

                        return "0.1";
                    default:
                        return "0.0";
                }
            }
        }

        /// <summary>
        /// 构建输出目录
        /// </summary>
        public string BuildOutputDir => GetVersionPath(Version, target);

        /// <summary>
        /// 获取版本目录
        /// </summary>
        /// <param name="version"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public string GetVersionPath(string version, BuildTarget target)
        {
            return System.IO.Path.Combine(outputDir, target.ToString(), version).Replace("\\", "/");
        }

        /// <summary>
        /// 构建输出目录(原始文件)
        /// </summary>
        public string OriginalBuildOutputDir => $"{BuildOutputDir}/Original";

        /// <summary>
        /// 构建输出目录(MD5包名文件)
        /// </summary>
        public string Md5BuildOutputDir => $"{BuildOutputDir}/Md5";

        /// <summary>
        /// 构建输出目录(最终文件)
        /// </summary>
        public string FinalBuildOutputDir => $"{BuildOutputDir}/Final";

        /// <summary>
        /// StreamingAssets复制目录
        /// </summary>
        public string StreamingAssetsCopyDir => System.IO.Path
            .Combine(Application.streamingAssetsPath, target.ToString(), Version).Replace("\\", "/");

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
            if (string.IsNullOrEmpty(_defaultOutputDir))
            {
                _defaultOutputDir = $"{Application.dataPath}/../output";
            }

            if (_dirty)
            {
                _dirty = false;
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssetIfDirty(this);
                AssetDatabase.Refresh();
            }

            GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            {
                // 整体配置信息使用一个背景区域包裹
                EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.ExpandWidth(true));
                {
                    // 绘制打包平台
                    var newTarget = (BuildTarget)EditorGUILayout.EnumPopup("打包平台", target);
                    if (newTarget != target)
                    {
                        target = newTarget;
                        _dirty = true;
                    }

                    // 绘制打包选项
                    var newOption = (BuildAssetBundleOptions)EditorGUILayout.EnumFlagsField("打包选项", option);
                    if (newOption != option)
                    {
                        option = newOption;
                        _dirty = true;
                    }

                    // 绘制输出目录
                    EditorGUILayout.BeginHorizontal(GUILayout.Height(20), GUILayout.ExpandWidth(true));
                    {
                        EditorGUILayout.LabelField("输出目录", GUILayout.Width(150));
                        if (GUILayout.Button(outputDir, GUILayout.ExpandWidth(true)))
                        {
                            string selectedPath = EditorUtility.OpenFolderPanel("选择输出目录",
                                Application.dataPath, Application.dataPath);
                            if (!string.IsNullOrEmpty(selectedPath))
                            {
                                // 保存绝对路径
                                outputDir = selectedPath;
                                _dirty = true;
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    // 绘制版本类型
                    var newVersionType = (EAssetBundleVersionType)EditorGUILayout.EnumPopup("版本类型", versionType);
                    if (newVersionType != versionType)
                    {
                        versionType = newVersionType;
                        _dirty = true;
                    }

                    if (versionType == EAssetBundleVersionType.Specific)
                    {
                        // 绘制版本号
                        var newVersion = EditorGUILayout.TextField("版本号", specificVersion);
                        if (newVersion != specificVersion)
                        {
                            specificVersion = newVersion;
                            _dirty = true;
                        }
                    }
                    else if (versionType == EAssetBundleVersionType.Increase)
                    {
                        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Height(20));
                        {
                            // 绘制递增版本号
                            EditorGUILayout.LabelField("递增版本号", GUILayout.Width(150));
                            EditorGUILayout.LabelField(Version, GUILayout.ExpandWidth(true));
                        }
                        GUILayout.EndHorizontal();
                    }

                    // 绘制是否复制到StreamingAssets下和上传远程地址
                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Height(20));
                    {
                        GUILayout.Label("复制到StreamingAssets下", GUILayout.Width(150));
                        var newCopyToStreamingAssets = EditorGUILayout.Toggle(copyToStreamingAssets);
                        if (newCopyToStreamingAssets != copyToStreamingAssets)
                        {
                            copyToStreamingAssets = newCopyToStreamingAssets;
                            _dirty = true;
                        }

                        GUILayout.Label("上传远程地址", GUILayout.Width(150));
                        var newUploadRemoteUrl = EditorGUILayout.TextField(uploadRemoteUrl);
                        if (newUploadRemoteUrl != uploadRemoteUrl)
                        {
                            uploadRemoteUrl = newUploadRemoteUrl;
                            _dirty = true;
                        }
                    }
                    GUILayout.EndHorizontal();

                    // 绘制是否引用分析
                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Height(20));
                    {
                        GUILayout.Label("引用分析", GUILayout.Width(150));
                        var newAnalyze = EditorGUILayout.Toggle(analyze);
                        if (newAnalyze != analyze)
                        {
                            analyze = newAnalyze;
                            _dirty = true;
                        }

                        if (analyze)
                        {
                            var newAnalyzeLimit = EditorGUILayout.IntField("引用数量阈值", analyzeLimit);
                            newAnalyzeLimit = Mathf.Max(newAnalyzeLimit, 1);
                            if (newAnalyzeLimit != analyzeLimit)
                            {
                                analyzeLimit = newAnalyzeLimit;
                                _dirty = true;
                            }
                        }
                    }
                    GUILayout.EndHorizontal();

                    // 绘制按钮,向左移动,向右移动,移动到最前,移动到最后,删除
                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Height(20));
                    {
                        GUILayout.FlexibleSpace(); // 最右边留白
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
                    }
                    GUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.ExpandWidth(true));
                {
                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Height(20));
                    {
                        // 所有指定变体信息
                        GUILayout.Label("额外变体信息", GUILayout.Width(150));
                        var newShaderVariantCount = EditorGUILayout.IntField(
                            extraShaderVariant.Count,
                            GUILayout.ExpandWidth(true));
                        if (newShaderVariantCount != extraShaderVariant.Count)
                        {
                            // 移除多余的变体信息
                            for (int i = extraShaderVariant.Count - 1;
                                 i >= newShaderVariantCount;
                                 i--)
                            {
                                extraShaderVariant.RemoveAt(i);
                            }

                            // 增加不足的变体信息
                            for (int i = extraShaderVariant.Count;
                                 i < newShaderVariantCount;
                                 i++)
                            {
                                extraShaderVariant.Add(new ShaderVariantInfo());
                            }

                            _dirty = true;
                        }
                    }
                    GUILayout.EndHorizontal();

                    for (int i = 0; i < extraShaderVariant.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Height(20));
                        {
                            EditorGUILayout.LabelField($"变体信息{i}:", GUILayout.Width(150));
                            
                            var shaderVariantInfo = extraShaderVariant[i];
                            if (GUILayout.Button(shaderVariantInfo.ShaderPath, GUILayout.Width(400)))
                            {
                                string selectedPath = EditorUtility.OpenFilePanelWithFilters("选择shader",
                                    Application.dataPath, new string[] { "shader", "shader" });
                                if (!string.IsNullOrEmpty(selectedPath) &&
                                    selectedPath.StartsWith(Application.dataPath))
                                {
                                    shaderVariantInfo.ShaderPath =
                                        "Assets" + selectedPath.Replace(Application.dataPath, "");
                                    _dirty = true;
                                }
                            }

                            var shaderVariantCount = EditorGUILayout.IntField(
                                shaderVariantInfo.ShaderVariants.Count,
                                GUILayout.Width(20));
                            if (shaderVariantCount != shaderVariantInfo.ShaderVariants.Count)
                            {
                                for (int k = shaderVariantInfo.ShaderVariants.Count - 1;
                                     k >= shaderVariantCount;
                                     k--)
                                {
                                    shaderVariantInfo.ShaderVariants.RemoveAt(k);
                                }

                                for (int k = shaderVariantInfo.ShaderVariants.Count;
                                     k < shaderVariantCount;
                                     k++)
                                {
                                    shaderVariantInfo.ShaderVariants.Add(string.Empty);
                                }

                                _dirty = true;
                            }

                            for (int j = 0; j < shaderVariantInfo.ShaderVariants.Count; j++)
                            {
                                var index = j;
                                var shaderVariant = shaderVariantInfo.ShaderVariants[index];
                                var newShaderVariant = EditorGUILayout.TextField(shaderVariant);
                                if (newShaderVariant != shaderVariant)
                                {
                                    shaderVariantInfo.ShaderVariants[index] = newShaderVariant;
                                    _dirty = true;
                                }
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();

                // 绘制+按钮和-按钮,增加一条新收集器和移除当前选中的收集器
                GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Height(20));
                {
                    GUILayout.FlexibleSpace(); // 最右边留白
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
                }
                GUILayout.EndHorizontal();

                // 使用滚动视图绘制所有收集器
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.ExpandWidth(true),
                    GUILayout.ExpandHeight(true));
                {
                    for (int i = 0; i < fileCollectors.Count; i++)
                    {
                        var collector = fileCollectors[i];

                        // 绘制收集器
                        collector.OnDrawGUI(this, _curSelectedIndex == i);
                    }
                }
                EditorGUILayout.EndScrollView();

                // 打包按钮
                if (GUILayout.Button("打包资源"))
                {
                    AssetBundleBuild().Forget();
                }
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
            // 不打包,不执行
            if (this.versionType == EAssetBundleVersionType.None)
            {
                return;
            }

            // 打包使用顺序流程执行
            var buildProcedure = new SequenceProcedure();
            // 窗口显示正在检查参数
            EditorUtility.DisplayProgressBar("AB包构建", "正在检查参数", 0.1f);
            // 检查相关参数
            buildProcedure.AddProcedure(new AssetBundleBuildProcedureCheck(this));
            // 窗口显示正在导表
            EditorUtility.DisplayProgressBar("AB包构建", "正在导表", 0.2f);
            // 数据表等重新生成
            buildProcedure.AddProcedure(new AssetBundleBuildProcedureDataTable(this));
            // 窗口显示正在清理打包信息
            EditorUtility.DisplayProgressBar("AB包构建", "正在清理打包信息", 0.3f);
            // 清理原有打包信息
            buildProcedure.AddProcedure(new AssetBundleBuildProcedureClear(this));
            // 窗口显示正在检查相关文件夹
            EditorUtility.DisplayProgressBar("AB包构建", "正在检查相关文件夹", 0.4f);
            // 相关文件夹检查
            buildProcedure.AddProcedure(new AssetBundleBuildProcedureFolder(this));
            // 窗口显示正在检查相关文件
            EditorUtility.DisplayProgressBar("AB包构建", "正在检查相关文件", 0.5f);
            // 相关文件检查
            buildProcedure.AddProcedure(new AssetBundleBuildProcedureFile(this));
            // 窗口显示正在生成shader变体
            EditorUtility.DisplayProgressBar("AB包构建", "正在生成shader变体", 0.6f);
            // shader收集及shader变体生成
            buildProcedure.AddProcedure(new AssetBundleBuildProcedureShader(this));
            // 窗口显示正在收集所有要打包文件信息
            EditorUtility.DisplayProgressBar("AB包构建", "正在收集所有要打包文件信息", 0.7f);
            // 收集所有要打包文件信息
            buildProcedure.AddProcedure(new AssetBundleBuildProcedureInfoCollect(this));
            // 窗口显示正在分析所有要打包文件信息
            EditorUtility.DisplayProgressBar("AB包构建", "正在分析所有要打包文件信息", 0.8f);
            // 分析所有要打包文件信息
            buildProcedure.AddProcedure(new AssetBundleBuildProcedureInfoAnalyze(this));
            // 窗口显示正在打包
            EditorUtility.DisplayProgressBar("AB包构建", "正在打包", 0.9f);
            // 正式打包
            buildProcedure.AddProcedure(new AssetBundleBuildProcedureBuild(this));
            // 窗口显示正在打包后处理
            EditorUtility.DisplayProgressBar("AB包构建", "正在打包后处理", 0.9f);
            // 打包后处理
            buildProcedure.AddProcedure(new AssetBundleBuildProcedurePostProcess(this));
            // 窗口显示正在拷贝本地
            EditorUtility.DisplayProgressBar("AB包构建", "正在拷贝本地", 0.9f);
            // 本地文件拷贝
            buildProcedure.AddProcedure(new AssetBundleBuildProcedureLocalFile(this));
            // 窗口显示正在上传远端
            EditorUtility.DisplayProgressBar("AB包构建", "正在上传远端", 0.9f);
            // 远端文件拷贝
            buildProcedure.AddProcedure(new AssetBundleBuildProcedureRemoteFile(this));

            // 正式执行
            try
            {
                await buildProcedure.Execute();

                // 打包完成后记录上次打包版本号
                lastVersion = Version;
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssetIfDirty(this);
                AssetDatabase.Refresh();

                // 打开打包文件夹
                EditorUtility.RevealInFinder(BuildOutputDir);
            }
            catch (Exception e)
            {
                // 弹窗
                var errorMsg =
                    $"打包出错, 当前打包流程:{buildProcedure.CurrentExecutingProcedure.GetType()}错误信息:{e.Message}\n{e.StackTrace}";
                EditorUtility.DisplayDialog("打包出错", errorMsg, "确定");
                Log.LogError(x => { x.Append(errorMsg); }, ELogType.Resource);
            }

            EditorUtility.ClearProgressBar();
        }
    }
}