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
        public static IExcelDataTableField CreateExcelDataTableField(string fieldType)
        {
            switch (fieldType)
            {
                case "byte":
                    return new ExcelByteField();
                case "short":
                    return new ExcelShortField();
                case "int":
                    return new ExcelIntField();
                case "long":
                    return new ExcelLongField();
                case "string":
                    return new ExcelStringField();
                case "bool":
                    return new ExcelBoolField();
                case "float":
                    return new ExcelFloatField();
                case "double":
                    return new ExcelDoubleField();
                case "char":
                    return new ExcelCharField();
                case "int[]":
                    return new ExcelIntArrayField();
                case "int[][]":
                    return new ExcelInt2DArrayField();
                default:
                    return null;
            }
        }
    }
}