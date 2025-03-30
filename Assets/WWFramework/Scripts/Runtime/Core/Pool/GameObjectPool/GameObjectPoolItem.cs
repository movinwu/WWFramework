/*------------------------------
 * 脚本名称: GameObjectPoolItem
 * 创建者: movin
 * 创建日期: 2025/03/30
------------------------------*/

using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 游戏物体缓存池元素标识脚本
    /// </summary>
    public class GameObjectPoolItem : MonoBehaviour
    {
        internal IGameObjectPool Pool;
        
        private void OnDestroy()
        {
            if (Pool != null)
            {
                Pool.Despawn(gameObject, true);
                Pool = null;
            }
        }
    }
}