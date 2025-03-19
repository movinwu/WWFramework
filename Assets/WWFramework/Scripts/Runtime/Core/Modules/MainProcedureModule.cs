/*------------------------------
 * 脚本名称: GameProcedureModule
 * 创建者: movin
 * 创建日期: 2025/03/19
------------------------------*/

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 游戏主流程模块
    /// <para> 管理游戏各阶段任务的执行顺序,如sdk初始化\挂接登陆\资源加载等 </para>
    /// </summary>
    public class MainProcedureModule : MonoBehaviour, IGameModule
    {
        /// <summary>
        /// 游戏主流程
        /// </summary>
        private SequenceProcedure _mainProcedure;
        
        public UniTask OnInit()
        {
            _mainProcedure = new SequenceProcedure();
            // SDK初始化
            _mainProcedure.AddProcedure(new SDKInitProcedure());
            // 热更新和认证同时执行
            var parallelProcedure = new ParallelProcedure();
            parallelProcedure.AddProcedure(new HotfixProcedure());
            parallelProcedure.AddProcedure(new AuthProcedure());
            _mainProcedure.AddProcedure(parallelProcedure);
            // 登陆
            _mainProcedure.AddProcedure(new LoginProcedure());

            return UniTask.CompletedTask;
        }

        public async UniTask StartMainProcedure()
        {
            // 开始执行
            await _mainProcedure.Execute();
        }

        public void OnRelease()
        {
            
        }
    }
}