/*------------------------------
 * 脚本名称: ExcelIntArrayField
 * 创建者: movin
 * 创建日期: 2025/05/11
------------------------------*/

using System.Text;

namespace WWFramework
{
    /// <summary>
    /// excel int数组字段
    /// </summary>
    public class ExcelIntArrayField : IExcelDataTableField
    {
        public string GetWhenIdBaseTypeName(string excelPath, string sheetName, string fieldName, string describe)
        {
            Log.LogError(sb =>
            {
                sb.Append("数据表");
                sb.Append(excelPath);
                sb.Append(":");
                sb.Append(sheetName);
                sb.Append("不能使用int数组字段作为id");
            },  ELogType.DataTable);
            return string.Empty;
        }

        public void GenerateField(StringBuilder stringBuilder, string excelPath, string sheetName, string fieldName,  string describe)
        {
            stringBuilder.AppendLine("        /// <summary>");
            stringBuilder.Append("        /// ");
            stringBuilder.AppendLine(describe);
            stringBuilder.AppendLine("        /// </summary>");
            stringBuilder.Append("        public int[] ");
            stringBuilder.Append(fieldName);
            stringBuilder.AppendLine(";");
            stringBuilder.AppendLine();
        }

        public void GenerateDeserializeField(StringBuilder stringBuilder, string excelPath, string sheetName, string fieldName)
        {
            stringBuilder.Append("            var ");
            stringBuilder.Append(fieldName);
            stringBuilder.AppendLine("_length = buffer.ReadInt();");
            stringBuilder.Append("            ");
            stringBuilder.Append(fieldName);
            stringBuilder.Append(" = new int[");
            stringBuilder.Append(fieldName);
            stringBuilder.AppendLine("_length];");
            stringBuilder.Append("            for (int i = 0; i < ");
            stringBuilder.Append(fieldName);
            stringBuilder.AppendLine("_length; i++)");
            stringBuilder.AppendLine("            {");
            stringBuilder.Append("                ");
            stringBuilder.Append(fieldName);
            stringBuilder.AppendLine("[i] = buffer.ReadInt();");
            stringBuilder.AppendLine("            }");
        }

        public void SerializeField(ByteBuffer writeBuffer, string cellContent, string excelPath, string sheetName, int row, int col)
        {
            var values = cellContent.Split(GameEntry.GlobalGameConfig.dataTableConfig.arraySplitChars[0]);
            var intArray = new int[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                if (int.TryParse(values[i].Trim(), out var value))
                {
                    intArray[i] = value;
                }
                else
                {
                    intArray[i] = 0;
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
                        sb.Append("列转换异常,类型为int数组,但值为");
                        sb.Append(cellContent);
                    },  ELogType.DataTable);
                    return;
                }
            }
            writeBuffer.WriteInt(intArray.Length);
            for (int i = 0; i < intArray.Length; i++)
            {
                writeBuffer.WriteInt(intArray[i]);
            }
        }
    }
}