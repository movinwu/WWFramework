/*------------------------------
 * 脚本名称: DataTableConvertEditor
 * 创建者: movin
 * 创建日期: 2025/05/06
------------------------------*/

using Cysharp.Threading.Tasks;
using UnityEditor;

namespace WWFramework
{
    /// <summary>
    /// 数据表导表
    /// </summary>
    public class DataTableConvertEditor
    {
        private static IDataTableConverter  _iDataTableConverter;
        
        /// <summary>
        /// 数据表导表器
        /// </summary>
        public static IDataTableConverter DataTableConverter => _iDataTableConverter ??= DataTableHelper.GetNewDataTableConverter();

        [MenuItem(GlobalEditorStringDefine.DataTableConvertEditor, priority = GlobalEditorPriorityDefine.DataTableConvertEditor)]
        private static void ConvertAll()
        {
            DataTableConverter.ConvertAll().Forget();
        }
    }
}