/*------------------------------
 * 脚本名称: EventModule
 * 创建者: movin
 * 创建日期: 2025/04/04
------------------------------*/

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 事件模块
    /// </summary>
    public class EventModule : EventCenter, IGameModule
    {
        public UniTask OnInit()
        {
            InitEventCenter();
            return UniTask.CompletedTask;
        }

        public void OnRelease()
        {
            ReleaseEventCenter();
        }
    }
}