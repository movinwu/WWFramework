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
    public class Singleton<T> where T : class, new()
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
                                typeof(T).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
                            var parameterlessCtors = constructors.Where(c => c.GetParameters().Length == 0).ToList();

                            // 检查是否存在其他构造函数
                            if (constructors.Length > parameterlessCtors.Count)
                            {
                                Log.LogWarning(sb =>
                                {
                                    sb.Append("[Singleton] ");
                                    sb.Append(typeof(T).Name);
                                    sb.Append(
                                        " has multiple constructors, only parameterless private constructor will be used");
                                });
                            }

                            // 检查无参构造函数
                            if (parameterlessCtors.Count == 0)
                            {
                                throw new InvalidOperationException(
                                    $"[Singleton] {typeof(T)} must have a private parameterless constructor");
                            }

                            if (parameterlessCtors.Count > 1)
                            {
                                throw new InvalidOperationException(
                                    $"[Singleton] {typeof(T)} has multiple parameterless constructors");
                            }

                            _instance = (T)parameterlessCtors[0].Invoke(null);
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