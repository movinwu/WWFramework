/*------------------------------
 * 脚本名称: ListEx
 * 创建者: movin
 * 创建日期: 2025/02/23
 * 来源: https://github.com/dotnet-easy/easy-dotnet
------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace WWFramework
{
    /// <summary>
    /// list拓展
    /// </summary>
    public static class ListEx
    {
        /// <summary>
        /// 判断两个列表是否相等。
        /// </summary>
        /// <typeparam name="T">列表元素类型</typeparam>
        /// <param name="list1">要比较的第一个列表</param>
        /// <param name="list2">要比较的第二个列表</param>
        /// <returns>如果两个列表相等，则返回 true；否则返回 false</returns>
        public static bool Equals<T>(this List<T> list1, List<T> list2)
        {
            if (list1 == null && list2 == null)
            {
                return true;
            }
            else if (list1 == null || list2 == null)
            {
                return false;
            }
            else if (list1.Count != list2.Count)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    if (!list1[i].Equals(list2[i]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }
        
        /// <summary>
        /// 对列表排序
        /// </summary>
        /// <param name="list">要排序的列表</param>
        /// <param name="comparisons">排序的委托</param>
        /// <typeparam name="T">列表元素类型</typeparam>
        /// <exception cref="ArgumentNullException">如果列表为 null</exception>
        /// <exception cref="ArgumentException">如果比较委托为空或包含 null 值</exception>
        public static void SortBy<T>(this List<T> list, Comparison<T>[] comparisons)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (comparisons == null || comparisons.Length == 0)
            {
                throw new ArgumentException("至少需要提供一个比较委托。");
            }

            if (comparisons.Any(x => x == null))
            {
                throw new ArgumentException("比较委托不能包含 null 值。");
            }

            // 使用多个 Comparison<T> 委托进行排序
            list.Sort((l1, l2) =>
            {
                for (int i = 0; i < comparisons.Length; i++)
                {
                    var comparison = comparisons[i];
                    int result = comparison(l1, l2);
                    if (result != 0) // 如果当前比较条件已经能确定顺序，直接返回结果
                    {
                        return result;
                    }
                }
                return 0; // 如果所有比较条件都相等，则返回 0
            });
        }

        /// <summary>
        /// 交换两个索引位置的元素
        /// </summary>
        /// <param name="list">列表</param>
        /// <param name="index1">下标1</param>
        /// <param name="index2">下标2</param>
        /// <typeparam name="T">列表元素类型</typeparam>
        public static void Swap<T>(this List<T> list, int index1, int index2)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (index1 < 0 || index1 >= list.Count || index2 < 0 || index2 >= list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index1), $"要交换的索引 {index1} 和 {index2} 超出范围, 列表长度为 {list.Count}");
            }

            if (index1 != index2)
            {
                (list[index1], list[index2]) = (list[index2], list[index1]);
            }
        }
    }
}