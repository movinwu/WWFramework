/*------------------------------
 * 脚本名称: DataTableTab
 * 创建者: movin
 * 创建日期: 2025/05/04
------------------------------*/

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 数据表页签
    /// </summary>
    public class DataTableTab : GlobalEditorTabBase
    {
        public override string Name => "数据表导出";
        public override UniTask<ITabContent> CreateContent()
        {
            return UniTask.FromResult<ITabContent>(new DataTableTabContent());
        }

        public override UniTask OnInit()
        {
            return UniTask.CompletedTask;
        }

        public override UniTask OnRelease()
        {
            return UniTask.CompletedTask;
        }
    }
}