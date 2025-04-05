/*------------------------------
 * 脚本名称: AssetBundleBuildTab
 * 创建者: movin
 * 创建日期: 2025/04/05
------------------------------*/

using Cysharp.Threading.Tasks;

namespace WWFramework
{
    /// <summary>
    /// AB构建页签
    /// </summary>
    public class AssetBundleBuildTab : GlobalEditorTabBase
    {
        public override string Name => "AB构建策略";
        
        public override UniTask<ITabContent> CreateContent()
        {
            return UniTask.FromResult<ITabContent>(new AssetBundleBuildTabContent());
        }

        public override UniTask OnInit()
        {
            return UniTask.CompletedTask;
        }

        public override UniTask OnDestroy()
        {
            return UniTask.CompletedTask;
        }
    }
}