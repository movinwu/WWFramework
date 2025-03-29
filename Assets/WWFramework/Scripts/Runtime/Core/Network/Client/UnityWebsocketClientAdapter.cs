/*------------------------------
 * 脚本名称: UnityWebsocketClientAdapter
 * 创建者: movin
 * 创建日期: 2025/03/29
------------------------------*/

using System;
using Cysharp.Threading.Tasks;
using UnityWebSocket;

namespace WWFramework
{
    /// <summary>
    /// UnityWebSocket 客户端适配器
    /// </summary>
    public class UnityWebsocketClientAdapter : IClientAdapter
    {
        /// <summary>
        /// 地址
        /// </summary>
        private string _address;

        /// <summary>
        /// 端口
        /// </summary>
        private int _port;
        
        private IWebSocket _webSocket;
        
        /// <inheritdoc/>
        public Action<byte[], int> OnPacketReceived { get; set; }
        
        /// <inheritdoc/>
        public Action OnUnexpectedDisconnect { get; set; }
        
        public UnityWebsocketClientAdapter(string address, int port)
        {
            this._address = address;
            this._port = port;
            this._webSocket = new WebSocket($"{_address}:{_port}");
            this._webSocket.OnOpen += Socket_OnOpen;
            this._webSocket.OnMessage += Socket_OnMessage;
            this._webSocket.OnClose += Socket_OnClose;
            this._webSocket.OnError += Socket_OnError;
            
            void Socket_OnOpen(object sender, OpenEventArgs e)
            {
                Log.LogDebug(sb => { sb.Append("网络连接成功"); }, ELogType.Network);
            }

            void Socket_OnMessage(object sender, MessageEventArgs e)
            {
                if (e.IsBinary)
                {
                    OnPacketReceived?.Invoke(e.RawData, e.RawData.Length);
                }
                else
                {
                    // todo 暂时不处理文本消息
                }
            }

            void Socket_OnClose(object sender, CloseEventArgs e)
            {
                Log.LogDebug(sb => { sb.Append("网络连接断开"); }, ELogType.Network);
                OnUnexpectedDisconnect?.Invoke();
            }

            void Socket_OnError(object sender, ErrorEventArgs e)
            {
                Log.LogError(sb => { sb.Append("网络连接出错"); }, ELogType.Network);
            }
        }
        
        /// <inheritdoc/>
        public async UniTask<bool> AsyncConnect(int connectTime)
        {
            this._webSocket.ConnectAsync();
            var connectTask = UniTask.WaitUntil(() => this._webSocket.ReadyState == WebSocketState.Open);
            var timeoutTask = UniTask.Delay(connectTime);
            await UniTask.WhenAny(connectTask, timeoutTask);
            if (this._webSocket.ReadyState == WebSocketState.Open)
            {
                Log.LogDebug(sb => { sb.Append("网络连接成功"); }, ELogType.Network);
                return true;
            }
            else
            {
                Log.LogError(sb => { sb.Append("网络连接超时"); }, ELogType.Network);
                // 确保断开连接
                await AsyncDisconnect();

                return false;
            }
        }

        /// <inheritdoc/>
        public UniTask<bool> AsyncSend(byte[] buffer, int offset, int size)
        {
            if (offset != 0 || size != buffer.Length)
            {
                var newBuffer = new byte[size];
                Array.Copy(buffer, offset, newBuffer, 0, size);
                this._webSocket.SendAsync(newBuffer);
            }
            else
            {
                this._webSocket.SendAsync(buffer);
            }
            return UniTask.FromResult(this._webSocket.ReadyState == WebSocketState.Open);
        }

        /// <inheritdoc/>
        public async UniTask<bool> AsyncReconnect(int reconnectTime)
        {
            // 确保断开连接
            await UniTask.WhenAny(UniTask.Delay(reconnectTime), ReconnectTask());
            if (this._webSocket.ReadyState == WebSocketState.Open)
            {
                Log.LogDebug(sb => { sb.Append("网络重连成功"); }, ELogType.Network);
                return true;
            }
            else
            {
                Log.LogError(sb => { sb.Append("网络重连超时"); }, ELogType.Network);
                // 确保断开连接
                await AsyncDisconnect();
                return false;
            }

            async UniTask ReconnectTask()
            {
                this._webSocket.CloseAsync();
                await UniTask.WaitUntil(() => this._webSocket.ReadyState == WebSocketState.Closed);
                this._webSocket.ConnectAsync();
                await UniTask.WaitUntil(() => this._webSocket.ReadyState == WebSocketState.Open);
            }
        }

        /// <inheritdoc/>
        public UniTask<bool> AsyncDisconnect()
        {
            this._webSocket.CloseAsync();
            return UniTask.FromResult(true);
        }
    }
}