/*------------------------------
 * 脚本名称: AuthProcedure
 * 创建者: movin
 * 创建日期: 2025/03/19
------------------------------*/

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 认证流程
    /// <para> 游戏认证过程,确认服务器是否在线,请求服务器列表 </para>
    /// </summary>
    public class AuthProcedure : ProcedureBase
    {
        public override (float current, float total) Progress => (count, total);

        private int count = 0;
        private int total = 5;

        protected override async UniTask DoExecute()
        {
            count = 0;

            while (count < total)
            {
                await UniTask.Delay(3000, DelayType.DeltaTime);
                count++;
            }
            await base.DoExecute();
        }
    }
}