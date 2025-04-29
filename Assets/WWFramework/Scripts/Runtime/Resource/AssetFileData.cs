/*------------------------------
 * 脚本名称: AssetFileData
 * 创建者: movin
 * 创建日期: 2025/04/30
------------------------------*/

using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WWFramework
{
    /// <summary>
    /// 单个资源文件数据
    /// </summary>
    public class AssetFileData
    {
        /// <summary>
        /// AB包数据
        /// </summary>
        private readonly AssetBundleData _assetBundleData;

        /// <summary>
        /// 资源路径
        /// </summary>
        private readonly string _assetPath;
        
        /// <summary>
        /// 资源加载状态
        /// </summary>
        public ELoadStatus LoadStatus { get; private set; }

        /// <summary>
        /// 资源引用计数
        /// </summary>
        private int _referenceCount;
        
        /// <summary>
        /// 资源对象
        /// </summary>
        private Object _asset;
        
        /// <summary>
        /// 释放延迟调用
        /// </summary>
        private readonly DelayInvokeHandler _releaseHandler;
        
        public AssetFileData(AssetBundleData assetBundleData, string assetPath)
        {
            _assetBundleData = assetBundleData;
            _assetPath = assetPath;
            _releaseHandler = new DelayInvokeHandler();
            Reset();
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            _asset = null;
            LoadStatus = ELoadStatus.NotStart;
            _referenceCount = 0;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async UniTask<T> LoadAsset<T>() where T : Object
        {
            if (LoadStatus == ELoadStatus.NotStart)
            {
                LoadStatus = ELoadStatus.Loading;
                try
                {
                    _asset = await _assetBundleData.AssetBundle.LoadAssetAsync<T>(_assetPath);
                    LoadStatus = ELoadStatus.Loaded;
                }
                catch (Exception e)
                {
                    Log.LogError(sb =>
                    {
                        sb.AppendLine("加载资源失败");
                        sb.AppendLine(e.ToString());
                    });
                    LoadStatus = ELoadStatus.Faulted;
                }
            }

            await UniTask.WaitUntil(() => LoadStatus == ELoadStatus.Loaded || LoadStatus == ELoadStatus.Faulted);
            if (LoadStatus == ELoadStatus.Loaded)
            {
                _referenceCount++;
                // 成功加载资源,取消释放延迟调用
                return _asset as T;
            }
            return default(T);
        }
        
        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="assetPath"></param>
        public void UnloadAsset(string assetPath)
        {
            if (_assetPath == assetPath)
            {
                _referenceCount--;
                if (_referenceCount < 0)
                {
                    Log.LogError(sb =>
                    {
                        sb.Append("资源引用计数小于0,资源路径:");
                        sb.AppendLine(_assetPath);
                    },  ELogType.Resource);
                    _referenceCount = 0;
                }
                else if (_referenceCount == 0)
                {
                    _releaseHandler.DelayInvoke(() =>
                    {
                        _asset = null;
                        LoadStatus = ELoadStatus.NotStart;
                        _assetBundleData?.TryReleaseAsset();
                    }, GameEntry.GlobalGameConfig.resourceConfig.assetReleaseDelayTime);
                }
            }
        }
    }
}