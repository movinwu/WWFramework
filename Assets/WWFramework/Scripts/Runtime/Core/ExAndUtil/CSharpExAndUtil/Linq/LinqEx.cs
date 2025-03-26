/*------------------------------
 * 脚本名称: LinqExtension
 * 创建者: movin
 * 创建日期: 2025/02/16
------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace WWFramework
{
    /// <summary>
    /// linq语句拓展
    /// </summary>
    public static class LinqEx
    {
        #region Select

        public static List<TResult> SelectList<TSource, TResult>(
            this List<TSource> source,
            Func<TSource, TResult> selector)
        {
            if (ReferenceEquals(source, null))
            {
                throw new ArgumentNullException("source list is null");
            }

            if (ReferenceEquals(selector, null))
            {
                throw new ArgumentNullException("selector is null");
            }
            
            List<TResult> result = new List<TResult>(source.Count);
            
            source.ForEach(item => { result.Add(ReferenceEquals(item, null) ? default(TResult) : selector(item)); });

            return result;
        }
        
        public static List<TResult> SelectList<TSource, TResult>(
            this TSource[] source,
            Func<TSource, TResult> selector)
        {
            if (ReferenceEquals(source, null))
            {
                throw new ArgumentNullException("source array is null");
            }

            if (ReferenceEquals(selector, null))
            {
                throw new ArgumentNullException("selector is null");
            }
            
            List<TResult> result = new List<TResult>(source.Length);
            
            source.ForEach(item => { result.Add(ReferenceEquals(item, null) ? default(TResult) : selector(item)); });

            return result;
        }
        
        public static TResult[] SelectArray<TSource, TResult>(
            this List<TSource> source,
            Func<TSource, TResult> selector)
        {
            if (ReferenceEquals(source, null))
            {
                throw new ArgumentNullException("source list is null");
            }

            if (ReferenceEquals(selector, null))
            {
                throw new ArgumentNullException("selector is null");
            }
            
            TResult[] result = new TResult[source.Count];

            for (int i = 0; i < source.Count; i++)
            {
                var item = source[i];
                result[i] = ReferenceEquals(item, null) ? default(TResult) : selector(item);
            }

            return result;
        }
        
        public static TResult[] SelectArray<TSource, TResult>(
            this TSource[] source,
            Func<TSource, TResult> selector)
        {
            if (ReferenceEquals(source, null))
            {
                throw new ArgumentNullException("source array is null");
            }

            if (ReferenceEquals(selector, null))
            {
                throw new ArgumentNullException("selector is null");
            }
            
            TResult[] result = new TResult[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                var item = source[i];
                result[i] = ReferenceEquals(item, null) ? default(TResult) : selector(item);
            }

            return result;
        }

        #endregion Select

        #region Foreach
        
        public static void ForEach<TSource>(this TSource[] source, Action<TSource> action)
        {
            if (ReferenceEquals(source, null))
            {
                throw new ArgumentNullException("source array is null");
            }
        
            if (ReferenceEquals(action, null))
            {
                throw new ArgumentNullException("action is null");
            }
            
            for (int i = 0; i < source.Length; i++)
            {
                action(source[i]);
            }
        }
        
        /// <summary>
        /// 预先拷贝一遍数组再遍历,适用于对原始数组会进行操作的情况
        /// </summary>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <exception cref="ArgumentNullException"></exception>
        public static void CopyForEach<TSource>(this TSource[] source, Action<TSource> action)
        {
            if (ReferenceEquals(source, null))
            {
                throw new ArgumentNullException("source array is null");
            }

            if (ReferenceEquals(action, null))
            {
                throw new ArgumentNullException("action is null");
            }

            TSource[] newArray = source.ToArray();
            ForEach(newArray, action);
        }
        
        /// <summary>
        /// 预先拷贝一遍列表再遍历,适用于对原始列表会进行操作的情况
        /// </summary>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <exception cref="ArgumentNullException"></exception>
        public static void CopyForEach<TSource>(this List<TSource> source, Action<TSource> action)
        {
            if (ReferenceEquals(source, null))
            {
                throw new ArgumentNullException("source array is null");
            }

            if (ReferenceEquals(action, null))
            {
                throw new ArgumentNullException("action is null");
            }

            List<TSource> newList = source.ToList();
            newList.ForEach(action);
        }

        #endregion Foreach

        #region Where

        public static List<TSource> WhereList<TSource>(
            this List<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (ReferenceEquals(source, null))
            {
                throw new ArgumentNullException("source list is null");
            }

            if (ReferenceEquals(predicate, null))
            {
                throw new ArgumentNullException("predicate is null");
            }
            
            List<TSource> result = new List<TSource>(source.Count);
            
            source.ForEach(item =>
            {
                if (predicate(item))
                {
                    result.Add(item);
                }
            });

            return result;
        }
        
        public static List<TSource> WhereList<TSource>(
            this TSource[] source,
            Func<TSource, bool> predicate)
        {
            if (ReferenceEquals(source, null))
            {
                throw new ArgumentNullException("source array is null");
            }

            if (ReferenceEquals(predicate, null))
            {
                throw new ArgumentNullException("predicate is null");
            }
            
            List<TSource> result = new List<TSource>(source.Length);
            
            source.ForEach(item =>
            {
                if (predicate(item))
                {
                    result.Add(item);
                }
            });

            return result;
        }
        
        public static TSource[] WhereArray<TSource>(
            this List<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (ReferenceEquals(source, null))
            {
                throw new ArgumentNullException("source list is null");
            }

            if (ReferenceEquals(predicate, null))
            {
                throw new ArgumentNullException("predicate is null");
            }

            bool[] result = new bool[source.Count];

            int count = 0;
            for (int i = 0; i < source.Count; i++)
            {
                result[i] = predicate(source[i]);
                count++;
            }

            TSource[] resultArray = new TSource[count];
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i])
                {
                    resultArray[resultArray.Length - count] = source[i];
                    count--;
                }
            }

            return resultArray;
        }
        
        public static TSource[] WhereArray<TSource>(
            this TSource[] source,
            Func<TSource, bool> predicate)
        {
            if (ReferenceEquals(source, null))
            {
                throw new ArgumentNullException("source array is null");
            }

            if (ReferenceEquals(predicate, null))
            {
                throw new ArgumentNullException("predicate is null");
            }

            bool[] result = new bool[source.Length];

            int count = 0;
            for (int i = 0; i < source.Length; i++)
            {
                result[i] = predicate(source[i]);
                count++;
            }

            TSource[] resultArray = new TSource[count];
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i])
                {
                    resultArray[^count] = source[i];
                    count--;
                }
            }

            return resultArray;
        }

        #endregion Where

        #region Except

        /// <summary>
        /// 差集
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="enableSecondNull"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static List<TSource> ExceptList<TSource>(
            this List<TSource> first,
            List<TSource> second,
            bool enableSecondNull = false)
        {
            if (ReferenceEquals(first, null))
            {
                throw new ArgumentNullException("first list is null");
            }

            if (ReferenceEquals(second, null))
            {
                if (enableSecondNull)
                {
                    return first.ToList();
                }
                else
                {
                    throw new ArgumentNullException("second list is null");
                }
            }

            const int USE_SET_LIMIT_NUM = 4;

            if (first.Count <= USE_SET_LIMIT_NUM)
            {
                List<TSource> result = new List<TSource>(first.Count);

                for (int i = 0; i < first.Count; i++)
                {
                    if (!second.Contains(first[i]))
                    {
                        result.Add(first[i]);
                    }
                }

                return result;
            }
            else
            {
                HashSet<TSource> secondSet = new HashSet<TSource>(first);
                List<TSource> result = new List<TSource>(first.Count);
                for (int i = 0; i < second.Count; i++)
                {
                    secondSet.Add(second[i]);
                }

                for (int i = 0; i < first.Count; i++)
                {
                    if (!secondSet.Contains(first[i]))
                    {
                        result.Add(first[i]);
                    }
                }

                return result;
            }
        }

        #endregion Except
    }
}