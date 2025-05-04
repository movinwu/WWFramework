/*------------------------------
 * 脚本名称: AssetDatabaseManager
 * 创建者: movin
 * 创建日期: 2025/05/02
------------------------------*/

#if UNITY_EDITOR
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// assetDataBase管理器
    /// </summary>
    public class AssetDatabaseManager : IResourceManager
    {
        /// <summary>
        /// 已加载的资源数据
        /// </summary>
        private Dictionary<string, AssetFileData> _loadedAssetFileDatas = new Dictionary<string, AssetFileData>();
        
        public UniTask Init()
        {
            return UniTask.CompletedTask;
        }

        public UniTask<T> LoadAsset<T>(string assetPath) where T : Object
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                return UniTask.FromResult(default(T));
            }
            
            if (!_loadedAssetFileDatas.TryGetValue(assetPath, out var assetFileData))
            {
                assetFileData = new AssetFileData(assetPath);
                _loadedAssetFileDatas.Add(assetPath, assetFileData);
            }
            
            return assetFileData.LoadAsset<T>();
        }

        public void UnloadAsset(string assetPath)
        {
            if (_loadedAssetFileDatas.TryGetValue(assetPath, out var assetFileData))
            {
                assetFileData.UnloadAsset(assetPath);
            }
        }

        public void UnloadAllAsset()
        {
            foreach (var assetFileData in _loadedAssetFileDatas.Values)
            {
                assetFileData.Reset();
            }
            _loadedAssetFileDatas.Clear();
        }

        public void Release()
        {
            UnloadAllAsset();
        }
    }
}
#endif