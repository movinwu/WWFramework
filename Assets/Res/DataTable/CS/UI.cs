/*------------------------------
 * 脚本名称: UI
 * 描述: 代码自动生成,任何修改都将失效,类已声明为partial,可拓展
------------------------------*/

namespace WWFramework
{
    /// <summary>
    /// UI 数据表行
    /// </summary>
    public partial class UI : Int32DataRow
    {
        /// <summary>
        /// id
        /// </summary>
        public int id;

        /// <summary>
        /// 界面预制体
        /// </summary>
        public string panel_prefab;

        /// <summary>
        /// 是否暂停上层界面
        /// </summary>
        public bool pause_last_panel;


        public override void DeserializedFields(ByteBuffer buffer)
        {
            id = buffer.ReadInt();
            panel_prefab = buffer.ReadString();
            pause_last_panel = buffer.ReadBool();

        }
    }
}