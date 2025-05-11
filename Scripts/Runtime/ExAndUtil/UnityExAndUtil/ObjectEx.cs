/*------------------------------
 * 脚本名称: ObjectEx
 * 创建者: movin
 * 创建日期: 2025/03/30
------------------------------*/

using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// Unity的Object拓展
    /// </summary>
    public static class ObjectEx
    {
        /// <summary>
        /// 安全销毁对象
        /// </summary>
        /// <param name="obj"></param>
        public static void SafeDestroy(this Object obj)
        {
            if (null == obj)
            {
                return;
            }
            
            if (Application.isPlaying)
            {
                Object.Destroy(obj);
            }
            else
            {
                Object.DestroyImmediate(obj);
            }
        }
        
        /// <summary>
        /// 获取或添加组件
        /// </summary>
        /// <param name="gameObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            if (null == gameObject)
            {
                return null;
            }

            var t = gameObject.GetComponent<T>();
            if (null == t)
            {
                t = gameObject.AddComponent<T>();
            }

            return t;
        }
    }
}