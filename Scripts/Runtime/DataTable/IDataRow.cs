/*------------------------------
 * 脚本名称: IDataRow
 * 创建者: movin
 * 创建日期: 2025/05/11
------------------------------*/

using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 数据表一行数据接口
    /// </summary>
    public interface IDataRow
    {
        /// <summary>
        /// 反序列化id
        /// </summary>
        /// <param name="bufferReader"></param>
        void DeserializedId(ByteBufferReader bufferReader);

        /// <summary>
        /// 反序列化字段
        /// </summary>
        /// <param name="bufferReader"></param>
        void DeserializedFields(ByteBufferReader bufferReader);
    }
}