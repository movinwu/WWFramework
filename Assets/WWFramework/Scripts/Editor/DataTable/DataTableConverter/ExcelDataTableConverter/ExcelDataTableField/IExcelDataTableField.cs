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
        /// 生成字段文本
        /// </summary>
        /// <param name="stringBuilder"></param>
        void GenerateField(StringBuilder stringBuilder);

        /// <summary>
        /// 生成反序列化字段文本
        /// </summary>
        /// <param name="stringBuilder"></param>
        void GenerateDeserializeField(StringBuilder stringBuilder);

        /// <summary>
        /// 序列化字段
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <param name="cellContent"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        void SerializeField(ByteBuffer writeBuffer, string cellContent, int row, int col);
    }
}