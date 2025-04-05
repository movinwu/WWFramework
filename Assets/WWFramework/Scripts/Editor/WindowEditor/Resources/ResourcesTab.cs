/*------------------------------
 * 脚本名称: ResourcesTab
 * 创建者: movin
 * 创建日期: 2025/04/05
------------------------------*/

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 资源页签
    /// </summary>
    public class ResourcesTab : GlobalEditorTabBase
    {
        public override UniTask<ITabContent> CreateContent()
        {
            ITabContent content = new ResourcesTabContent();
            return UniTask.FromResult(content);
        }

        public override UniTask OnInit()
        {
            Name = "资源";
            return UniTask.CompletedTask;
        }

        public override UniTask OnDestroy()
        {
            return UniTask.CompletedTask;
        }
    }
}