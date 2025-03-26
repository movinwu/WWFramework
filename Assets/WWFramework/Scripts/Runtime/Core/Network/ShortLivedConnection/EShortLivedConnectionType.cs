/*------------------------------
 * 脚本名称: EShortLivedConnectionType
 * 创建者: movin
 * 创建日期: 2025/03/26
------------------------------*/

namespace WWFramework
{
    /// <summary>
    /// 短连接类型
    /// </summary>
    public enum EShortLivedConnectionType : byte
    {
        /// <summary>
        /// unity的http连接 <see cref="UnityEngine.Networking.UnityWebRequest"/>
        /// </summary>
        UnityHttp,
        
        /// <summary>
        /// unity的https连接 <see cref="UnityEngine.Networking.UnityWebRequest"/>
        /// </summary>
        UnityHttps,
        
        /// <summary>
        /// netcore的http连接 <see cref="NetCoreServer.HttpClient"/>
        /// </summary>
        NetCoreHttp,
        
        /// <summary>
        /// netcore的https连接 <see cref="NetCoreServer.HttpsClient"/>
        /// </summary>
        NetCoreHttps,
    }
}