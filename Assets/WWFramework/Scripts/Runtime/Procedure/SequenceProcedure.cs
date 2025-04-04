/*------------------------------
 * 脚本名称: GameModuleBase
 * 创建者: movin
 * 创建日期: 2025/03/19
------------------------------*/

using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace WWFramework
{
    /// <summary>
    /// 顺序流程
    /// </summary>
    public sealed class SequenceProcedure : ProcedureBase
    {
        /// <summary>
        /// 所有等待顺序执行的流程
        /// </summary>
        public List<ProcedureBase> Procedures { get; private set; } = new List<ProcedureBase>();

        public override (float current, float total) Progress
        {
            get
            {
                return (Procedures.WhereArray(x => x.IsFinished).Length, Procedures.Count);
            }
        }
        
        /// <summary>
        /// 当前正在执行的流程
        /// </summary>
        public ProcedureBase CurrentExecutingProcedure => Procedures.First(x => !x.IsFinished);

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
            foreach (var procedure in Procedures)
            {
                await procedure.Execute();
            }
        }
    }
}