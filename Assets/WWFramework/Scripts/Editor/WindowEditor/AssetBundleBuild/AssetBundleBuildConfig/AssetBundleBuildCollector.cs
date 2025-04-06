/*------------------------------
 * 脚本名称: AssetBundleBuildCollector
 * 创建者: movin
 * 创建日期: 2025/04/06
------------------------------*/

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

namespace WWFramework
{
    /// <summary>
    /// AB包构建收集器
    /// </summary>
    [Serializable]
    public class AssetBundleBuildCollector
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
        public ECollectorBuildType buildType = ECollectorBuildType.EachFile;

        /// <summary>
        /// 文件类型
        /// </summary>
        public EFileType fileType = EFileType.Prefab;

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
        public void OnDrawGUI(AssetBundleBuildConfig config, bool isSelected)
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
                    AssetDatabase.SaveAssets();
                }

                EditorGUILayout.BeginHorizontal();
                {
                    // 显示当前路径
                    EditorGUILayout.LabelField("Folder Path", GUILayout.Width(150));

                    // 添加选择按钮
                    if (GUILayout.Button(folderPath, GUILayout.ExpandWidth(true)))
                    {
                        // 打开系统文件夹选择对话框
                        string selectedPath = EditorUtility.OpenFolderPanel("Select Target Folder",
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
                var newIgnorePattern = EditorGUILayout.TextField("Ignore Pattern", ignorePattern);
                if (newIgnorePattern != ignorePattern)
                {
                    ignorePattern = newIgnorePattern;
                    _dirty = true;
                }

                // 绘制多选下拉列表选择文件类型
                var newFileType = (EFileType)EditorGUILayout.EnumFlagsField("File Type", fileType);
                if (newFileType != fileType)
                {
                    fileType = newFileType;
                    _dirty = true;
                }

                // 绘制单选下拉列表选择文件夹构建类型
                var newBuildType = (ECollectorBuildType)EditorGUILayout.EnumPopup("Build Type", buildType);
                if (newBuildType != buildType)
                {
                    buildType = newBuildType;
                    _dirty = true;
                }

                // 绘制折叠列表展示所有收集到的文件路径
                _isExpanded = EditorGUILayout.Foldout(_isExpanded, "Collected Files");
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
            if (string.IsNullOrEmpty(folderPath))
            {
                collectedFiles = new List<string>(0);
                return;
            }
            List<string> files = new List<string>();
            if ((fileType & EFileType.Prefab) != 0)
            {
                files.AddRange(AssetDatabaseEx.GetAllAssetPath<GameObject>(folderPath));
            }
            if ((fileType & EFileType.Texture) != 0)
            {
                files.AddRange(AssetDatabaseEx.GetAllAssetPath<Texture2D>(folderPath));
            }
            if ((fileType & EFileType.Audio) != 0)
            {
                files.AddRange(AssetDatabaseEx.GetAllAssetPath<AudioClip>(folderPath));
            }
            if ((fileType & EFileType.Font) != 0)
            {
                files.AddRange(AssetDatabaseEx.GetAllAssetPath<Font>(folderPath));
            }
            if ((fileType & EFileType.Material) != 0)
            {
                files.AddRange(AssetDatabaseEx.GetAllAssetPath<Material>(folderPath));
            }
            if ((fileType & EFileType.Model) != 0)
            {
                files.AddRange(AssetDatabaseEx.GetAllAssetPath<Mesh>(folderPath));
            }
            if ((fileType & EFileType.Text) != 0)
            {
                files.AddRange(AssetDatabaseEx.GetAllAssetPath<TextAsset>(folderPath).WhereList(x => x.EndsWith(".txt")));
            }
            if ((fileType & EFileType.Byte) != 0)
            {
                files.AddRange(AssetDatabaseEx.GetAllAssetPath<TextAsset>(folderPath).WhereList(x => x.EndsWith(".bytes")));
            }
            if ((fileType & EFileType.ScriptableObject) != 0)
            {
                files.AddRange(AssetDatabaseEx.GetAllAssetPath<ScriptableObject>(folderPath));
            }
            if ((fileType & EFileType.Shader) != 0)
            {
                files.AddRange(AssetDatabaseEx.GetAllAssetPath<Shader>(folderPath));
            }
            if ((fileType & EFileType.Animation) != 0)
            {
                files.AddRange(AssetDatabaseEx.GetAllAssetPath<AnimationClip>(folderPath));
            }
            if ((fileType & EFileType.Video) != 0)
            {
                files.AddRange(AssetDatabaseEx.GetAllAssetPath<VideoClip>(folderPath));
            }
            
            // 正在匹配,移除忽略的正则
            collectedFiles = string.IsNullOrEmpty(ignorePattern) ? files : files.WhereList(x => !Regex.IsMatch(x, ignorePattern));
        }
    }
}