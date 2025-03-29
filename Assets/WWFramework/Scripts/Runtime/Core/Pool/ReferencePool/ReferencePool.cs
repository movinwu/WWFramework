/*------------------------------
 * 脚本名称: ReferencePool
 * 创建者: movin
 * 创建日期: 2025/03/29
------------------------------*/

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 单个引用池
    /// </summary>
    public class ReferencePool : MonoBehaviour, IPool
    {
        /// <summary>
        /// 闲置引用列表
        /// </summary>
        private readonly List<IReferencePoolItem> _freeItemList = new List<IReferencePoolItem>();
        
        /// <summary>
        /// 引用池对象类型
        /// </summary>
        private System.Type _itemType;
        
        /// <summary>
        /// 已经取用的引用hash表
        /// </summary>
        private readonly HashSet<IReferencePoolItem> _spawnedItemHash = new HashSet<IReferencePoolItem>();
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="preCreateNum">提前创建数量</param>
        /// <typeparam name="T">缓存池中缓存的物体类型,必须实现公开无参构造函数的引用类型</typeparam>
        public void Init<T>(int preCreateNum = 0) where T : class, IReferencePoolItem, new()
        {
            _itemType = typeof(T);
            int curCount = _freeItemList.Count;
            for (int i = curCount; i < preCreateNum; i++)
            {
                var item = new T();
                _freeItemList.Add(item);
            }
        }
        
        /// <summary>
        /// 从引用池中获取一个引用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async UniTask<T> Spawn<T>() where T : class, IReferencePoolItem, new()
        {
            // 检验类型是否正确
            var type = typeof(T);
            if (type == _itemType || type.IsSubclassOf(_itemType))
            {
                Log.LogError(sb =>
                {
                    sb.Append("ReferencePool: Spawn<");
                    sb.Append(type.Name);
                    sb.Append("> type error");
                });
                return default(T);
            }
            
            // 检验闲置引用列表是否为空
            if (_freeItemList.Count > 0)
            {
                var item = _freeItemList[_freeItemList.Count - 1];
                _freeItemList.RemoveAt(_freeItemList.Count - 1);
                _spawnedItemHash.Add(item);
                await item.OnSpawnFromPool();
                return item as T;
            }
            
            // 创建一个新引用
            var newItem = new T();
            _spawnedItemHash.Add(newItem);
            await newItem.OnSpawnFromPool();
            return newItem;
        }
        
        /// <summary>
        /// 将引用返回到引用池中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public async UniTask Despawn<T>(T item) where T : class, IReferencePoolItem, new()
        {
            // 检验类型是否正确
            var type = typeof(T);
            if (type == _itemType || type.IsSubclassOf(_itemType))
            {
                Log.LogError(sb =>
                {
                    sb.Append("ReferencePool: Despawn<");
                    sb.Append(type.Name);
                    sb.Append("> type error");
                });
                return;
            }
            
            // 从正在使用的引用列表中移除
            if (_spawnedItemHash.Remove(item))
            {
                _freeItemList.Add(item);
                await item.OnDespawnToPool();
            }
            else
            {
                Log.LogError(sb =>
                {
                    sb.Append("ReferencePool: Despawn<");
                    sb.Append(type.Name);
                    sb.Append("> item not found");
                });
            }
        }

        /// <inheritdoc/>
        public async UniTask DespawnAll()
        {
            foreach (var item in _spawnedItemHash)
            {
                _freeItemList.Add(item);
                await item.OnDespawnToPool();
            }
            _spawnedItemHash.Clear();
        }

        /// <summary>
        /// 释放所有引用
        /// </summary>
        private void OnDestroy()
        {
            DespawnAll().Forget();
            _spawnedItemHash.Clear();
            _freeItemList.Clear();
        }
    }
}