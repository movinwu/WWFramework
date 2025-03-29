/*------------------------------
 * 脚本名称: PoolModule
 * 创建者: movin
 * 创建日期: 2025/03/29
------------------------------*/

using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

namespace WWFramework
{
    /// <summary>
    /// 缓存池模块
    /// </summary>
    public class PoolModule : MonoBehaviour, IGameModule
    {
        private Dictionary<int, IPool> _allPoolDic = new Dictionary<int, IPool>();

        public UniTask OnInit()
        {
            return UniTask.CompletedTask;
        }

        public void OnRelease()
        {
            foreach (var pool in _allPoolDic.Values)
            {
                pool.DespawnAll().Forget();
            }
            _allPoolDic.Clear();
        }

        private void OnDestroy()
        {
            foreach (var pool in _allPoolDic.Values)
            {
                pool.DespawnAll().Forget();
            }
            _allPoolDic.Clear();
        }

        /// <summary>
        /// 创建缓存池
        /// <para> 部分缓存池创建完成后使用前需要进行初始化 </para>
        /// </summary>
        /// <typeparam name="T">缓存池类型</typeparam>
        /// <returns>创建的缓存池</returns>
        public T CreatePool<T>(int poolId) where T : class, IPool, new()
        {
            if (!_allPoolDic.TryGetValue(poolId, out var pool))
            {
                pool = new T();
                _allPoolDic.Add(poolId, pool);
            }

            return pool as T;
        }

        /// <summary>
        /// 异步删除缓存池
        /// </summary>
        /// <param name="poolId">缓存池ID</param>
        public async UniTask RemovePoolAsync(int poolId)
        {
            if (_allPoolDic.TryGetValue(poolId, out var pool))
            {
                await pool.DespawnAll();
                _allPoolDic.Remove(poolId);
            }
        }

        /// <summary>
        /// 获取指定类型的缓存池
        /// </summary>
        /// <typeparam name="T">缓存池类型</typeparam>
        /// <param name="poolId">缓存池ID</param>
        /// <returns>缓存池实例</returns>
        public T GetPool<T>(int poolId) where T : class, IPool, new()
        {
            if (_allPoolDic.TryGetValue(poolId, out var pool))
            {
                return pool as T;
            }
            return default;
        }
    }
}