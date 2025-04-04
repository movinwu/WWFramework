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
    /// <para> 实现 <see cref="IPool"/> 的缓存池对象，都需要继承Monobehaviour。 </para>
    /// </summary>
    public interface IPool
    {
        /// <summary>
        /// 缓存池Id
        /// </summary>
        int PoolId { get; set; }
        
        /// <summary>
        /// 释放所有缓存
        /// </summary>
        /// <returns></returns>
        UniTask DespawnAll();
    }
}