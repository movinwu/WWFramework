/*------------------------------
 * 脚本名称: Example3
 * 描述: 代码自动生成,任何修改都将失效,类已声明为partial,可拓展
------------------------------*/

namespace WWFramework
{
    /// <summary>
    /// Example3 数据表行
    /// </summary>
    public partial class Example3 : Int32DataRow
    {
        /// <summary>
        /// id
        /// </summary>
        public int id;

        /// <summary>
        /// byte示例字段
        /// </summary>
        public byte byte_example;

        /// <summary>
        /// short示例字段
        /// </summary>
        public short short_example;

        /// <summary>
        /// int示例字段
        /// </summary>
        public int int_example;

        /// <summary>
        /// long示例字段
        /// </summary>
        public long long_example;

        /// <summary>
        /// string示例字段
        /// </summary>
        public string string_example;

        /// <summary>
        /// bool示例字段
        /// </summary>
        public bool bool_example;

        /// <summary>
        /// float示例字段
        /// </summary>
        public float float_example;

        /// <summary>
        /// double示例字段
        /// </summary>
        public double double_example;

        /// <summary>
        /// char示例字段
        /// </summary>
        public char char_example;

        /// <summary>
        /// 一维数组示例字段
        /// </summary>
        public int[] array_example;

        /// <summary>
        /// 二维数组示例字段
        /// </summary>
        public int[][] array_array_example;


        public override void DeserializedFields(ByteBuffer buffer)
        {
            id = buffer.ReadInt();
            byte_example = buffer.ReadByte();
            short_example = buffer.ReadShort();
            int_example = buffer.ReadInt();
            long_example = buffer.ReadLong();
            string_example = buffer.ReadString();
            bool_example = buffer.ReadBool();
            float_example = buffer.ReadFloat();
            double_example = buffer.ReadDouble();
            char_example = (char)buffer.ReadByte();
            var array_example_length = buffer.ReadInt();
            array_example = new int[array_example_length];
            for (int i = 0; i < array_example_length; i++)
            {
                array_example[i] = buffer.ReadInt();
            }
            var array_array_example_length0 = buffer.ReadInt();
            array_array_example = new int[array_array_example_length0][];
            for (int i = 0; i < array_array_example_length0; i++)
            {
                var array_array_example_length1 = buffer.ReadInt();
                array_array_example = new int[array_array_example_length1][];
                for (int j = 0; j < array_array_example_length1; j++)
                {
                    array_array_example[i][j] = buffer.ReadInt();
                }
            }

        }
    }
}