/*------------------------------
 * 脚本名称: GameConfig
 * 创建者: movin
 * 创建日期: 2025/03/18
------------------------------*/

using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace WWFramework
{
    /// <summary>
    /// 游戏配置类,全局游戏配置
    /// </summary>
    public class GameConfig : ScriptableObject
    {
        [Header("日志配置")] public LogConfig logConfig;

        [Header("网络配置")] public NetworkConfig networkConfig;

        [Header("资源配置")] public ResourceConfig resourceConfig;

#if UNITY_EDITOR
        
        /// <summary>
        /// 检查配置是否存在,editor下创建不存在的配置文件
        /// </summary>
        [ContextMenu("检查所有配置")]
        public void CheckConfigs()
        {
            if (null == logConfig)
            {
                logConfig = CheckConfig<LogConfig>();
            }

            if (null == networkConfig)
            {
                networkConfig = CheckConfig<NetworkConfig>();
            }

            if (null == resourceConfig)
            {
                resourceConfig = CheckConfig<ResourceConfig>();
            }

            T CheckConfig<T>() where T : ScriptableObject
            {
                var path = $"{GlobalEditorStringDefine.GameConfigFolderPath}/{typeof(T).Name}.asset";
                var config = AssetDatabase.LoadAssetAtPath<T>(path);
                if (config == null)
                {
                    config = ScriptableObject.CreateInstance<T>();
                    AssetDatabase.CreateAsset(config, path);
                    AssetDatabase.SaveAssets();
                }

                return config;
            }
        }

#endif
    }
}