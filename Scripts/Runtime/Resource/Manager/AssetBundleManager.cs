/*------------------------------
 * 脚本名称: AssetBundleManager
 * 创建者: movin
 * 创建日期: 2025/04/30
------------------------------*/

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// AB包管理器
    /// </summary>
    public class AssetBundleManager : IResourceManager
    {
        /// <summary>
        /// 所有AB包数据
        /// key-资源路径,value-资源所属AB包数据
        /// </summary>
        private readonly Dictionary<string, AssetBundleData> _allAssetBundleDatas = new Dictionary<string, AssetBundleData>();

        /// <inheritdoc/>
        public UniTask Init()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 实际加载AB包的方法
        /// </summary>
        /// <param name="assetBundleData"></param>
        /// <returns></returns>
        protected virtual UniTask<AssetBundle> LoadAssetBundle(AssetBundleData assetBundleData)
        {
            return UniTask.FromResult(AssetBundle.LoadFromFile(assetBundleData.Name));
        }

        /// <inheritdoc/>
        public async UniTask<T> LoadAsset<T>(string assetPath) where T : Object
        {
            if (_allAssetBundleDatas.TryGetValue(assetPath, out var assetBundleData))
            {
                return await assetBundleData.LoadAsset<T>(assetPath);
            }

            return default(T);
        }

        /// <inheritdoc/>
        public void UnloadAsset(string assetPath)
        {
            if (_allAssetBundleDatas.TryGetValue(assetPath, out var assetBundleData))
            {
                assetBundleData.UnloadAsset(assetPath);
            }
        }

        /// <inheritdoc/>
        public void UnloadAllAsset()
        {
            foreach (var assetBundleData in _allAssetBundleDatas.Values)
            {
                assetBundleData.TryReleaseAsset();
            }
        }

        /// <inheritdoc/>
        public void Release()
        {
            UnloadAllAsset();
            _allAssetBundleDatas.Clear();
        }
    }
}