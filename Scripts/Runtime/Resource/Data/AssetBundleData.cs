/*------------------------------
 * 脚本名称: AssetBundleData
 * 创建者: movin
 * 创建日期: 2025/04/30
------------------------------*/

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// ab包数据
    /// </summary>
    public class AssetBundleData
    {
        /// <summary>
        /// md5
        /// </summary>
        public string MD5 { get; private set; }
        
        /// <summary>
        /// ab包名
        /// </summary>
        public string Name { get; private set;}
        
        /// <summary>
        /// 依赖的ab包
        /// </summary>
        private readonly List<AssetBundleData> _dependencies;
        
        /// <summary>
        /// 被依赖的AB包数据
        /// </summary>
        /// <returns></returns>
        private readonly HashSet<AssetBundleData> _dependents;

        /// <summary>
        /// 是否正在加载中
        /// </summary>
        private bool _isLoading;
        
        /// <summary>
        /// ab包
        /// </summary>
        public AssetBundle AssetBundle { get; private set; }
        
        /// <summary>
        /// 释放延迟调用
        /// </summary>
        private readonly DelayInvokeHandler _releaseHandler;

        /// <summary>
        /// 重试加载次数
        /// </summary>
        private int _loadRetryTimer = 0;
        
        /// <summary>
        /// 所有资源
        /// </summary>
        private readonly Dictionary<string, AssetBundleFileData> _assetFiles;

        /// <summary>
        /// 已经加载完毕的所有资源数量
        /// </summary>
        private int _loadedAssetCount = 0;

        /// <summary>
        /// 加载AssetBundle资源委托
        /// </summary>
        private Func<AssetBundleData, UniTask<AssetBundle>> _loadAssetBundleFunc;
        
        public AssetBundleData(
            string md5,
            string name, 
            List<AssetBundleData> dependencies,
            Func<AssetBundleData, UniTask<AssetBundle>> loadAssetBundleFunc)
        {
            MD5 = md5;
            Name = name;
            _dependencies = dependencies;
            _loadAssetBundleFunc = loadAssetBundleFunc;
            _dependents = new HashSet<AssetBundleData>();
            if (null == _dependencies)
            {
                _dependencies = new List<AssetBundleData>(0);
            }
            _dependents = new HashSet<AssetBundleData>();
            _isLoading = false;
            AssetBundle = null;
            _assetFiles = new Dictionary<string, AssetBundleFileData>();
            _loadedAssetCount = 0;
            _releaseHandler = new DelayInvokeHandler();
        }

        /// <summary>
        /// 添加单个资源
        /// </summary>
        /// <param name="assetPath"></param>
        public void AddAsset(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                return;
            }
            
            if (!_assetFiles.ContainsKey(assetPath))
            {
                _assetFiles.Add(assetPath, new AssetBundleFileData(this, assetPath));
            }
        }

        /// <summary>
        /// 加载bundle
        /// </summary>
        /// <param name="resetRetryTimer">重置重新加载次数</param>
        public async UniTask LoadBundle(bool resetRetryTimer = true)
        {
            //加载包,取消释放延迟
            _releaseHandler.Cancel();
            
            // 已经发起加载的,不再重新发起加载
            if (_isLoading)
            {
                return;
            }

            _isLoading = true;
            
            if (resetRetryTimer)
            {
                _loadRetryTimer = 0;
            }
            
            // 异步加载AB包(网络加载或本地加载)
            if (null != _loadAssetBundleFunc)
            {
                AssetBundle = await _loadAssetBundleFunc(this);
            }
            
            if (null != AssetBundle)
            {
                _isLoading = false;
                return;
            }
            // 加载失败,是否走重新加载的逻辑
            Log.LogError(sb =>
            {
                sb.Append("AB包加载失败,包名:");
                sb.Append(Name);
            }, ELogType.Resource);
            _loadRetryTimer++;
            if (_loadRetryTimer > GameEntry.GlobalGameConfig.resourceConfig.abLoadRetryCount)
            {
                _isLoading = false;
                return;
            }
            else
            {
                await LoadBundle(false);
            }
            _isLoading = false;
        }
        
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetPath"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async UniTask<T> LoadAsset<T>(string assetPath) where T : UnityEngine.Object
        {
            if (!_assetFiles.TryGetValue(assetPath, out var assetFile))
            {
                return default(T);
            }

            // 确保依赖包和当前包都加载完成
            var waitTasks = new UniTask[_dependencies.Count + 1];
            for (int i = 0; i < _dependencies.Count; i++)
            {
                waitTasks[i] = _dependencies[i].LoadBundle();
                // 添加依赖关系
                _dependencies[i].AddDependent(this);
            }
            waitTasks[_dependencies.Count] = LoadBundle();
            await UniTask.WhenAll(waitTasks);
            
            // 确保当前的最终加载状态都是完成(依赖包加载失败,仍然加载资源)
            if (null == AssetBundle)
            {
                TryReleaseAsset();
                return default(T);
            }
            
            var asset = await assetFile.LoadAsset<T>();
            if (null == asset)
            {
                TryReleaseAsset();
            }
            else
            {
                _loadedAssetCount++;
            }
            return asset;
        }
        
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="assetPath"></param>
        public void UnloadAsset(string assetPath)
        {
            if (!_assetFiles.TryGetValue(assetPath, out var assetFile))
            {
                return;
            }

            if (assetFile.UnloadAsset(assetPath))
            {
                _loadedAssetCount--;
                TryReleaseAsset();
            }
        }

        /// <summary>
        /// 添加被依赖
        /// </summary>
        /// <param name="dependency"></param>
        private void AddDependent(AssetBundleData dependency)
        {
            _dependents.Add(dependency);
        }
        
        /// <summary>
        /// 移除被依赖
        /// </summary>
        /// <param name="dependency"></param>
        private void RemoveDependent(AssetBundleData dependency)
        {
            _dependents.Remove(dependency);
            TryReleaseAsset();
        }

        /// <summary>
        /// 尝试释放资源
        /// </summary>
        public void TryReleaseAsset()
        {
            // 检查所有资源的引用情况
            if (_loadedAssetCount < 0)
            {
                Log.LogError(sb =>
                {
                    sb.Append($"ab包加载出的资源引用计数异常,检查是否重复减少引用");
                }, ELogType.Resource);
                _loadedAssetCount = 0;
            }
            else if (_loadedAssetCount == 0)
            {
                if (_dependents.Count == 0)
                {
                    _releaseHandler.DelayInvoke(() =>
                    {
                        _isLoading = false;
                        AssetBundle.Unload(true);
                        AssetBundle = null;
                        // 重置所有资源
                        foreach (var assetFile in _assetFiles.Values)
                        {
                            assetFile.Reset();
                        }
                        // 所有依赖包尝试释放
                        foreach (var dependency in _dependencies)
                        {
                            dependency.RemoveDependent(this);
                        }
                
                    }, GameEntry.GlobalGameConfig.resourceConfig.abReleaseDelayTime);
                }
            }
        }
    }
}