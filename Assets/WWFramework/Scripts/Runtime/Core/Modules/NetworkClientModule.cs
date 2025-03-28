/*------------------------------
 * 脚本名称: NetworkModule
 * 创建者: movin
 * 创建日期: 2025/03/26
------------------------------*/

#define NETCORE_TCP // 定义使用NetCoreTcp

using Cysharp.Threading.Tasks;

namespace WWFramework
{
    /// <summary>
    /// 网络客户端模块
    /// </summary>
    public class NetworkClientModule : IGameModule
    {
        /// <summary>
        /// 客户端适配器
        /// </summary>
        private IClientAdapter _clientAdapter;

        /// <summary>
        /// 重连开始回调
        /// </summary>
        public System.Action OnReconnectStart;
        
        /// <summary>
        /// 重连成功回调
        /// </summary>
        public System.Action OnReconnectSuccess;
        
        /// <summary>
        /// 重连失败回调
        /// </summary>
        public System.Action<int> OnReconnectFail;
        
        /// <summary>
        /// 连接成功回调
        /// </summary>
        public System.Action OnConnectedSuccess;

        /// <summary>
        /// 连接失败回调
        /// </summary>
        public System.Action OnConnectedFailed;

        /// <summary>
        /// 断开连接回调
        /// </summary>
        public System.Action OnDisconnected;
        
        /// <summary>
        /// 当前的连接状态
        /// </summary>
        public EClientState ClientState { get; private set; } = EClientState.Offline;
        
        public UniTask OnInit()
        {
            return UniTask.CompletedTask;
        }

        public void OnRelease()
        {
        }

        /// <summary>
        /// 异步发起连接
        /// </summary>
        public async UniTask Connect()
        {
            if (ClientState != EClientState.Offline)
            {
                return;
            }
            
            if (null == _clientAdapter)
            {
#if NETCORE_TCP
                _clientAdapter = new TcpClientAdapter(
                    GameEntry.GlobalGameConfig.networkConfig.gameAddress,
                    GameEntry.GlobalGameConfig.networkConfig.gamePort);
#endif
                _clientAdapter.OnPacketReceived = OnPacketReceived;
                _clientAdapter.OnUnexpectedDisconnect = () =>
                {
                    // 还在连接状态,说明首次意外断开连接,尝试重连
                    if (ClientState == EClientState.Connected)
                    {
                        // 发起重连
                        Reconnect().Forget();
                    }
                };
            }
            
            ClientState = EClientState.Connecting;
            var connected = await _clientAdapter.AsyncConnect(GameEntry.GlobalGameConfig.networkConfig.connectTimeout);
            if (connected)
            {
                ClientState = EClientState.Connected;
                OnConnectedSuccess?.Invoke();
            }
            else
            {
                ClientState = EClientState.Offline;
                OnConnectedFailed?.Invoke();
            }
        }
        
        /// <summary>
        /// 重连
        /// </summary>
        public async UniTask<bool> Reconnect()
        {
            if (ClientState != EClientState.Connected)
            {
                return false;
            }
            
            if (_clientAdapter != null)
            {
                ClientState = EClientState.Reconnecting;
                var count = 0;
                var waitTime = GameEntry.GlobalGameConfig.networkConfig.reconnectInterval;
                var waitTotalCount = GameEntry.GlobalGameConfig.networkConfig.reconnectCount;
                OnReconnectStart?.Invoke();
                while (count < waitTotalCount)
                {
                    count++;
                    var isConnected = await _clientAdapter.AsyncReconnect(waitTime);
                    if (isConnected)
                    {
                        ClientState = EClientState.Connected;
                        OnReconnectSuccess?.Invoke();
                        return true;
                    }
                    else
                    {
                        OnReconnectFail?.Invoke(count);
                    }
                }
                
                ClientState = EClientState.Offline;
                // 重连失败,视为彻底断开连接
                OnDisconnected?.Invoke();
            }
                
            return false;
        }
        
        public async UniTask Send(byte[] buffer)
        {
            if (ClientState != EClientState.Connected)
            {
                return;
            }
            
            if (_clientAdapter != null)
            {
                await _clientAdapter.AsyncSend(buffer);
            }
            else
            {
                // 如果发送不成功,视为连接断开,开始尝试重连
                var reconnected = await Reconnect();
                // 重连成功,继续发送消息
                if (reconnected)
                {
                    await Send(buffer);
                }
                // 重连不成功,放弃发送
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public async UniTask Disconnect()
        {
            if (ClientState != EClientState.Connected)
            {
                return;
            }
            
            if (_clientAdapter != null)
            {
                ClientState = EClientState.Disconnecting;
                await _clientAdapter.AsyncDisconnect();
                _clientAdapter = null;
                ClientState = EClientState.Offline;
            }
        }

        /// <summary>
        /// 当收到完整包时调用
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="length"></param>
        private void OnPacketReceived(byte[] packet, int length)
        {
            Temp = System.Text.Encoding.UTF8.GetString(packet, 0, length);
        }

        public string Temp;
    }
}