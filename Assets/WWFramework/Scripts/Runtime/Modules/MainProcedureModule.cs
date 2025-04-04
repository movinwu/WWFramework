/*------------------------------
 * 脚本名称: GameProcedureModule
 * 创建者: movin
 * 创建日期: 2025/03/19
------------------------------*/

using System.Collections.Generic;
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
        /// 需要在GUI中展示的所有流程
        /// <para> 如果一个流程需要展示在GUI中,则需要添加到GUIProcedures中 </para>
        /// </summary>
        public List<ProcedureBase> GUIProcedures { get; private set; } = new List<ProcedureBase>();
        
        /// <summary>
        /// 游戏主流程
        /// </summary>
        public SequenceProcedure MainProcedure { get; private set; }

        public UniTask OnInit()
        {
            MainProcedure = new SequenceProcedure();
            // SDK初始化
            MainProcedure.AddProcedure(new SDKInitProcedure());
            // 热更新和认证同时执行
            var parallelProcedure = new ParallelProcedure();
            parallelProcedure.AddProcedure(new HotfixProcedure());
            parallelProcedure.AddProcedure(new AuthProcedure());
            MainProcedure.AddProcedure(parallelProcedure);
            // 登陆
            MainProcedure.AddProcedure(new LoginProcedure());

            return UniTask.CompletedTask;
        }

        public async UniTask StartMainProcedure()
        {
            // 开始执行
            await MainProcedure.Execute();
        }

        public void OnRelease()
        {
        }
    }
}