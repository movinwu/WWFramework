/*------------------------------
 * 脚本名称: AssetDatabaseEx
 * 创建者: movin
 * 创建日期: 2025/04/05
------------------------------*/

using System;
using System.Collections.Generic;

namespace WWFramework
{
#if UNITY_EDITOR
    /// <summary>
    /// AssetDatabase扩展
    /// </summary>
    public static class AssetDatabaseEx
    {
        /// <summary>
        /// 获取指定文件夹下所有资源
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns></returns>
        public static List<T> GetAllAssets<T>(string folderPath) where T : UnityEngine.Object
        {
            // 验证文件夹路径有效性
            if (!UnityEditor.AssetDatabase.IsValidFolder(folderPath))
            {
                Log.LogError(sb =>
                {
                    sb.Append($"无效的文件夹路径: ");
                    sb.Append(folderPath);
                });
                return new List<T>(0);
            }

            // 构建搜索过滤器（根据类型）
            string filter = $"t:{typeof(T).Name}";

            // 搜索所有符合条件的GUID
            string[] guids = UnityEditor.AssetDatabase.FindAssets(filter, new[] { folderPath });

            return guids.SelectList(guid =>
            {
                // 通过GUID获取资源路径
                string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);

                // 加载资源并尝试转换类型
                T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);

                return asset;
            }, (asset, _) => asset != null);
        }
        
        /// <summary>
        /// 获取指定文件夹下所有资源
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns></returns>
        public static List<string> GetAllAssetPath<T>(string folderPath) where T : UnityEngine.Object
        {
            // 验证文件夹路径有效性
            if (!UnityEditor.AssetDatabase.IsValidFolder(folderPath))
            {
                Log.LogError(sb =>
                {
                    sb.Append($"无效的文件夹路径: ");
                    sb.Append(folderPath);
                });
                return new List<string>(0);
            }

            // 构建搜索过滤器（根据类型）
            string filter = $"t:{typeof(T).Name}";

            // 搜索所有符合条件的GUID
            string[] guids = UnityEditor.AssetDatabase.FindAssets(filter, new[] { folderPath });

            return guids.SelectList(UnityEditor.AssetDatabase.GUIDToAssetPath, (asset, _) => asset != null);
        }
    }
#endif
}