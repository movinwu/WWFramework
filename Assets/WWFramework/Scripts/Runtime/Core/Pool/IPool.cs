/*------------------------------
 * 脚本名称: IPool
 * 创建者: movin
 * 创建日期: 2025/03/29
------------------------------*/

using Cysharp.Threading.Tasks;

namespace WWFramework
{
    /// <summary>
    /// 缓存池接口
    /// </summary>
    public interface IPool
    {
        /// <summary>
        /// 释放所有缓存
        /// </summary>
        /// <returns></returns>
        UniTask DespawnAll();
    }
}