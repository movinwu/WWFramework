/*------------------------------
 * 脚本名称: AssetBundleBuildConfigTab
 * 创建者: movin
 * 创建日期: 2025/04/05
------------------------------*/

using Cysharp.Threading.Tasks;

namespace WWFramework
{
    /// <summary>
    /// AB资源构建配置页签
    /// </summary>
    public class AssetBundleBuildConfigTab : ITab
    {
        /// <summary>
        /// AB资源构建配置
        /// </summary>
        public readonly AssetBundleBuildConfig Config;

        public string Name => Config?.name ?? "空配置";
        public int Index { get; set; }
        public ITabContent Content { get; set; }
        public TabGroup Group { get; set; }
        
        public AssetBundleBuildConfigTab(AssetBundleBuildConfig config)
        {
            Config = config;
        }
        
        public UniTask<ITabContent> CreateContent()
        {
            return UniTask.FromResult<ITabContent>(Config);
        }

        public UniTask OnLayout()
        {
            return UniTask.CompletedTask;
        }

        public UniTask OnInit()
        {
            return UniTask.CompletedTask;
        }

        public UniTask OnShow()
        {
            return UniTask.CompletedTask;
        }

        public UniTask OnHide()
        {
            return UniTask.CompletedTask;
        }

        public UniTask OnDestroy()
        {
            return UniTask.CompletedTask;
        }
    }
}