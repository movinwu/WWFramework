/*------------------------------
 * 脚本名称: ExcelIntField
 * 创建者: movin
 * 创建日期: 2025/05/11
------------------------------*/

using System.Text;

namespace WWFramework
{
    /// <summary>
    /// excel int字段
    /// </summary>
    public class ExcelIntField : IExcelDataTableField
    {
        public string GetWhenIdBaseTypeName(string excelPath, string sheetName, string fieldName, string describe)
        {
            return ": Int32DataRow";
        }

        public void GenerateField(StringBuilder stringBuilder, string excelPath, string sheetName, string fieldName,  string describe)
        {
            stringBuilder.AppendLine("        /// <summary>");
            stringBuilder.Append("        /// ");
            stringBuilder.AppendLine(describe);
            stringBuilder.AppendLine("        /// </summary>");
            stringBuilder.Append("        public int ");
            stringBuilder.Append(fieldName);
            stringBuilder.AppendLine(";");
            stringBuilder.AppendLine();
        }

        public void GenerateDeserializeField(StringBuilder stringBuilder, string excelPath, string sheetName, string fieldName)
        {
            stringBuilder.Append("            ");
            stringBuilder.Append(fieldName);
            stringBuilder.AppendLine(" = buffer.ReadInt32();");
        }

        public void SerializeField(ByteBufferWriter writeBufferReader, string cellContent, string excelPath, string sheetName, int row, int col)
        {
            if (int.TryParse(cellContent.Trim(), out var value))
            {
                writeBufferReader.WriteInt32(value);
            }
            else
            {
                writeBufferReader.WriteInt32(0);
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
                    sb.Append("列转换异常,类型为int,但值为");
                    sb.Append(cellContent);
                },  ELogType.DataTable);
            }
        }
    }
}