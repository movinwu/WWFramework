/*------------------------------
 * 脚本名称: IClone
 * 创建者: movin
 * 创建日期: 2025/02/16
------------------------------*/

namespace WWFramework
{
    /// <summary>
    /// 克隆接口
    /// </summary>
    public interface IClone
    {
        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T DeepClone<T>(T obj) where T : IClone;

        /// <summary>
        /// 浅拷贝
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T MemberwiseClone<T>(T obj) where T : IClone;
    }
}