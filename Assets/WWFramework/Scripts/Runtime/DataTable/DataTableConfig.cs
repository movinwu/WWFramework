/*------------------------------
 * 脚本名称: DataTableConfig
 * 创建者: movin
 * 创建日期: 2025/05/04
------------------------------*/

using System.Collections.Generic;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 数据表配置
    /// </summary>
    public class DataTableConfig : ScriptableObject
    {
#if UNITY_EDITOR

        [Header("excel路径")] public string dataTableExcelPath;
        
        [Header("模板路径")] public string dataTableTemplatePath;
        
        [Header("cs路径")] public string dataTableCsPath;

#endif

        [Header("byte路径")] public string dataTableBytePath;

        [Header("所有数据表名称"), ReadOnly] public List<string> dataTableNames;

        [Header("数组分割字符串")] public char[] arraySplitChars;
    }
}