/*------------------------------
 * 脚本名称: NetworkConfig
 * 创建者: movin
 * 创建日期: 2025/03/27
------------------------------*/

using UnityEngine;
using UnityEngine.Serialization;

namespace WWFramework
{
    /// <summary>
    /// 网络连接配置
    /// </summary>
    [CreateAssetMenu(
        fileName = GlobalEditorStringDefine.NetworkConfigName, 
        menuName = GlobalEditorStringDefine.NetworkConfig, 
        order = GlobalEditorPriorityDefine.NetworkConfig)]
    public class NetworkConfig : ScriptableObject
    {
        [Header("大厅服务器地址")] public string lobbyAddress = "127.0.0.1";
        [Header("游戏服务器地址")] public string gameAddress = "127.0.0.1";
        [Header("游戏服务器端口")] public int gamePort = 8080;
        
        [Header("连接超时时间(毫秒)")] public int connectTimeout = 5000;
        [Header("重连间隔(毫秒)")] public int reconnectInterval = 5000;
        [Header("重连次数")] public int reconnectCount = 3;
    }
}