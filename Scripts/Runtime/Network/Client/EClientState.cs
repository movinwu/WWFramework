/*------------------------------
 * 脚本名称: EClientState
 * 创建者: movin
 * 创建日期: 2025/03/28
------------------------------*/

namespace WWFramework
{
    /// <summary>
    /// 客户端连接状态
    /// </summary>
    public enum EClientState : byte
    {
        /// <summary>
        /// 离线
        /// </summary>
        Offline,
        
        /// <summary>
        /// 连接中
        /// </summary>
        Connecting,
        
        /// <summary>
        /// 已连接
        /// </summary>
        Connected,
        
        /// <summary>
        /// 重连中
        /// </summary>
        Reconnecting,
        
        /// <summary>
        /// 断开连接中
        /// </summary>
        Disconnecting,
    }
}