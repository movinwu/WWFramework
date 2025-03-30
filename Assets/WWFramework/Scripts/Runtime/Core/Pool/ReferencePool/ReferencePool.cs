/*------------------------------
 * 脚本名称: ReferencePool
 * 创建者: movin
 * 创建日期: 2025/03/29
------------------------------*/

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 单个引用池
    /// </summary>
    public class ReferencePool : MonoBehaviour, IPool
    {
        /// <summary>
        /// 闲置引用字典
        /// </summary>
        private readonly Dictionary<int, List<IReferencePoolItem>> _freeItemDic
            = new Dictionary<int, List<IReferencePoolItem>>();
        
        /// <summary>
        /// 已经取用的引用hash表
        /// </summary>
        private readonly HashSet<IReferencePoolItem> _spawnedItemHash = new HashSet<IReferencePoolItem>();

        /// <summary>
        /// 创建引用的委托
        /// </summary>
        private System.Func<int, IReferencePoolItem> _createItemFunc;

        /// <inheritdoc/>
        public int PoolId { get; set; }
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="createItemFunc">创建引用的委托,参数为引用id</param>
        public async UniTask Init(System.Func<int, IReferencePoolItem> createItemFunc)
        {
            this._createItemFunc = createItemFunc;
            
            await DespawnAll();
            _spawnedItemHash.Clear();
            _freeItemDic.Clear();
        }
        
        /// <summary>
        /// 从引用池中获取一个引用
        /// </summary>
        /// <param name="itemTypeId">引用类型id</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async UniTask<T> Spawn<T>(int itemTypeId) where T : class, IReferencePoolItem
        {
            // 检验创建引用委托是否为空
            if (null == _createItemFunc)
            {
                Log.LogError(sb =>
                {
                    sb.AppendLine("创建引用的委托为空,请先调用Init方法进行初始化");
                });
                return default;
            }
            
            // 检验闲置引用列表是否为空
            if (!_freeItemDic.TryGetValue(itemTypeId, out var freeItemList))
            {
                freeItemList = new List<IReferencePoolItem>();
                _freeItemDic.Add(itemTypeId, freeItemList);
            }
            if (freeItemList.Count > 0)
            {
                var item = freeItemList[^1];
                freeItemList.RemoveAt(freeItemList.Count - 1);
                item.PoolItemTypeId = itemTypeId;
                _spawnedItemHash.Add(item);
                await item.OnSpawnFromPool();
                return item as T;
            }
            
            // 创建一个新引用
            var newItem = _createItemFunc(itemTypeId);
            _spawnedItemHash.Add(newItem);
            newItem.PoolItemTypeId = itemTypeId;
            await newItem.OnSpawnFromPool();
            return newItem as T;
        }
        
        /// <summary>
        /// 将引用返回到引用池中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public async UniTask Despawn<T>(T item) where T : class, IReferencePoolItem
        {
            // 从正在使用的引用列表中移除
            if (_spawnedItemHash.Remove(item))
            {
                if (!_freeItemDic.TryGetValue(item.PoolItemTypeId, out var freeItemList))
                {
                    freeItemList = new List<IReferencePoolItem>();
                    _freeItemDic.Add(item.PoolItemTypeId, freeItemList);
                }
                freeItemList.Add(item);
                await item.OnDespawnToPool();
            }
        }

        /// <inheritdoc/>
        public async UniTask DespawnAll()
        {
            foreach (var item in _spawnedItemHash)
            {
                if (!_freeItemDic.TryGetValue(item.PoolItemTypeId, out var freeItemList))
                {
                    freeItemList = new List<IReferencePoolItem>();
                    _freeItemDic.Add(item.PoolItemTypeId, freeItemList);
                }
                freeItemList.Add(item);
                await item.OnDespawnToPool();
            }
            _spawnedItemHash.Clear();
        }

        /// <summary>
        /// 释放所有引用
        /// </summary>
        private void OnDestroy()
        {
            foreach (var item in _spawnedItemHash)
            {
                item.OnDespawnToPool().Forget();
            }
            _spawnedItemHash.Clear();
            foreach (var item in _freeItemDic)
            {
                item.Value.Clear();
            }
            
            // 确保意外销毁时从缓存池模块中移除引用
            GameEntry.Pool.RemovePool(this.PoolId).Forget();
        }
    }
}