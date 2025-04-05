/*------------------------------
 * 脚本名称: GlobalEditorTabBase
 * 创建者: movin
 * 创建日期: 2025/04/05
------------------------------*/

using Cysharp.Threading.Tasks;

namespace WWFramework
{
    /// <summary>
    /// 全局编辑器Tab基类
    /// </summary>
    public abstract class GlobalEditorTabBase : ITab
    {
        public abstract string Name { get; }
        public int Index { get; set; }
        public ITabContent Content { get; set; }
        public TabGroup Group { get; set; }

        public abstract UniTask<ITabContent> CreateContent();

        public UniTask OnLayout()
        {
            return UniTask.CompletedTask;
        }

        public abstract UniTask OnInit();

        /// <summary>
        /// GUI绘制时调用
        /// </summary>
        /// <param name="isSelected">是否选中</param>
        public virtual void OnGUI(bool isSelected)
        {
            
        }
        
        public virtual UniTask OnShow()
        {
            return UniTask.CompletedTask;
        }

        public virtual UniTask OnHide()
        {
            return UniTask.CompletedTask;
        }

        public abstract UniTask OnDestroy();
    }
}