/*------------------------------
 * 脚本名称: GameObjectPooll
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
    /// RectTransform游戏对象缓存池
    /// </summary>
    public class GameObjectRectTransformPool : MonoBehaviour, IPool
    {
        /// <summary>
        /// 闲置引用列表
        /// </summary>
        private readonly List<GameObject> _freeItemList = new List<GameObject>();

        /// <summary>
        /// 游戏对象模板模板
        /// </summary>
        private GameObject _gameObjectPrefab;
        
        /// <summary>
        /// 从池中取出物体时默认父节点
        /// </summary>
        private RectTransform _spawnParent;
        
        /// <summary>
        /// 放回池中时默认父节点
        /// </summary>
        private RectTransform _despawnParent;
        
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
        private readonly HashSet<GameObject> _spawnedItemHash = new HashSet<GameObject>();
        
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
            RectTransform spawnParent = null,
            RectTransform despawnParent = null,
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
            
            _gameObjectPrefab = gameObjectPrefab;
            RectTransform rectTrans = null;
            if (null == spawnParent || null == despawnParent)
            {
                rectTrans = this.GetComponent<RectTransform>();
                if (null == rectTrans)
                {
                    rectTrans = this.gameObject.GetComponent<RectTransform>();
                }
            }
            _spawnParent = null == spawnParent ? rectTrans : spawnParent;
            _despawnParent = null == despawnParent ? rectTrans : despawnParent;
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
                _freeItemList.Add(item);
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
            RectTransform parent = null,
            Vector3 localPosition = default,
            Quaternion localRotation = default,
            Vector3 localScale = default)
        {
            if (null == parent)
            {
                parent = _spawnParent;
            }
            
            GameObject item = null;
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
                item = GameObject.Instantiate(_gameObjectPrefab, parent, false);
                _spawnedItemHash.Add(item);
            }
            
            item.transform.localPosition = localPosition;
            item.transform.localRotation = localRotation;
            item.transform.localScale = localScale;
            if (null != _onSpawnFromPool)
            {
                await _onSpawnFromPool(item);
            }
            return item;
        }
        
        /// <summary>
        /// 将引用返回到引用池中
        /// </summary>
        /// <param name="item"></param>
        public async UniTask Despawn(GameObject item)
        {
            if (null == item)
            {
                Log.LogError(sb =>
                {
                    sb.Append("GameObjectTransformPool: Despawn item is null");
                });
                return;
            }
            
            // 从正在使用的引用列表中移除
            if (_spawnedItemHash.Remove(item))
            {
                _freeItemList.Add(item);
                item.transform.SetParent(_despawnParent, false);
                item.SetActive(false);
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
                item.transform.localScale = Vector3.one;
                if (null != _onDespawnToPool)
                {
                    await _onDespawnToPool(item);
                }
            }
            else
            {
                Log.LogError(sb =>
                {
                    sb.Append("GameObjectTransformPool: Despawn item not found");
                });
            }
        }

        /// <inheritdoc/>
        public async UniTask DespawnAll()
        {
            foreach (var item in _spawnedItemHash)
            {
                _freeItemList.Add(item);
                item.transform.SetParent(_despawnParent, false);
                item.SetActive(false);
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
                item.transform.localScale = Vector3.one;
                if (null != _onDespawnToPool)
                {
                    await _onDespawnToPool(item);
                }
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