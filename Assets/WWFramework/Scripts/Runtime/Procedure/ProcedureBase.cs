/*------------------------------
 * 脚本名称: GameModuleBase
 * 创建者: movin
 * 创建日期: 2025/03/19
------------------------------*/

using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace WWFramework
{
    /// <summary>
    /// 流程基类
    /// </summary>
    public class ProcedureBase
    {
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsFinished { get; private set; } = false;

        /// <summary>
        /// 进度
        /// </summary>
        public virtual (float current, float total) Progress => IsFinished ? (1, 1) : (0, 1);

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public async UniTask Execute()
        {
            IsFinished = false;
            await DoExecute();
            IsFinished = true;
        }

        /// <summary>
        /// 具体执行的内容
        /// </summary>
        /// <returns></returns>
        protected virtual UniTask DoExecute()
        {
            return UniTask.CompletedTask;
        }
    }
}