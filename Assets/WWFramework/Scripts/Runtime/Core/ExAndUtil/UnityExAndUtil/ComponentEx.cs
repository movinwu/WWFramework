/*------------------------------
 * 脚本名称: ComponentEx
 * 创建者: movin
 * 创建日期: 2025/03/30
------------------------------*/

using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 组件扩展
    /// </summary>
    public static class ComponentEx
    {
        /// <summary>
        /// 获取或添加组件
        /// </summary>
        /// <param name="component"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            if (null == component)
            {
                return null;
            }

            var t = component.GetComponent<T>();
            if (null == t)
            {
                t = component.gameObject.AddComponent<T>();
            }

            return t;
        }
    }
}