/*------------------------------
 * 脚本名称: IReferencePoolItem
 * 创建者: movin
 * 创建日期: 2025/03/29
------------------------------*/

using Cysharp.Threading.Tasks;

namespace WWFramework
{
    /// <summary>
    /// 引用缓存池中存储的对象
    /// </summary>
    public interface IReferencePoolItem
    {
        /// <summary>
        /// 当从池中取出时调用
        /// </summary>
        /// <returns></returns>
        UniTask OnSpawnFromPool();

        /// <summary>
        /// 当放回池中时调用
        /// </summary>
        /// <returns></returns>
        UniTask OnDespawnToPool();
    }
}