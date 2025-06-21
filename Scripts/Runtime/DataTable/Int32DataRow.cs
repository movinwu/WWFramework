/*------------------------------
 * 脚本名称: Int32DataRow
 * 创建者: movin
 * 创建日期: 2025/05/11
------------------------------*/

namespace WWFramework
{
    /// <summary>
    /// int 为id的数据表行
    /// </summary>
    public abstract class Int32DataRow : IDataRow
    {
        public int ID { get; private set; }

        public void DeserializedId(ByteBufferReader bufferReader)
        {
            this.ID = bufferReader.ReadInt32();
        }

        /// <summary>
        /// 序列化字段
        /// </summary>
        /// <param name="bufferReader"></param>
        public abstract void DeserializedFields(ByteBufferReader bufferReader);
    }
}