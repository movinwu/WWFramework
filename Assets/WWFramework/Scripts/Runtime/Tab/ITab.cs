/*------------------------------
 * 脚本名称: ITab
 * 创建者: movin
 * 创建日期: 2025/03/18
------------------------------*/

using Cysharp.Threading.Tasks;

namespace WWFramework
{
    /// <summary>
    /// 标签接口
    /// </summary>
    public interface ITab
    {
        /// <summary>
        /// 标签名,可以用于展示
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// 标签索引,用于排序
        /// </summary>
        int Index { get; set; }
        
        /// <summary>
        /// 标签内容
        /// </summary>
        ITabContent Content { get; set; }
        
        /// <summary>
        /// 标签组
        /// </summary>
        TabGroup Group { get; set; }

        /// <summary>
        /// 异步创建标签内容
        /// </summary>
        /// <returns></returns>
        UniTask<ITabContent> CreateContent();

        /// <summary>
        /// 当重新排布标签位置时调用
        /// </summary>
        /// <returns></returns>
        UniTask OnLayout();
        
        /// <summary>
        /// 异步初始化
        /// </summary>
        /// <returns></returns>
        UniTask OnInit();
        
        /// <summary>
        /// 异步显示
        /// </summary>
        /// <returns></returns>
        UniTask OnShow();

        /// <summary>
        /// 异步隐藏
        /// </summary>
        /// <returns></returns>
        UniTask OnHide();

        /// <summary>
        /// 异步销毁
        /// </summary>
        /// <returns></returns>
        UniTask OnRelease();
    }
}