/*------------------------------
 * 脚本名称: AssetBundleInfoTab
 * 创建者: movin
 * 创建日期: 2025/04/05
------------------------------*/

using Cysharp.Threading.Tasks;

namespace WWFramework
{
    /// <summary>
    /// AB构建页签
    /// </summary>
    public class AssetBundleInfoTab : GlobalEditorTabBase
    {
        public override string Name => "AB构建策略";
        
        public override UniTask<ITabContent> CreateContent()
        {
            return UniTask.FromResult<ITabContent>(new AssetBundleInfoTabContent());
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