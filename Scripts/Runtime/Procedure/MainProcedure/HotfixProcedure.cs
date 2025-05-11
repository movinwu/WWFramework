/*------------------------------
 * 脚本名称: HotfixProcedure
 * 创建者: movin
 * 创建日期: 2025/03/19
------------------------------*/

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 热更新流程
    /// 1. 检查热更新资源
    /// 2. 下载热更新资源
    /// 3. 解析热更新资源
    /// </summary>
    public class HotfixProcedure : ProcedureBase
    {
        protected override UniTask DoExecute()
        {
            GameEntry.MainProcedure.GUIProcedures.Add(new HotfixProcedure());
            return base.DoExecute();
        }
    }
}