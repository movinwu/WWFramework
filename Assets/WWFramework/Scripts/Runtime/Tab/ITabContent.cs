/*------------------------------
 * 脚本名称: ITabContent
 * 创建者: movin
 * 创建日期: 2025/03/18
------------------------------*/

using Cysharp.Threading.Tasks;

namespace WWFramework
{
    /// <summary>
    /// 标签展示内容接口
    /// <para> 可以是对应标签展示的UI界面 </para>
    /// </summary>
    public interface ITabContent
    {
        /// <summary>
        /// 标签
        /// </summary>
        ITab Tab { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        UniTask OnInit();
        
        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        UniTask OnShow();
        
        /// <summary>
        /// 隐藏
        /// </summary>
        /// <returns></returns>
        UniTask OnHide();
        
        /// <summary>
        /// 销毁
        /// </summary>
        /// <returns></returns>
        UniTask OnDestroy();
    }
}