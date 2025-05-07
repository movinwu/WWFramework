/*------------------------------
 * 脚本名称: DataTableHelper
 * 创建者: movin
 * 创建日期: 2025/05/06
------------------------------*/

namespace WWFramework
{
    /// <summary>
    /// 数据表辅助类
    /// </summary>
    public class DataTableHelper
    {
        /// <summary>
        /// 获取新的数据表转换器对象
        /// </summary>
        public static IDataTableConverter GetNewDataTableConverter()
        {
            return new ExcelDataTableConverter();
        }

        /// <summary>
        /// 创建Excel数据表字段
        /// </summary>
        public static IExcelDataTableField CreateExcelDataTableField(string fieldName, string fieldType,
            string fieldDescribe)
        {
            switch (fieldType)
            {
                default:
                    return null;
            }
        }
    }
}