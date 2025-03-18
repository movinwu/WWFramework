/*------------------------------
 * 脚本名称: GameEntry
 * 创建者: movin
 * 创建日期: 2025/03/18
------------------------------*/

using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 游戏入口类,全局游戏入口
    /// </summary>
    public class GameEntry : MonoBehaviour
    {
        [Header("全局游戏配置")]
        public GameConfig GlobalGameConfig;

        private void Awake()
        {
            // 初始化日志
            Log.Init(GlobalGameConfig.EnableLogType, GlobalGameConfig.EnableLogDebug, GlobalGameConfig.EnableLogWarning, GlobalGameConfig.EnableLogError);
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            
        }

        private void LateUpdate()
        {
            
        }

        private void OnDestroy()
        {
            
        }

        private void FixUpdate()
        {
            
        }
    }
}