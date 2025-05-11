/*------------------------------
 * 脚本名称: GameObjectTransformPool
 * 创建者: movin
 * 创建日期: 2025/03/29
------------------------------*/

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// Transform游戏对象缓存池
    /// </summary>
    public class GameObjectTransformPool : MonoBehaviour, IGameObjectPool
    {
        /// <summary>
        /// 闲置引用列表
        /// </summary>
        private readonly List<GameObjectPoolItem> _freeItemList = new List<GameObjectPoolItem>();

        /// <summary>
        /// 游戏对象模板模板
        /// </summary>
        private GameObject _gameObjectPrefab;
        
        /// <summary>
        /// 从池中取出物体时默认父节点
        /// </summary>
        private Transform _spawnParent;
        
        /// <summary>
        /// 放回池中时默认父节点
        /// </summary>
        private Transform _despawnParent;
        
        /// <summary>
        /// 从池中取出引用时调用
        /// </summary>
        private Func<GameObject, UniTask> _onSpawnFromPool;
        
        /// <summary>
        /// 将引用返回到池中时调用
        /// </summary>
        private Func<GameObject, UniTask> _onDespawnToPool;
        
        /// <summary>
        /// 已经取用的引用hash表
        /// </summary>
        private HashSet<GameObjectPoolItem> _spawnedItemHash = new HashSet<GameObjectPoolItem>();

        /// <inheritdoc/>
        public int PoolId { get; set; }
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="gameObjectPrefab">游戏对象模板</param>
        /// <param name="spawnParent">从池中取出物体时默认父节点</param>
        /// <param name="despawnParent">将引用返回到池中时默认父节点</param>
        /// <param name="preCreateNum">提前创建数量</param>
        /// <param name="onSpawnFromPool">从池中取出引用时调用</param>
        /// <param name="onDespawnToPool">将引用返回到池中时调用</param>
        public void Init(
            GameObject gameObjectPrefab, 
            Transform spawnParent = null,
            Transform despawnParent = null,
            int preCreateNum = 0,
            Func<GameObject, UniTask> onSpawnFromPool = null,
            Func<GameObject, UniTask> onDespawnToPool = null)
        {
            if (null == gameObjectPrefab)
            {
                Log.LogError(sb =>
                {
                    sb.Append("ReferencePool: Init gameObjectPrefab is null");
                });
                return;
            }
            
            _freeItemList.Clear();
            _spawnedItemHash.Clear();
            
            _gameObjectPrefab = gameObjectPrefab;
            _spawnParent = null == spawnParent ? transform : spawnParent;
            _despawnParent = null == despawnParent ? transform : despawnParent;
            _gameObjectPrefab.name = "OriginalGameObject";
            _gameObjectPrefab.transform.SetParent(_spawnParent, false);
            _gameObjectPrefab.SetActive(false);
            _gameObjectPrefab.transform.localPosition = Vector3.zero;
            _gameObjectPrefab.transform.localRotation = Quaternion.identity;
            _gameObjectPrefab.transform.localScale = Vector3.one;
            _onSpawnFromPool = onSpawnFromPool;
            _onDespawnToPool = onDespawnToPool;
            
            int curCount = _freeItemList.Count;
            for (int i = curCount; i < preCreateNum; i++)
            {
                var item = GameObject.Instantiate(gameObjectPrefab, _despawnParent, false);
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
                item.transform.localScale = Vector3.one;
                item.SetActive(false);
                item.name = "PooledItem";
                var poolItem = item.GetOrAddComponent<GameObjectPoolItem>();
                poolItem.Pool = this;
                _freeItemList.Add(poolItem);
            }
        }
        
        /// <summary>
        /// 从引用池中获取一个引用
        /// </summary>
        /// <param name="parent">指定父级</param>
        /// <param name="localPosition">指定本地坐标</param>
        /// <param name="localRotation">指定旋转</param>
        /// <param name="localScale">指定缩放</param>
        /// <returns></returns>
        public async UniTask<GameObject> Spawn(
            Transform parent = null,
            Vector3? localPosition = null,
            Quaternion? localRotation = null,
            Vector3? localScale = null)
        {
            // 校验引用池模板
            if (null == _gameObjectPrefab)
            {
                Log.LogError(sb =>
                {
                    sb.Append("GameObjectTransformPool: Spawn _gameObjectPrefab is null");
                });
                return null;
            }
            
            if (null == parent)
            {
                parent = _spawnParent;
            }
            
            GameObjectPoolItem item = null;
            // 检验闲置引用列表是否为空
            if (_freeItemList.Count > 0)
            {
                item = _freeItemList[_freeItemList.Count - 1];
                _freeItemList.RemoveAt(_freeItemList.Count - 1);
                _spawnedItemHash.Add(item);
                item.transform.SetParent(parent, false);
            }
            else
            {
                // 创建一个新引用
                var obj = GameObject.Instantiate(_gameObjectPrefab, parent, false);
                obj.name = "PooledItem";
                item = obj.GetOrAddComponent<GameObjectPoolItem>();
                item.Pool = this;
                _spawnedItemHash.Add(item);
            }
            
            item.transform.localPosition = localPosition ?? Vector3.zero;
            item.transform.localRotation = localRotation ?? Quaternion.identity;
            item.transform.localScale = localScale ?? Vector3.one;
            item.gameObject.SetActive(true);
            if (null != _onSpawnFromPool)
            {
                await _onSpawnFromPool(item.gameObject);
            }
            return item.gameObject;
        }
        
        /// <inheritdoc/>
        public async UniTask Despawn(GameObject gameObject, bool isDestroy = false)
        {
            if (null == gameObject)
            {
                Log.LogError(sb =>
                {
                    sb.Append("GameObjectTransformPool: Despawn item is null");
                });
                return;
            }
            var item = gameObject.GetComponent<GameObjectPoolItem>();
            if (null == item)
            {
                Log.LogError(sb =>
                {
                    sb.Append("GameObjectTransformPool: Despawn item not found");
                });
                return;
            }
            
            // 从正在使用的引用列表中移除
            if (_spawnedItemHash.Remove(item))
            {
                if (isDestroy)
                {
                    return;
                }
                _freeItemList.Add(item);
                item.transform.SetParent(_despawnParent, false);
                item.gameObject.SetActive(false);
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
                item.transform.localScale = Vector3.one;
                if (null != _onDespawnToPool)
                {
                    await _onDespawnToPool(item.gameObject);
                }
            }
            // 对于已经被删除的物体, 从空闲池中删除
            else if (isDestroy)
            {
                _freeItemList.Remove(item);
            }
        }

        /// <inheritdoc/>
        public async UniTask DespawnAll()
        {
            var newHash = new HashSet<GameObjectPoolItem>(_spawnedItemHash.Count);
            (newHash, _spawnedItemHash) = (_spawnedItemHash, newHash);
            foreach (var item in newHash)
            {
                _freeItemList.Add(item);
                item.transform.SetParent(_despawnParent, false);
                item.gameObject.SetActive(false);
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
                item.transform.localScale = Vector3.one;
                item.Pool = null;
                if (null != _onDespawnToPool)
                {
                    await _onDespawnToPool(item.gameObject);
                }
            }
            newHash.Clear();
        }

        /// <summary>
        /// 释放所有引用
        /// </summary>
        private void OnDestroy()
        {
            DespawnAll().Forget();
            
            // 确保意外销毁时从缓存池模块中移除引用
            GameEntry.Pool.RemovePool(this.PoolId).Forget();
        }
    }
}