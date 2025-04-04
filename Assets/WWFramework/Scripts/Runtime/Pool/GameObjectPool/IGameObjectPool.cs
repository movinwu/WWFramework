/*------------------------------
 * 脚本名称: IGameObjectPool
 * 创建者: movin
 * 创建日期: 2025/03/30
------------------------------*/

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 游戏物体缓存池接口
    /// </summary>
    internal interface IGameObjectPool : IPool
    {
        /// <summary>
        /// 将物体放回缓存池
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="isDestroy">物体是否已被销毁</param>
        /// <returns></returns>
        UniTask Despawn(GameObject gameObject, bool isDestroy = false);
    }
}