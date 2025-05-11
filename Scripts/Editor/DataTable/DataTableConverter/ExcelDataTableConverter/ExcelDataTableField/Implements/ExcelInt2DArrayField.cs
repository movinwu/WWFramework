/*------------------------------
 * 脚本名称: ExcelInt2DArrayField
 * 创建者: movin
 * 创建日期: 2025/05/11
------------------------------*/

using System.Text;

namespace WWFramework
{
    /// <summary>
    /// excel int二维数组字段
    /// </summary>
    public class ExcelInt2DArrayField : IExcelDataTableField
    {
        public string GetWhenIdBaseTypeName(string excelPath, string sheetName, string fieldName, string describe)
        {
            Log.LogError(sb =>
            {
                sb.Append("数据表");
                sb.Append(excelPath);
                sb.Append(":");
                sb.Append(sheetName);
                sb.Append("不能使用int二维数组字段作为id");
            },  ELogType.DataTable);
            return string.Empty;
        }

        public void GenerateField(StringBuilder stringBuilder, string excelPath, string sheetName, string fieldName,  string describe)
        {
            stringBuilder.AppendLine("        /// <summary>");
            stringBuilder.Append("        /// ");
            stringBuilder.AppendLine(describe);
            stringBuilder.AppendLine("        /// </summary>");
            stringBuilder.Append("        public int[][] ");
            stringBuilder.Append(fieldName);
            stringBuilder.AppendLine(";");
            stringBuilder.AppendLine();
        }

        public void GenerateDeserializeField(StringBuilder stringBuilder, string excelPath, string sheetName, string fieldName)
        {
            stringBuilder.Append("            var ");
            stringBuilder.Append(fieldName);
            stringBuilder.AppendLine("_length0 = buffer.ReadInt();");
            stringBuilder.Append("            ");
            stringBuilder.Append(fieldName);
            stringBuilder.Append(" = new int[");
            stringBuilder.Append(fieldName);
            stringBuilder.AppendLine("_length0][];");
            stringBuilder.Append("            for (int i = 0; i < ");
            stringBuilder.Append(fieldName);
            stringBuilder.AppendLine("_length0; i++)");
            stringBuilder.AppendLine("            {");
            
            stringBuilder.Append("                var ");
            stringBuilder.Append(fieldName);
            stringBuilder.AppendLine("_length1 = buffer.ReadInt();");
            stringBuilder.Append("                ");
            stringBuilder.Append(fieldName);
            stringBuilder.Append(" = new int[");
            stringBuilder.Append(fieldName);
            stringBuilder.AppendLine("_length1][];");
            stringBuilder.Append("                for (int j = 0; j < ");
            stringBuilder.Append(fieldName);
            stringBuilder.AppendLine("_length1; j++)");
            stringBuilder.AppendLine("                {");
            stringBuilder.Append("                    ");
            stringBuilder.Append(fieldName);
            stringBuilder.AppendLine("[i][j] = buffer.ReadInt();");
            stringBuilder.AppendLine("                }");
            
            stringBuilder.AppendLine("            }");
        }

        public void SerializeField(ByteBuffer writeBuffer, string cellContent, string excelPath, string sheetName, int row, int col)
        {
            var rows = cellContent.Split(GameEntry.GlobalGameConfig.dataTableConfig.arraySplitChars[1]);
            var int2DArray = new int[rows.Length][];
            for (int i = 0; i < rows.Length; i++)
            {
                var values = rows[i].Split(GameEntry.GlobalGameConfig.dataTableConfig.arraySplitChars[0]);
                int2DArray[i] = new int[values.Length];
                for (int j = 0; j < values.Length; j++)
                {
                    if (int.TryParse(values[j].Trim(), out var value))
                    {
                        int2DArray[i][j] = value;
                    }
                    else
                    {
                        int2DArray[i][j] = 0;
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
                            sb.Append("列转换异常,类型为int二维数组,但值为");
                            sb.Append(cellContent);
                        },  ELogType.DataTable);
                        return;
                    }
                }
            }
            writeBuffer.WriteInt(int2DArray.Length);
            for (int i = 0; i < int2DArray.Length; i++)
            {
                writeBuffer.WriteInt(int2DArray[i].Length);
                for (int j = 0; j < int2DArray[i].Length; j++)
                {
                    writeBuffer.WriteInt(int2DArray[i][j]);
                }
            }
        }
    }
}