/*------------------------------
 * 脚本名称: ExcelBoolField
 * 创建者: movin
 * 创建日期: 2025/05/11
------------------------------*/

using System.Text;

namespace WWFramework
{
    /// <summary>
    /// excel bool字段
    /// </summary>
    public class ExcelBoolField : IExcelDataTableField
    {
        public string GetWhenIdBaseTypeName(string excelPath, string sheetName, string fieldName, string describe)
        {
            Log.LogError(sb =>
            {
                sb.Append("数据表");
                sb.Append(excelPath);
                sb.Append(":");
                sb.Append(sheetName);
                sb.Append("不能使用bool字段作为id");
            },  ELogType.DataTable);
            return string.Empty;
        }

        public void GenerateField(StringBuilder stringBuilder, string excelPath, string sheetName, string fieldName,  string describe)
        {
            stringBuilder.AppendLine("        /// <summary>");
            stringBuilder.Append("        /// ");
            stringBuilder.AppendLine(describe);
            stringBuilder.AppendLine("        /// </summary>");
            stringBuilder.Append("        public bool ");
            stringBuilder.Append(fieldName);
            stringBuilder.AppendLine(";");
            stringBuilder.AppendLine();
        }

        public void GenerateDeserializeField(StringBuilder stringBuilder, string excelPath, string sheetName, string fieldName)
        {
            stringBuilder.Append("            ");
            stringBuilder.Append(fieldName);
            stringBuilder.AppendLine(" = buffer.ReadBool();");
        }

        public void SerializeField(ByteBuffer writeBuffer, string cellContent, string excelPath, string sheetName, int row, int col)
        {
            if (bool.TryParse(cellContent.Trim(), out var value))
            {
                writeBuffer.WriteBool(value);
            }
            else
            {
                writeBuffer.WriteBool(false);
                Log.LogError(sb =>
                {
                    sb.Append("数据表");
                    sb.Append(excelPath);
                    sb.Append(":");
                    sb.Append(sheetName);
                    sb.Append("第");
                    sb.Append(row);
                    sb.Append("行,第");
                    sb.Append(col);
                    sb.Append("列转换异常,类型为bool,但值为");
                    sb.Append(cellContent);
                },  ELogType.DataTable);
            }
        }
    }
}