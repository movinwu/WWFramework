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
#if NETCORE_TCP
        /// <summary>
        /// tcp客户端
        /// </summary>
        private TcpClient _tcpClient;
        
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
        /// <param name="address">服务器地址</param>
        /// <param name="port">服务器端口</param>
        public async UniTask ConnectAsync(string address, int port)
        {
            _tcpClient ??= new TcpClient(
                GameEntry.GlobalGameConfig.networkConfig.gameAddress,
                GameEntry.GlobalGameConfig.networkConfig.gamePort);
            
            _tcpClient = new TcpClient(address, port);
            if (_tcpClient.ConnectAsync())
            {
                var connectTask = UniTask.WaitUntil(() => _tcpClient.IsConnected);
                var timeoutTask = UniTask.Delay(5000);
                await UniTask.WhenAny(connectTask, timeoutTask);
                if (_tcpClient.IsConnected)
                {
                    Log.LogDebug(sb =>
                    {
                        sb.Append("网络连接成功");
                    }, ELogType.Network);
                }
                else
                {
                    Log.LogError(sb =>
                    {
                        sb.Append("网络连接超时");
                    }, ELogType.Network);
                    // 确保断开连接
                    if (_tcpClient.DisconnectAsync())
                    {
                        await UniTask.WaitUntil(() => !_tcpClient.IsConnected && !_tcpClient.IsConnecting);
                    }
                }
            }
            else
            {
                Log.LogError(sb =>
                {
                    sb.Append("发起网络连接失败");
                }, ELogType.Network);
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            if (_tcpClient != null)
            {
                _tcpClient.Disconnect();
                _tcpClient = null;
            }
        }
#endif
    }
}