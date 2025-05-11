/*------------------------------
 * 脚本名称: AssetFileData
 * 创建者: movin
 * 创建日期: 2025/05/02
------------------------------*/

using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 单个资源文件数据
    /// </summary>
    public class AssetFileData
    {
        /// <summary>
        /// 资源路径
        /// </summary>
        protected readonly string AssetPath;

        /// <summary>
        /// 资源加载状态
        /// </summary>
        protected bool IsLoading;

        /// <summary>
        /// 资源引用计数
        /// </summary>
        protected int ReferenceCount;
        
        /// <summary>
        /// 资源对象
        /// </summary>
        protected Object Asset;
        
        /// <summary>
        /// 释放延迟调用
        /// </summary>
        protected readonly DelayInvokeHandler ReleaseHandler;
        
        public AssetFileData(string assetPath)
        {
            AssetPath = assetPath;
            ReleaseHandler = new DelayInvokeHandler();
            Reset();
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            Asset = null;
            IsLoading = false;
            ReferenceCount = 0;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async UniTask<T> LoadAsset<T>() where T : Object
        {
            // 加载资源时先取消可能的资源释放
            ReleaseHandler.Cancel();
            // 正在加载中,等待加载完毕
            if (IsLoading)
            {
                await UniTask.WaitUntil(() => !IsLoading);
            }
            // 资源为空,走加载流程
            if (null == Asset)
            {
                IsLoading = true;
                Asset = await RealLoadAsset<T>();
                IsLoading = false;
            }
            // 加载完成
            if (null == Asset)
            {
                Log.LogError(sb =>
                {
                    sb.Append("加载资源失败,资源地址:");
                    sb.Append(AssetPath);
                },  ELogType.Resource);
                TryReleaseAsset();
                return default(T);
            }
            ReferenceCount++;
            // 成功加载资源,取消释放延迟调用
            return Asset as T;
        }

        /// <summary>
        /// 实际加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected virtual UniTask<T> RealLoadAsset<T>() where T : Object
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(AssetPath);
            return UniTask.FromResult(asset);
        }
        
        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="assetPath"></param>
        public bool UnloadAsset(string assetPath)
        {
            if (AssetPath == assetPath)
            {
                ReferenceCount--;
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
            if (ReferenceCount < 0)
            {
                Log.LogError(sb =>
                {
                    sb.Append("资源引用计数小于0,资源路径:");
                    sb.AppendLine(AssetPath);
                },  ELogType.Resource);
                ReferenceCount = 0;
            }
            else if (ReferenceCount == 0)
            {
                ReleaseHandler.DelayInvoke(() =>
                {
                    Asset = null;
                }, GameEntry.GlobalGameConfig.resourceConfig.assetReleaseDelayTime);
            }
        }
    }
}