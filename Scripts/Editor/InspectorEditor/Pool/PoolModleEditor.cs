/*------------------------------
 * 脚本名称: PoolModleEditor
 * 创建者: movin
 * 创建日期: 2025/03/30
------------------------------*/

using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 缓存池模块编辑器
    /// </summary>
    [CustomEditor(typeof(PoolModule))]
    public class PoolModleEditor : InspectorEditor
    {
        private bool _showPools = true;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // 获取PoolModule实例
            PoolModule poolModule = (PoolModule)target;

            _showPools = EditorGUILayout.Foldout(_showPools, "All Pools");
            if (_showPools)
            {
                // 反射获取所有缓存池
                var poolsField = typeof(PoolModule).GetField("_allPoolDic", BindingFlags.NonPublic | BindingFlags.Instance);
                if (poolsField != null)
                {
                    if (poolsField.GetValue(poolModule) is Dictionary<int, IPool> pools)
                    {
                        // 遍历所有缓存池
                        foreach (var pool in pools)
                        {
                            EditorGUILayout.BeginVertical("box");

                            // 显示缓存池ID
                            EditorGUILayout.LabelField($"Pool ID: {pool.Key}");

                            // 显示缓存池类型
                            EditorGUILayout.LabelField($"Pool Type: {pool.Value.GetType().Name}");

                            // 显示缓存池引用
                            EditorGUILayout.ObjectField(
                                obj: pool.Value as Object,
                                objType: typeof(Object),
                                allowSceneObjects: true
                            );

                            EditorGUILayout.EndVertical();
                        }
                    }
                }
            }
        }
    }
}