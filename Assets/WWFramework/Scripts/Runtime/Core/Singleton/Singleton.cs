/*------------------------------
 * 脚本名称: Singleton
 * 创建者: movin
 * 创建日期: 2025/03/18
------------------------------*/

using System;
using System.Linq;
using System.Reflection;

namespace WWFramework
{
    /// <summary>
    /// 普通单例基类
    /// </summary>
    /// <typeparam name="T">单例类型</typeparam>
    public class Singleton<T> where T : class
    {
        private static T _instance;
        private static readonly object _lock = new object();

        /// <summary>
        /// 获取单例实例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            var constructors =
                                typeof(T).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                                    .WhereArray(c => c.GetParameters().Length == 0);

                            if (constructors.Length == 1)
                            {
                                _instance = (T)constructors[0].Invoke(null);
                            }
                        }
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// 销毁单例实例
        /// </summary>
        public static void DestroyInstance()
        {
            _instance = null;
        }
    }
}