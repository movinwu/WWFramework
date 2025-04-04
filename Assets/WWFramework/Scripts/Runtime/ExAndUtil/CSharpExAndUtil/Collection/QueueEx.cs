/*------------------------------
 * 脚本名称: QueueEx
 * 创建者: movin
 * 创建日期: 2025/02/23
 * 来源: https://github.com/dotnet-easy/easy-dotnet
------------------------------*/

using System.Collections.Generic;

namespace WWFramework
{
    /// <summary>
    /// queue拓展
    /// </summary>
    public static class QueueEx
    {
        /// <summary>
        /// 将指定集合中的元素添加到队列的末尾。
        /// </summary>
        /// <typeparam name="T">队列元素类型</typeparam>
        /// <param name="queue">队列</param>
        /// <param name="collection">要添加到队列中的集合</param>
        public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> collection)
        {
            foreach (T item in collection)
            {
                queue.Enqueue(item);
            }
        }
    }
}