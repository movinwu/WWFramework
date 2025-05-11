/*------------------------------
 * 脚本名称: GameModuleBase
 * 创建者: movin
 * 创建日期: 2025/03/19
------------------------------*/

using Cysharp.Threading.Tasks;

namespace WWFramework
{
    /// <summary>
    /// 游戏模块接口
    /// </summary>
    public interface IGameModule
    {
        /// <summary>
        /// 模块初始化
        /// </summary>
        UniTask OnInit();

        /// <summary>
        /// 模块释放
        /// </summary>
        void OnRelease();
    }
}