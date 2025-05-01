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
        private bool _isLoading;

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
            _isLoading = false;
            _referenceCount = 0;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async UniTask<T> LoadAsset<T>() where T : Object
        {
            // 加载资源时先取消可能的资源释放
            _releaseHandler.Cancel();
            // 正在加载中,等待加载完毕
            if (_isLoading)
            {
                await UniTask.WaitUntil(() => !_isLoading);
            }
            // 资源为空,走加载流程
            if (null == _asset)
            {
                _isLoading = true;
                _asset = await _assetBundleData.AssetBundle.LoadAssetAsync<T>(_assetPath);
                _isLoading = false;
            }
            // 加载完成
            if (null == _asset)
            {
                Log.LogError(sb =>
                {
                    sb.Append("加载资源失败,资源地址:");
                    sb.Append(_assetPath);
                });
                TryReleaseAsset();
                return default(T);
            }
            _referenceCount++;
            // 成功加载资源,取消释放延迟调用
            return _asset as T;
        }
        
        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="assetPath"></param>
        public bool UnloadAsset(string assetPath)
        {
            if (_assetPath == assetPath)
            {
                _referenceCount--;
                TryReleaseAsset();
                return true;
            }

            return false;
        }

        /// <summary>
        /// 尝试释放资源
        /// </summary>
        private void TryReleaseAsset()
        {
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
                }, GameEntry.GlobalGameConfig.resourceConfig.assetReleaseDelayTime);
            }
        }
    }
}