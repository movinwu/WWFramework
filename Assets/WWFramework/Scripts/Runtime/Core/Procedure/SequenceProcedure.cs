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
        private readonly List<ProcedureBase> _procedures = new List<ProcedureBase>();

        public override (float progress, float total) Progress
        {
            get
            {
                return (_procedures.WhereArray(x => x.IsFinished).Length, _procedures.Count);
            }
        }
        
        /// <summary>
        /// 当前正在执行的流程
        /// </summary>
        public ProcedureBase CurrentExecutingProcedure => _procedures.First(x => !x.IsFinished);

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
            foreach (var procedure in _procedures)
            {
                await procedure.Execute();
            }
        }
    }
}