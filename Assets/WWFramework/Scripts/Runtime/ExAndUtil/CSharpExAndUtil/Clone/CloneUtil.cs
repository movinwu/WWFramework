/*------------------------------
 * 脚本名称: CloneUtil
 * 创建者: movin
 * 创建日期: 2025/02/16
------------------------------*/

namespace WWFramework
{
    /// <summary>
    /// clone工具类
    /// </summary>
    public static class CloneUtil
    {
        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T DeepClone<T>(T obj) where T : IClone
        {
            // 如果对象为 null，则返回 null
            if (ReferenceEquals(obj, null))
            {
                return default(T);
            }
            
            return obj.DeepClone(obj);
        }

        /// <summary>
        /// 浅拷贝
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T MemberwiseClone<T>(T obj) where T : IClone
        {
            // 如果对象为 null，则返回 null
            if (ReferenceEquals(obj, null))
            {
                return default(T);
            }
            
            return obj.MemberwiseClone(obj);
        }
    }
}