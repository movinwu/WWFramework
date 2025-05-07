/*------------------------------
 * 脚本名称: DataTableConfig
 * 创建者: movin
 * 创建日期: 2025/05/04
------------------------------*/

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

#endif

        [Header("数据表byte保存路径")] public string dataTableBytePath;

        [Header("包含所有数据表名称"), ReadOnly] public string[] dataTableNames;
    }
}