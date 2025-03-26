/*------------------------------
 * 脚本名称: IPersistentConnectionAdapter
 * 创建者: movin
 * 创建日期: 2025/03/26
------------------------------*/

using Cysharp.Threading.Tasks;

namespace WWFramework
{
    /// <summary>
    /// 长连接适配器接口
    /// <para> 定义如何适配长连接类型,方便拓展支持新的长连接类型 </para>
    /// </summary>
    public interface IPersistentConnectionAdapter
    {
        /// <summary>
        /// 校验连接地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        bool CheckAddress(string address);
        
        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        UniTask Connect();
        
        /// <summary>
        /// 发送字节数组
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        UniTask<bool> Send(byte[] data);
        
        /// <summary>
        /// 发送字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        UniTask<bool> Send(string text);

        /// <summary>
        /// 接收字节数组
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        UniTask Receive(byte[] data);
        
        /// <summary>
        /// 接收字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        UniTask Receive(string text);

        /// <summary>
        /// 重连
        /// </summary>
        UniTask ReConnect();
        
        /// <summary>
        /// 断开连接
        /// </summary>
        UniTask Disconnect();
    }
}