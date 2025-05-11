/*------------------------------
 * 脚本名称: DataTableTabContent
 * 创建者: movin
 * 创建日期: 2025/05/04
------------------------------*/

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;

namespace WWFramework
{
    /// <summary>
    /// 数据表页签内容
    /// </summary>
    public class DataTableTabContent : GlobalEditorTabContentBase
    {
        /// <summary>
        /// 找到的所有数据表
        /// </summary>
        private List<(string excel, string sheet)> _foundedDataTables = new List<(string excel, string sheet)>();
        
        /// <summary>
        /// 配置标脏
        /// </summary>
        private bool _isConfigDirty = true;
        
        public override UniTask OnInit()
        {
            return UniTask.CompletedTask;
        }

        public override UniTask OnRelease()
        {
            return UniTask.CompletedTask;
        }

        public override void OnDrawGUI()
        {
            var config = GameEntry.GlobalGameConfig.dataTableConfig;
            // 标脏时保存配置
            if (_isConfigDirty)
            {
                _isConfigDirty = false;
                EditorUtility.SetDirty(config);
                AssetDatabase.SaveAssetIfDirty(config);
                
                // 重新计算找到的数据表
            }
            // 绘制配置内容,可修改
            
        }
    }
}