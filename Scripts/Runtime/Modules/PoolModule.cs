/*------------------------------
 * 脚本名称: PoolModule
 * 创建者: movin
 * 创建日期: 2025/03/29
------------------------------*/

using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace WWFramework
{
    /// <summary>
    /// 缓存池模块
    /// </summary>
    public class PoolModule : MonoBehaviour, IGameModule
    {
        private readonly Dictionary<int, IPool> _allPoolDic = new Dictionary<int, IPool>();

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
        /// </summary>
        /// <param name="poolId">缓存池ID</param>
        /// <param name="onInit">初始化回调</param>
        /// <typeparam name="T">缓存池类型</typeparam>
        /// <returns>创建的缓存池</returns>
        public T CreatePool<T>(int poolId, System.Action<T> onInit = null) where T : MonoBehaviour, IPool
        {
            if (!_allPoolDic.TryGetValue(poolId, out var pool))
            {
                var obj = new GameObject($"{typeof(T).Name}_{poolId}");
                obj.transform.SetParent(transform);
                pool = obj.AddComponent<T>();
                pool.PoolId = poolId;
                _allPoolDic.Add(poolId, pool);
            }

            var p = pool as T;
            onInit?.Invoke(p);
            return p;
        }

        /// <summary>
        /// 异步删除缓存池
        /// </summary>
        /// <param name="poolId">缓存池ID</param>
        internal async UniTask RemovePool(int poolId)
        {
            if (_allPoolDic.Remove(poolId, out var pool))
            {
                await pool.DespawnAll();
            }
        }
        
        /// <summary>
        /// 销毁缓存池
        /// </summary>
        /// <param name="pool"></param>
        /// <typeparam name="T"></typeparam>
        public async UniTask DestroyPool<T>(T pool) where T : MonoBehaviour, IPool
        {
            await DestroyPool(pool.PoolId);
        }
        
        /// <summary>
        /// 销毁缓存池
        /// </summary>
        /// <param name="poolId">缓存池ID</param>
        public async UniTask DestroyPool(int poolId)
        {
            if (_allPoolDic.Remove(poolId, out var pool) && pool is Object obj)
            {
                await pool.DespawnAll();
                obj.SafeDestroy();
            }
        }

        /// <summary>
        /// 获取指定类型的缓存池
        /// </summary>
        /// <typeparam name="T">缓存池类型</typeparam>
        /// <param name="poolId">缓存池ID</param>
        /// <returns>缓存池实例</returns>
        public T GetPool<T>(int poolId) where T : MonoBehaviour, IPool
        {
            if (_allPoolDic.TryGetValue(poolId, out var pool))
            {
                return pool as T;
            }
            return default;
        }
    }
}