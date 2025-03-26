/*------------------------------
 * 脚本名称: EPersistentConnectionType
 * 创建者: movin
 * 创建日期: 2025/03/26
------------------------------*/

namespace WWFramework
{
    /// <summary>
    /// 长连接类型
    /// </summary>
    public enum EPersistentConnectionType : byte
    {
        /// <summary>
        /// NetCore的Tcp连接 <see cref="NetCoreServer.TcpClient"/>
        /// </summary>
        NetCoreTcp,
        
        /// <summary>
        /// NetCore的Udp连接 <see cref="NetCoreServer.UdpClient"/>
        /// </summary>
        NetCoreUdp,
        
        /// <summary>
        /// NetCore的WebSocket连接(Ws) <see cref="NetCoreServer.WsClient"/>
        /// </summary>
        NetCoreWs,
        
        /// <summary>
        /// NetCore的WebSocket连接(Wss) <see cref="NetCoreServer.WssClient"/>
        /// </summary>
        NetCoreWss,
        
        /// <summary>
        /// UnityWebSocket的WebSocket连接(Websocket) <see cref="UnityWebSocket.WebSocket"/>
        /// </summary>
        UnityWebSocketWebsocket,
    }
}