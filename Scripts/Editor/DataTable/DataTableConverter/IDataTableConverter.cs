/*------------------------------
 * 脚本名称: IDataTableConverter
 * 创建者: movin
 * 创建日期: 2025/05/06
------------------------------*/

using Cysharp.Threading.Tasks;

namespace WWFramework
{
    /// <summary>
    /// 数据表导表器接口
    /// </summary>
    public interface IDataTableConverter
    {
        /// <summary>
        /// 全部表格导表
        /// </summary>
        /// <returns></returns>
        UniTask ConvertAll();

        /// <summary>
        /// 单个表格导表
        /// </summary>
        /// <param name="filePath">表格文件路径</param>
        /// <param name="name">表格名称</param>
        /// <param name="savePath">保存路径</param>
        /// <returns></returns>
        UniTask Convert(string filePath, string name, string savePath);
    }
}