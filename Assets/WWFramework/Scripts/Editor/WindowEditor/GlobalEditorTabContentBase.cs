/*------------------------------
 * 脚本名称: GlobalEditorTabContentBase
 * 创建者: movin
 * 创建日期: 2025/04/05
------------------------------*/

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 全局编辑器Tab内容基类
    /// </summary>
    public abstract class GlobalEditorTabContentBase : ITabContent
    {
        public ITab Tab { get; set; }
        public abstract UniTask OnInit();

        /// <summary>
        /// GUI函数调用
        /// </summary>
        public abstract void OnDrawGUI();

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