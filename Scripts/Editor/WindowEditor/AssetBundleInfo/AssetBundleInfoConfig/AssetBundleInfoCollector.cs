/*------------------------------
 * 脚本名称: AssetBundleInfoCollector
 * 创建者: movin
 * 创建日期: 2025/04/06
------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Video;

namespace WWFramework
{
    /// <summary>
    /// AB包构建收集器
    /// </summary>
    [Serializable]
    public class AssetBundleInfoCollector
    {
        /// <summary>
        /// 文件夹路径
        /// </summary>
        public string folderPath = string.Empty;

        /// <summary>
        /// 忽略的正则表达式
        /// </summary>
        public string ignorePattern = string.Empty;

        /// <summary>
        /// 文件夹构建类型
        /// </summary>
        public ECollectorInfoType infoType = ECollectorInfoType.EachFile;

        /// <summary>
        /// 要打包的文件后缀
        /// </summary>
        public string fileExtensions = string.Empty;

        /// <summary>
        /// 包含的文件
        /// </summary>
        public List<string> collectedFiles;

        /// <summary>
        /// 是否需要重新收集
        /// </summary>
        private bool _dirty = true;

        /// <summary>
        /// 是否展开文件列表
        /// </summary>
        private bool _isExpanded = false;

        /// <summary>
        /// 绘制GUI
        /// </summary>
        /// <param name="config">所属配置文件</param>
        /// <param name="isSelected">是否被选中</param>
        public void OnDrawGUI(AssetBundleInfoConfig config, bool isSelected)
        {
            // 根据是否选中设置不同的背景颜色
            if (isSelected)
            {
                GUI.backgroundColor = Color.green; // 选中时设置为绿色
            }
            else
            {
                GUI.backgroundColor = Color.gray; // 未选中时设置为灰色
            }

            // 绘制一个背景区域，以便区分不同的收集器
            EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.ExpandWidth(true));
            {
                // 添加一个透明的按钮覆盖整个绘制区域，用于处理点击事件
                if (GUILayout.Button("", GUIStyle.none, GUILayout.ExpandWidth(true), GUILayout.Height(20)))
                {
                    config.ChangeSelect(this);
                }

                // 判断是否标脏，标脏则重新计算 CollectedFiles
                if (_dirty)
                {
                    _dirty = false;
                    CollectFiles();
                    // 保存配置文件
                    EditorUtility.SetDirty(config);
                    AssetDatabase.SaveAssetIfDirty(config);
                    AssetDatabase.Refresh();
                }

                EditorGUILayout.BeginHorizontal();
                {
                    // 显示当前路径
                    EditorGUILayout.LabelField("文件夹路径", GUILayout.Width(150));

                    // 添加选择按钮
                    if (GUILayout.Button(folderPath, GUILayout.ExpandWidth(true)))
                    {
                        // 打开系统文件夹选择对话框
                        string selectedPath = EditorUtility.OpenFolderPanel("选择目标路径",
                            Application.dataPath, "");
                        if (!string.IsNullOrEmpty(selectedPath) && selectedPath.StartsWith(Application.dataPath))
                        {
                            // 将绝对路径转换为相对路径（相对于Assets目录）
                            folderPath = "Assets" + selectedPath.Replace(Application.dataPath, "");
                            _dirty = true;
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();


                // 绘制输入框输入忽略正则
                var newIgnorePattern = EditorGUILayout.TextField("忽略文件路径正则", ignorePattern);
                if (newIgnorePattern != ignorePattern)
                {
                    ignorePattern = newIgnorePattern;
                    _dirty = true;
                }

                // 绘制输入框输入文件类型
                var newFileExtensions = EditorGUILayout.TextField("要打包的文件后缀,以;隔开多个后缀", fileExtensions);
                if (newFileExtensions != fileExtensions)
                {
                    fileExtensions = newFileExtensions;
                    _dirty = true;
                }

                // 绘制单选下拉列表选择文件夹构建类型
                var newBuildType = (ECollectorInfoType)EditorGUILayout.EnumPopup("ab包类型", infoType);
                if (newBuildType != infoType)
                {
                    infoType = newBuildType;
                    _dirty = true;
                }

                // 绘制折叠列表展示所有收集到的文件路径
                _isExpanded = EditorGUILayout.Foldout(_isExpanded, "已收集文件");
                if (_isExpanded && collectedFiles != null)
                {
                    EditorGUI.indentLevel++;
                    foreach (var file in collectedFiles)
                    {
                        EditorGUILayout.LabelField(file);
                    }

                    EditorGUI.indentLevel--;
                }
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 根据当前配置,收集所有文件路径
        /// </summary>
        private void CollectFiles()
        {
            if (string.IsNullOrEmpty(folderPath) || string.IsNullOrEmpty(fileExtensions))
            {
                collectedFiles = new List<string>(0);
                return;
            }
            // 获取文件夹下所有文件路径
            var extensionArray = fileExtensions.Split(';');
            var extensionHash = new HashSet<string>();
            extensionArray.ForEach(x =>
            {
                // 判断以'.'字符开头且只有一个'.'字符
                if (x.StartsWith(".") && x.LastIndexOf(".", StringComparison.Ordinal) == 0)
                {
                    extensionHash.Add(x);
                }
                // 判断没有'.'字符
                else if (!x.Contains("."))
                {
                    extensionHash.Add("." + x);
                }
            });
            List<string> files = AssetDatabase.FindAssets(string.Empty, new[] { folderPath })
                .SelectList(x => AssetDatabase.GUIDToAssetPath(x))
                .WhereList(x =>
                {
                    // 获取x后缀
                    var ext = System.IO.Path.GetExtension(x);
                    // 判断是否包含后缀
                    return extensionHash.Contains(ext);
                });
            

            try
            {
                // 正在匹配,移除忽略的正则
                if (!string.IsNullOrEmpty(ignorePattern))
                {
                    files = files.WhereList(x => !Regex.IsMatch(x, ignorePattern));
                }
            }
            catch (Exception)
            {
                //Log.LogError(sb => sb.AppendLine("正则表达式错误"));
            }
            
            collectedFiles = files;
        }
    }
}