/*------------------------------
 * 脚本名称: IExcelDataTableField
 * 创建者: movin
 * 创建日期: 2025/05/04
------------------------------*/

using System.IO;
using System.Text;

namespace WWFramework
{
    /// <summary>
    /// excel数据表属性字段接口
    /// </summary>
    public interface IExcelDataTableField
    {
        /// <summary>
        /// 当作为id字段时,对应的数据表基类类型名
        /// </summary>
        /// <param name="excelPath"></param>
        /// <param name="sheetName"></param>
        /// <param name="fieldName"></param>
        /// <param name="describe"></param>
        /// <returns></returns>
        string GetWhenIdBaseTypeName(string excelPath, string sheetName, string fieldName,  string describe);
        
        /// <summary>
        /// 生成字段文本
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="excelPath"></param>
        /// <param name="sheetName"></param>
        /// <param name="fieldName"></param>
        /// <param name="describe"></param>
        void GenerateField(StringBuilder stringBuilder, string excelPath, string sheetName, string fieldName, string describe);

        /// <summary>
        /// 生成反序列化字段文本
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="excelPath"></param>
        /// <param name="sheetName"></param>
        /// <param name="fieldName"></param>
        void GenerateDeserializeField(StringBuilder stringBuilder, string excelPath, string sheetName, string fieldName);

        /// <summary>
        /// 序列化字段
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <param name="cellContent"></param>
        /// <param name="excelPath"></param>
        /// <param name="sheetName"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        void SerializeField(ByteBuffer writeBuffer, string cellContent, string excelPath, string sheetName, int row, int col);
    }
}