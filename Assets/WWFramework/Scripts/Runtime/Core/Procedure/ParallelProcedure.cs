/*------------------------------
 * 脚本名称: ParallelProcedure
 * 创建者: movin
 * 创建日期: 2025/03/19
------------------------------*/

using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace WWFramework
{
    /// <summary>
    /// 并行流程
    /// </summary>
    public sealed class ParallelProcedure : ProcedureBase
    {
        private readonly List<ProcedureBase> _procedures = new List<ProcedureBase>();

        public override (float progress, float total) Progress => (_procedures.WhereArray(x => x.IsFinished).Length, _procedures.Count);

        /// <summary>
        /// 添加流程
        /// </summary>
        /// <param name="procedure"></param>
        public void AddProcedure(ProcedureBase procedure)
        {
            _procedures.Add(procedure);
        }

        protected override async UniTask DoExecute()
        {
            await UniTask.WhenAll(_procedures.SelectArray(x => x.Execute()));
        }
    }
}