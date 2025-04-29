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
    public class AssetBundleManager
    {
        /// <summary>
        /// 所有AB包数据
        /// key-资源路径,value-资源所属AB包数据
        /// </summary>
        private Dictionary<string, AssetBundleData> _allAssetBundle = new Dictionary<string, AssetBundleData>();
        
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetPath"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async UniTask<T> LoadAsset<T>(string assetPath) where T : UnityEngine.Object
        {
            if (_allAssetBundle.TryGetValue(assetPath, out var assetBundleData))
            {
                return await assetBundleData.LoadAsset<T>(assetPath);
            }

            return default(T);
        }
    }
}