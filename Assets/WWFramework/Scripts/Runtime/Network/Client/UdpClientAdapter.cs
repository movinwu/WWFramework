/*------------------------------
 * 脚本名称: UdpClientAdapter
 * 创建者: movin
 * 创建日期: 2025/03/28
------------------------------*/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Cysharp.Threading.Tasks;
using UdpClient = NetCoreServer.UdpClient;

namespace WWFramework
{
    /// <summary>
    /// udp 客户端连接适配器
    /// <para> Udp不适合处理大数据包,如果需要处理大量大数据包时,请使用Tcp </para>
    /// <para> 每次发送或接收到大数据包时会打印警告并不予处理,大量此类警告出现,需要注意切分数据包或换成其他可靠协议 </para>
    /// </summary>
    public class UdpClientAdapter : UdpClient, IClientAdapter
    {
        /// <summary>
        /// UDP 数据包安全长度,超过这个长度视为大数据包
        /// </summary>
        private const int PacketSafeLength = 1400;

        /// <summary>
        /// 缓存数据包
        /// </summary>
        private static readonly byte[] Buffer = new byte[PacketSafeLength];

        public UdpClientAdapter(IPAddress address, int port) : base(address, port)
        {
        }

        public UdpClientAdapter(string address, int port) : base(address, port)
        {
        }

        public UdpClientAdapter(IPEndPoint endpoint) : base(endpoint)
        {
        }

        protected override void OnConnected()
        {
            // 开始接收数据包
            ReceiveAsync();
        }

        /// <inheritdoc/>
        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            // 不处理数据包的分片,采用规避分片的方式
            // 前后端发送给时都需要校验是否是大数据包,对应大数据包进行提示,不予发送
            if (size > PacketSafeLength)
            {
                Log.LogWarning(sb => { sb.Append("收到大数据包,不予处理"); }, ELogType.Network);
            }
            else
            {
                Array.Copy(buffer, offset, Buffer, 0, (int)size);
                OnPacketReceived?.Invoke(Buffer, (int)size);
            }

            // 重新接收数据包
            ReceiveAsync();
        }

        /// <inheritdoc/>
        protected override void OnDisconnected()
        {
            base.OnDisconnected();
            OnUnexpectedDisconnect?.Invoke();
        }

        /// <inheritdoc/>
        public override bool SendAsync(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            if (size > PacketSafeLength)
            {
                Log.LogWarning(sb => { sb.Append("发送大数据包,不予处理"); }, ELogType.Network);
                return false;
            }

            return base.SendAsync(endpoint, buffer, offset, size);
        }

        /// <inheritdoc/>
        protected override void OnError(SocketError error)
        {
            base.OnError(error);
            Log.LogError(sb =>
            {
                sb.Append("网络连接失败");
                sb.Append($"错误码:{error}");
            }, ELogType.Network);
        }

        /// <inheritdoc/>
        public Action<byte[], int> OnPacketReceived { get; set; }

        /// <inheritdoc/>
        public Action OnUnexpectedDisconnect { get; set; }

        /// <inheritdoc/>
        public UniTask<bool> AsyncConnect(int connectTime)
        {
            if (this.Connect())
            {
                return UniTask.FromResult(true);
            }
            else
            {
                Log.LogError(sb => { sb.Append("发起网络连接失败"); }, ELogType.Network);
                return UniTask.FromResult(false);
            }
        }

        /// <inheritdoc/>
        public UniTask<bool> AsyncSend(byte[] buffer, int offset, int size)
        {
            if (size > PacketSafeLength)
            {
                Log.LogWarning(sb => { sb.Append("发送大数据包,不予处理"); });
                return UniTask.FromResult(false);
            }

            return UniTask.FromResult(SendAsync(buffer, offset, size));
        }

        /// <inheritdoc/>
        public UniTask<bool> AsyncReconnect(int reconnectTime)
        {
            // udp 不发起重连,直接视为连接失败,断开连接
            return UniTask.FromResult(false);
        }

        /// <inheritdoc/>
        public UniTask<bool> AsyncDisconnect()
        {
            return UniTask.FromResult(this.Disconnect());
        }
    }
}