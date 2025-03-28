/*------------------------------
 * 脚本名称: IClientAdapter
 * 创建者: movin
 * 创建日期: 2025/03/27
------------------------------*/

using System;
using Cysharp.Threading.Tasks;

namespace WWFramework
{
    /// <summary>
    /// 客户端适配器
    /// </summary>
    public interface IClientAdapter
    {
        /// <summary>
        /// 数据包接收事件
        /// </summary>
        Action<byte[], int> OnPacketReceived { get; set; }
        
        /// <summary>
        /// 意外断开连接事件
        /// </summary>
        Action OnUnexpectedDisconnect { get; set; }

        /// <summary>
        /// 异步发起连接
        /// </summary>
        /// <param name="connectTime">连接等待时间(毫秒值)</param>
        UniTask<bool> AsyncConnect(int connectTime);

        /// <summary>
        /// 异步发送数据包
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        UniTask<bool> AsyncSend(byte[] buffer);

        /// <summary>
        /// 异步重连
        /// </summary>
        /// <param name="reconnectTime">重连等待时间(毫秒值)</param>
        UniTask<bool> AsyncReconnect(int reconnectTime);

        /// <summary>
        /// 断开连接
        /// </summary>
        UniTask<bool> AsyncDisconnect();
    }
}