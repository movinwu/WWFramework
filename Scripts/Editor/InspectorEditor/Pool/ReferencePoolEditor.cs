/*------------------------------
 * 脚本名称: ReferencePoolEditor
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
    /// 引用缓存池的Inspector编辑器
    /// </summary>
    [CustomEditor(typeof(ReferencePool))]
    public class ReferencePoolEditor : InspectorEditor
    {
        private bool _showFreeItems = true;
        private bool _showSpawnedItems = true;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            // 获取ReferencePool实例
            ReferencePool pool = (ReferencePool)target;

            _showFreeItems = EditorGUILayout.Foldout(_showFreeItems, "Free Items");
            if (_showFreeItems)
            {
                // 通过反射获取空闲对象字典
                var freeItemsField = typeof(ReferencePool).GetField("_freeItemDic", BindingFlags.NonPublic | BindingFlags.Instance);
                if (freeItemsField != null)
                {
                    if (freeItemsField.GetValue(pool) is Dictionary<int, List<IReferencePoolItem>> freeItems)
                    {
                        // 遍历字典
                        foreach (var kvp in freeItems)
                        {
                            // 类型缩进显示
                            EditorGUILayout.LabelField($"   {kvp.Key}");
                            // 遍历集合,每个元素缩进两次显示
                            foreach (var item in kvp.Value)
                            {
                                EditorGUILayout.LabelField($"       {item.GetType().Name}_{item.GetHashCode()}");
                            }
                        }
                    }
                }
            }

            // 展示缓存池中的已使用对象
            _showSpawnedItems = EditorGUILayout.Foldout(_showSpawnedItems, "Spawned Items");
            if (_showSpawnedItems)
            {
                // 通过反射获取已使用对象哈希表
                var spawnedItemsField = typeof(ReferencePool).GetField("_spawnedItemHash", BindingFlags.NonPublic | BindingFlags.Instance);
                if (spawnedItemsField != null)
                {
                    if (spawnedItemsField.GetValue(pool) is HashSet<IReferencePoolItem> spawnedItems)
                    {
                        // 遍历集合,每个元素缩进显示
                        foreach (var item in spawnedItems)
                        {
                            EditorGUILayout.LabelField($"   {item.GetType().Name}_{item.GetHashCode()}");
                        }
                    }
                }
            }
        }
    }
}