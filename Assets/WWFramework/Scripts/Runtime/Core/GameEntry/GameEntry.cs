/*------------------------------
 * 脚本名称: GameEntry
 * 创建者: movin
 * 创建日期: 2025/03/18
------------------------------*/

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace WWFramework
{
    /// <summary>
    /// 游戏入口类,全局游戏入口
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        [Header("全局游戏配置")]
        public GameConfig globalGameConfig;

        #region 主包中各模块声明

        /// <summary>
        /// 流程模块
        /// </summary>
        public static MainProcedureModule mainProcedure;

        #endregion 主包中各模块生声明

        private void Awake()
        {
            // 初始化日志
            Log.Init(globalGameConfig.EnableLogType, globalGameConfig.EnableLogDebug, globalGameConfig.EnableLogWarning, globalGameConfig.EnableLogError);
            
            // 初始化各个模块
            mainProcedure = AddModule<MainProcedureModule>();
            mainProcedure.OnInit();
            
            // 模块初始化完成,开始执行主流程
            mainProcedure.StartMainProcedure().Forget();
        }
        
        private void OnDestroy()
        {
            // 释放各个模块并置空
            mainProcedure.OnRelease();
            mainProcedure = null;
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
                return obj.AddComponent(typeof(T)) as T;
            }
            // 对于C#对象模块,反射创建对象
            else
            {
                return Activator.CreateInstance(typeof(T)) as T;
            }
        }
    }
}