/*------------------------------
 * 脚本名称: StackRange
 * 创建者: movin
 * 创建日期: 2025/02/23
 * 来源: https://github.com/dotnet-easy/easy-dotnet
------------------------------*/

using System.Collections.Generic;

namespace WWFramework
{
    /// <summary>
    /// stack拓展
    /// </summary>
    public static class StackEx
    {
        /// <summary>
        /// 将指定集合中的元素添加到栈的最上方。
        /// </summary>
        /// <typeparam name="T">栈元素类型</typeparam>
        /// <param name="stack">栈</param>
        /// <param name="collection">要添加到栈中的集合</param>
        public static void EnqueueRange<T>(this Stack<T> stack, IEnumerable<T> collection)
        {
            foreach (T item in collection)
            {
                stack.Push(item);
            }
        }
    }
}