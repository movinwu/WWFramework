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
        /// <summary>
        /// 所有并行的流程
        /// </summary>
        public List<ProcedureBase> Procedures { get; private set; } = new List<ProcedureBase>();

        public override (float current, float total) Progress => (Procedures.WhereArray(x => x.IsFinished).Length, Procedures.Count);

        /// <summary>
        /// 添加流程
        /// </summary>
        /// <param name="procedure"></param>
        public void AddProcedure(ProcedureBase procedure)
        {
            Procedures.Add(procedure);
        }

        protected override async UniTask DoExecute()
        {
            await UniTask.WhenAll(Procedures.SelectArray(x => x.Execute()));
        }
    }
}