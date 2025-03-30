/*------------------------------
 * 脚本名称: GameObjectRectTransformPoolEditor
 * 创建者: movin
 * 创建日期: 2025/03/30
------------------------------*/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace WWFramework
{
    /// <summary>
    /// GameObjectRectTransformPool的Inspector编辑器
    /// </summary>
    [CustomEditor(typeof(GameObjectRectTransformPool))]
    public class GameObjectRectTransformPoolEditor : InspectorEditor
    {
        private bool _showFreeItems = true;
        private bool _showSpawnedItems = true;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            // 获取GameObjectRectTransformPool实例
            GameObjectRectTransformPool pool = (GameObjectRectTransformPool)target;

            _showFreeItems = EditorGUILayout.Foldout(_showFreeItems, "Free Items");
            if (_showFreeItems)
            {
                // 通过反射获取空闲对象列表
                var freeItemsField = typeof(GameObjectRectTransformPool).GetField("_freeItemList", BindingFlags.NonPublic | BindingFlags.Instance);
                if (freeItemsField != null)
                {
                    if (freeItemsField.GetValue(pool) is List<GameObjectPoolItem> freeItems)
                    {
                        // 遍历列表,每个元素缩进显示
                        foreach (var item in freeItems)
                        {
                            EditorGUILayout.ObjectField(
                                obj: item,
                                objType: typeof(GameObjectPoolItem),
                                allowSceneObjects: true
                            );
                        }
                    }
                }
            }

            // 展示缓存池中的已使用对象
            _showSpawnedItems = EditorGUILayout.Foldout(_showSpawnedItems, "Spawned Items");
            if (_showSpawnedItems)
            {
                // 通过反射获取已使用对象哈希表
                var spawnedItemsField = typeof(GameObjectRectTransformPool).GetField("_spawnedItemHash", BindingFlags.NonPublic | BindingFlags.Instance);
                if (spawnedItemsField != null)
                {
                    if (spawnedItemsField.GetValue(pool) is HashSet<GameObjectPoolItem> spawnedItems)
                    {
                        // 遍历集合,每个元素缩进显示
                        foreach (var item in spawnedItems)
                        {
                            EditorGUILayout.ObjectField(
                                obj: item,
                                objType: typeof(GameObjectPoolItem),
                                allowSceneObjects: true
                            );
                        }
                    }
                }
            }
        }
    }
}