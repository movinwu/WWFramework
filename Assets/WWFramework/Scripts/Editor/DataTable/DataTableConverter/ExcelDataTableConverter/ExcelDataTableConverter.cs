/*------------------------------
 * 脚本名称: ExcelDataTableConverter
 * 创建者: movin
 * 创建日期: 2025/05/06
------------------------------*/

using System;
using System.Collections.Generic;
using System.IO;
using Codice.CM.Common.Serialization.Replication;
using Cysharp.Threading.Tasks;
using OfficeOpenXml;
using UnityEditor;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// excel数据表转换器
    /// </summary>
    public class ExcelDataTableConverter : IDataTableConverter
    {
        /// <summary>
        /// 需要导出的sheet名称前缀
        /// </summary>
        private const string SheetNamePrefix = "D_";

        /// <summary>
        /// 字节数组
        /// </summary>
        private ByteBuffer _buffer = new ByteBuffer();
        
        public async UniTask ConvertAll()
        {
            // 读取配置
            var config = GameEntry.GlobalGameConfig.dataTableConfig;
            // excel路径
            var excelPath = config.dataTableExcelPath;
            // 拼接得到绝对路径
            var filePath = Path.Combine(Application.dataPath, "..", excelPath).Replace('\\', '/');
            // 得到所有文件路径
            var xlsFiles = Directory.GetFiles(filePath, "*.xls");
            var xlsxFiles = Directory.GetFiles(filePath, "*.xlsx");
            var files = xlsFiles.ConcatArray(xlsxFiles, null);
            files = files.SelectArray(x => x.Replace('\\', '/'));
            
            // 使用EPPlus遍历打开excel,得到所有sheet名称,将所有以指定前缀开头的sheet加入字典中
            var sheetNamesDic = new Dictionary<string, string>();
            files.ForEach(x =>
            { 
                using (var package = new ExcelPackage(new FileInfo(x)))
                {
                    foreach (var sheet in package.Workbook.Worksheets)
                    {
                        if (sheet.Name.StartsWith(SheetNamePrefix))
                        {
                            var sheetName = sheet.Name.Replace(SheetNamePrefix, "");
                            if (sheetNamesDic.TryGetValue(sheetName, out var path))
                            {
                                Log.LogError(sb =>
                                {
                                    sb.Append(path);
                                    sb.Append(" 和 ");
                                    sb.Append(x);
                                    sb.Append("中都有名为 ");
                                    sb.Append(sheetName);
                                    sb.Append(" 的sheet,请检查!");
                                }, ELogType.DataTable);
                            }
                            else
                            {
                                sheetNamesDic.Add(sheetName, x);
                            }
                        }
                    }
                }
            });
            
            // 输出文件夹
            var savePath = config.dataTableBytePath;
            var outputPath = Path.Combine(Application.dataPath, "..", savePath).Replace('\\', '/');
            // 创建空文件夹
            if (Directory.Exists(outputPath))
            {
                Directory.Delete(outputPath, true);
            }
            Directory.CreateDirectory(outputPath);
            
            // TODO 所有要导出的sheet名称和excel路径已经保存在sheetNamesDic中,这里可以进行剔除特定sheet等操作后再导出
            
            // 遍历字典,调用导出函数导出
            float progress = 0;
            foreach (var item in sheetNamesDic)
            {
                var byteSavePath = Path.Combine(outputPath, $"{item.Key}.bytes").Replace('\\', '/');
                // 展示进度条
                EditorUtility.DisplayProgressBar("正在导出数据表", $"正在导出{item.Key}数据表", progress / sheetNamesDic.Count);
                await Convert(item.Value, item.Key, byteSavePath);
                progress++;
            }
        }

        public async UniTask Convert(string filePath, string name, string savePath)
        {
            try
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var sheet = package.Workbook.Worksheets[SheetNamePrefix + name];
                    if (sheet == null)
                    {
                        Log.LogError(sb =>
                        {
                            sb.Append("数据表转换失败: ");
                            sb.Append(filePath);
                            sb.Append(" 中不存在名为 ");
                            sb.Append(SheetNamePrefix + name);
                            sb.Append(" 的sheet!");
                        }, ELogType.DataTable);
                        return;
                    }
                    
                    // 读取第一行,一直读取到空格或结尾,确定行数
                    var columnCount = 1;   // 默认第一列留空,作为备注
                    while (true)
                    {
                        var cell = sheet.Cells[1, columnCount + 1];
                        if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString().Trim()))
                        {
                            break;
                        }
                        columnCount++;
                    }
                    // 读取第2列,一直读取到空格或结尾,确定列数
                    var rowCount = 0;
                    while (true)
                    {
                        var cell = sheet.Cells[rowCount + 1, 2];
                        if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString().Trim()))
                        {
                            break;
                        }
                        rowCount++;
                    }
                    
                    // 去除前4行标题部分和第一列备注
                    rowCount -= 4;
                    columnCount -= 1;
                    
                    // 检验内容长度
                    if (rowCount < 0 || columnCount < 0)
                    {
                        Log.LogError(sb =>
                        {
                            sb.Append("数据表转换失败: ");
                            sb.Append(filePath);
                            sb.Append(" 中名为 ");
                            sb.Append(SheetNamePrefix + name);
                            sb.Append(" 的sheet行数或列数不足4行或1列!");
                        }, ELogType.DataTable);
                        return;
                    }
                    
                    // 生成数据表字段列表
                    var fields = new List<IExcelDataTableField>();
                    for (var i = 1; i < columnCount; i++)
                    {
                        // 读取字段平台
                        var csCellValue = ReadSheetCell(sheet, 3, i);
                        if (csCellValue.Contains('c'))
                        {
                            // 读取字段描述
                            var describeCellValue = ReadSheetCell(sheet, 1, i);
                            // 读取字段类型
                            var typeCellValue = ReadSheetCell(sheet, 2, i);
                            // 读取字段名称
                            var fieldName = ReadSheetCell(sheet, 4, i);
                            // 获取字段
                            var field = DataTableHelper.CreateExcelDataTableField(fieldName, typeCellValue, describeCellValue);
                            
                            fields.Add(field);
                        }
                    }
                    
                    // 写入数据长度
                    _buffer.WritePointer = 0;
                    _buffer.WriteInt(rowCount);
                    // 记录写入数据长度的写入指针位置
                    var pointer = rowCount * 4 + 4;
                    // 循环遍历,逐行写入
                    for (var i = 0; i < rowCount; i++)
                    {
                        _buffer.WritePointer = pointer;
                        for (var j = 0; j < fields.Count; j++)
                        {
                            if (null != fields[j])
                            {
                                fields[j].SerializeField(_buffer, ReadSheetCell(sheet, i + 5, j + 1),  i + 5, j + 1);
                            }
                        }
                        // 写入完成后,记录写入指针位置,计算写入长度,最后将数据长度写入到数据表头部
                        var newPointer = _buffer.WritePointer;
                        _buffer.WritePointer = 4 + i * 4;
                        _buffer.WriteInt(newPointer - pointer);
                        pointer = newPointer;
                        _buffer.WritePointer = newPointer;
                    }
                    // 所有数据写入完成,写入文件
                    _buffer.ReadPointer = 0;
                    File.WriteAllBytes(savePath, _buffer.CopyUnreadBytes());
                }
            }
            catch (Exception ex)
            {
                Log.LogError(sb =>
                {
                    sb.Append("数据表转换失败: ");
                    sb.Append(ex.Message);
                    sb.Append("\n");
                    sb.Append(ex.StackTrace);
                },  ELogType.DataTable);
            }
            
            // 清理进度条
            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// 读取单个单元格
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private string ReadSheetCell(ExcelWorksheet sheet, int row, int col)
        {
            var cell = sheet.Cells[row, col];
            if (null == cell)
            {
                return string.Empty;
            }

            if (null == cell.Value)
            {
                return string.Empty;
            }

            return cell.Value.ToString();
        }
    }
}