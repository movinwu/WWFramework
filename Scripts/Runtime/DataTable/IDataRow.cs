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
        /// <param name="buffer"></param>
        void DeserializedId(ByteBuffer buffer);

        /// <summary>
        /// 反序列化字段
        /// </summary>
        /// <param name="buffer"></param>
        void DeserializedFields(ByteBuffer buffer);
    }
}