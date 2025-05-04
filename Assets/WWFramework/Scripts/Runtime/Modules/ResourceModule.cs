/*------------------------------
 * 脚本名称: ResourceModule
 * 创建者: movin
 * 创建日期: 2025/05/01
------------------------------*/

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 资源管理模块
    /// </summary>
    public class ResourceModule : MonoBehaviour, IGameModule
    {
        /// <summary>
        /// 资源管理器
        /// </summary>
        private IResourceManager _resourceManager;

        /// <inheritdoc />
        public UniTask OnInit()
        {
            _resourceManager = null;
            switch (GameEntry.GlobalGameConfig.resourceConfig.resourceMode)
            {
#if UNITY_EDITOR
                case EResourceMode.Development:
                    _resourceManager = new AssetDatabaseManager();
                    break;
#endif
                case EResourceMode.LocalAssetBundle:
                    _resourceManager = new LocalAssetBundleManager();
                    break;
                case EResourceMode.RemoteAssetBundle:
                    _resourceManager = new RemoteAssetBundleManager();
                    break;
            }

            if (null == _resourceManager)
            {
                Log.LogError(sb =>
                {
                    sb.Append("[ResourceModule] ResourceManager is null.");
                }, ELogType.Resource);
                return UniTask.CompletedTask;
            }
            return _resourceManager.Init();
        }
        
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetPath"></param>
        /// <typeparam name="T"></typeparam>
        public async UniTask<T> LoadAsset<T>(string assetPath) where T : Object
        {
            if (null == _resourceManager)
            {
                Log.LogError(sb =>
                {
                    sb.Append("[ResourceModule] ResourceManager is null.");
                }, ELogType.Resource);
                return null;
            }
            return await _resourceManager.LoadAsset<T>(assetPath);
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="folderPath">文件夹名,在热更工程中将提供一个专门的const类定义各种文件夹名,方便调用</param>
        /// <param name="assetName">资源名</param>
        /// <param name="extension">资源类型</param>
        public async UniTask<T> LoadAsset<T>(string folderPath, string assetName, EResourceExtension extension) where T : Object
        {
            if (string.IsNullOrEmpty(folderPath) || string.IsNullOrEmpty(assetName))
            {
                return default(T);
            }
            
            var assetPath = $"{folderPath}/{assetName}{ResourceHelper.GetExtension(extension)}";
            return await _resourceManager.LoadAsset<T>(assetPath);
        }
        
        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="assetPath"></param>
        public void UnloadAsset(string assetPath)
        {
            if (null == _resourceManager)
            {
                Log.LogError(sb =>
                {
                    sb.Append("[ResourceModule] ResourceManager is null.");
                }, ELogType.Resource);
                return;
            }
            _resourceManager.UnloadAsset(assetPath);
        }
        
        /// <summary>
        /// 卸载资源
        /// </summary>
        public void UnloadAsset(string folderPath, string assetName, EResourceExtension extension)
        {
            if (string.IsNullOrEmpty(folderPath) || string.IsNullOrEmpty(assetName))
            {
                return;
            }
            
            var assetPath = $"{folderPath}/{assetName}{ResourceHelper.GetExtension(extension)}";
            _resourceManager.UnloadAsset(assetPath);
        }
        
        /// <summary>
        /// 卸载所有资源
        /// </summary>
        public void UnloadAllAsset()
        {
            if (null == _resourceManager)
            {
                Log.LogError(sb =>
                {
                    sb.Append("[ResourceModule] ResourceManager is null.");
                }, ELogType.Resource);
                return;
            }
            _resourceManager.UnloadAllAsset();
        }

        /// <inheritdoc />
        public void OnRelease()
        {
            if (null == _resourceManager)
            {
                Log.LogError(sb =>
                {
                    sb.Append("[ResourceModule] ResourceManager is null.");
                }, ELogType.Resource);
                return;
            }
            _resourceManager.Release();
            _resourceManager = null;
        }
    }
}