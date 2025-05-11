/*------------------------------
 * 脚本名称: GameEntry
 * 创建者: movin
 * 创建日期: 2025/03/18
------------------------------*/

using System;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 游戏入口类,全局游戏入口
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        [Header("全局游戏配置"), SerializeField] private GameConfig globalGameConfig;

        private static GameConfig _globalGameConfig;

        /// <summary>
        /// 全局游戏配置
        /// </summary>
        public static GameConfig GlobalGameConfig
        {
            get
            {
                if (null == _globalGameConfig)
                {
#if UNITY_EDITOR
                    // 编辑器下确保配置文件存在
                    CheckGlobalConfigExist();
#else
                    throw new Exception("请确保全局游戏配置文件已生成并正确赋值");
#endif
                }

                return _globalGameConfig;
            }
        }

        #region 主包中各模块声明

        /// <summary>
        /// 流程模块
        /// </summary>
        public static MainProcedureModule MainProcedure { get; private set; }

        /// <summary>
        /// 网络模块
        /// </summary>
        public static NetworkClientModule NetworkClient { get; private set; }

        /// <summary>
        /// 池模块
        /// </summary>
        public static PoolModule Pool { get; private set; }

        /// <summary>
        /// 事件模块
        /// </summary>
        public static EventModule Event { get; private set; }

        #endregion 主包中各模块生声明

        private void Awake()
        {
            // 不销毁
            DontDestroyOnLoad(this.gameObject);
            
            // 赋值全局游戏配置
            _globalGameConfig = globalGameConfig;
            
            // 初始化各个模块
            MainProcedure = AddModule<MainProcedureModule>();
            NetworkClient = AddModule<NetworkClientModule>();
            Pool = AddModule<PoolModule>();
            Event = AddModule<EventModule>();

            // 模块初始化完成,开始执行主流程
            MainProcedure.StartMainProcedure().Forget();
        }

        private void OnDestroy()
        {
            // 释放各个模块并置空
            MainProcedure.OnRelease();
            NetworkClient.OnRelease();
            Pool.OnRelease();
            Event.OnRelease();
            MainProcedure = null;
            NetworkClient = null;
            Pool = null;
            Event = null;
        }

        /// <summary>
        /// 添加模块
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T AddModule<T>() where T : class, IGameModule
        {
            // 对于组件模块,创建子物体并挂载
            if (typeof(T).IsSubclassOf(typeof(Component)))
            {
                var obj = new GameObject(typeof(T).Name);
                obj.transform.SetParent(transform);
                var module = obj.AddComponent(typeof(T)) as T;
                module.OnInit();
                return module;
            }
            // 对于C#对象模块,反射创建对象
            else
            {
                var module = Activator.CreateInstance(typeof(T)) as T;
                module.OnInit();
                return module;
            }
        }

#if UNITY_EDITOR
        
        /// <summary>
        /// 检查全局配置
        /// </summary>
        [ContextMenu("检查全局配置")]
        private void CheckGlobalConfig()
        {
            CheckGlobalConfigExist();
            globalGameConfig = _globalGameConfig;
        }

        /// <summary>
        /// 检查全局配置文件是否存在
        /// </summary>
        private static void CheckGlobalConfigExist()
        {
            var path = $"{GlobalEditorStringDefine.GameConfigFolderPath}/{nameof(GameConfig)}.asset";
            _globalGameConfig = AssetDatabase.LoadAssetAtPath<GameConfig>(path);
            if (null == _globalGameConfig)
            {
                _globalGameConfig = ScriptableObject.CreateInstance<GameConfig>();
            }
            _globalGameConfig.CheckConfigs();
            EditorUtility.SetDirty(_globalGameConfig);
            AssetDatabase.SaveAssetIfDirty(_globalGameConfig);
        }
#endif
    }
}