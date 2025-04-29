/*------------------------------
 * 脚本名称: AssetBundleData
 * 创建者: movin
 * 创建日期: 2025/04/30
------------------------------*/

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
        private readonly List<AssetBundleData> _dependents;
        
        /// <summary>
        /// 加载中加载状态
        /// </summary>
        private ELoadStatus LoadingStatus { get; set; }
        
        /// <summary>
        /// 最终加载结果的加载状态
        /// </summary>
        private ELoadStatus LoadedStatus {  get; set; }
        
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
        private readonly Dictionary<string, AssetFileData> _assetFiles;
        
        public AssetBundleData(string md5, string name,  List<AssetBundleData> dependencies)
        {
            MD5 = md5;
            Name = name;
            _dependencies = dependencies;
            _dependents = new List<AssetBundleData>();
            if (null == _dependencies)
            {
                _dependencies = new List<AssetBundleData>(0);
            }
            // 添加所有被依赖的包
            foreach (var dependency in _dependencies)
            {
                dependency._dependents.Add(this);
            }
            LoadingStatus = ELoadStatus.NotStart;
            LoadedStatus = ELoadStatus.NotStart;
            AssetBundle = null;
            _assetFiles = new Dictionary<string, AssetFileData>();
            _releaseHandler = new DelayInvokeHandler();
        }

        public AssetBundleData(List<AssetBundleData> dependents)
        {
            _dependents = dependents;
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
                _assetFiles.Add(assetPath, new AssetFileData(this, assetPath));
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
            if (LoadedStatus != ELoadStatus.NotStart)
            {
                return;
            }
            LoadedStatus = ELoadStatus.Loading;
            
            if (LoadingStatus == ELoadStatus.NotStart)
            {
                LoadingStatus = ELoadStatus.Loading;
                if (resetRetryTimer)
                {
                    _loadRetryTimer = 0;
                }
                // TODO 开始加载,走网络加载或本地文件加载
            }
            
            await UniTask.WaitUntil(() => LoadingStatus == ELoadStatus.Loaded || LoadingStatus == ELoadStatus.Faulted);
            if (LoadingStatus == ELoadStatus.Loaded)
            {
                LoadingStatus = ELoadStatus.NotStart;
                LoadedStatus = ELoadStatus.Loaded;
                return;
            }
            // 加载失败,是否走重新加载的逻辑
            Log.LogError(sb =>
            {
                sb.Append("AB包加载失败,包名:");
                sb.Append(Name);
            }, ELogType.Resource);
            _loadRetryTimer++;
            LoadingStatus = ELoadStatus.NotStart;
            if (_loadRetryTimer > GameEntry.GlobalGameConfig.resourceConfig.abLoadRetryCount)
            {
                LoadedStatus = ELoadStatus.Faulted;
            }
            else
            {
                await LoadBundle(false);
            }
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
            }
            waitTasks[_dependencies.Count] = LoadBundle();
            await UniTask.WhenAll(waitTasks);
            
            // 确保当前的最终加载状态都是完成(依赖包加载失败,仍然加载资源)
            if (LoadedStatus != ELoadStatus.Loaded)
            {
                TryReleaseAsset();
                return default(T);
            }
            
            var asset = await assetFile.LoadAsset<T>();
            if (null == asset)
            {
                TryReleaseAsset();
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
            assetFile.UnloadAsset(assetPath);
        }

        /// <summary>
        /// 尝试释放资源
        /// </summary>
        public void TryReleaseAsset()
        {
            // 检查所有资源的引用情况
            foreach (var assetFile in _assetFiles.Values)
            {
                if (assetFile.LoadStatus != ELoadStatus.NotStart
                    && assetFile.LoadStatus != ELoadStatus.Loaded)
                {
                    return;
                }
            }
            // 检查所有被依赖包都释放完成
            foreach (var dependent in _dependents)
            {
                if (dependent.LoadedStatus != ELoadStatus.NotStart
                    && dependent.LoadedStatus != ELoadStatus.Loaded)
                {
                    return;
                }
            }
            
            _releaseHandler.DelayInvoke(() =>
            {
                LoadedStatus = ELoadStatus.NotStart;
                LoadingStatus = ELoadStatus.NotStart;
                AssetBundle = null;
                // 重置所有资源
                foreach (var assetFile in _assetFiles.Values)
                {
                    assetFile.Reset();
                }
                // 所有依赖包尝试释放
                foreach (var dependency in _dependencies)
                {
                    dependency.TryReleaseAsset();
                }
                
            }, GameEntry.GlobalGameConfig.resourceConfig.abReleaseDelayTime);
        }
    }
}