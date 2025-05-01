/*------------------------------
 * 脚本名称: IResourceManager
 * 创建者: movin
 * 创建日期: 2025/05/01
------------------------------*/

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 资源管理器接口
    /// </summary>
    public interface IResourceManager
    {
        /// <summary>
        /// 初始化
        /// </summary>
        UniTask Init();
        
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetPath">资源地址</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns></returns>
        UniTask<T> LoadAsset<T>(string assetPath) where T : Object;
        
        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        void UnloadAsset(string assetPath);

        /// <summary>
        /// 卸载所有资源
        /// </summary>
        void UnloadAllAsset();

        /// <summary>
        /// 释放
        /// </summary>
        void Release();
    }
}