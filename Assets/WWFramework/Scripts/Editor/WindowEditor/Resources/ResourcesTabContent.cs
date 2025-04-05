/*------------------------------
 * 脚本名称: ResourcesTabContent
 * 创建者: movin
 * 创建日期: 2025/04/05
------------------------------*/

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 资源页签内容
    /// </summary>
    public class ResourcesTabContent : GlobalEditorTabContentBase
    {
        public override UniTask OnInit()
        {
            return UniTask.CompletedTask;
        }

        public override UniTask OnDestroy()
        {
            return UniTask.CompletedTask;
        }
        
        public override void OnGUI()
        {
            GUILayout.Label("资源页签内容");
        }
    }
}