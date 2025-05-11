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
            Func<TSource, TResult> selector,
            Func<TSource, TResult, bool> predicate = null)
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

            if (null == predicate)
            {
                source.ForEach(item => { result.Add(ReferenceEquals(item, null) ? default(TResult) : selector(item)); });
            }
            else
            {
                source.ForEach(item =>
                {
                    var tResult = ReferenceEquals(item, null) ? default(TResult) : selector(item);
                    if (predicate(item, tResult))
                    {
                        result.Add(tResult); 
                    }
                });
            }

            return result;
        }
        
        public static List<TResult> SelectList<TSource, TResult>(
            this TSource[] source,
            Func<TSource, TResult> selector,
            Func<TSource, TResult, bool> predicate = null)
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

            if (null == predicate)
            {
                source.ForEach(item => { result.Add(ReferenceEquals(item, null) ? default(TResult) : selector(item)); });
            }
            else
            {
                source.ForEach(item =>
                {
                    var tResult = ReferenceEquals(item, null) ? default(TResult) : selector(item);
                    if (predicate(item, tResult))
                    {
                        result.Add(tResult); 
                    }
                });
            }

            return result;
        }
        
        public static TResult[] SelectArray<TSource, TResult>(
            this List<TSource> source,
            Func<TSource, TResult> selector,
            Func<TSource, TResult, bool> predicate = null)
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

            if (null != predicate)
            {
                for (int i = 0; i < source.Count; i++)
                {
                    var item = source[i];
                    result[i] = ReferenceEquals(item, null) ? default(TResult) : selector(item);
                }

                int count = 0;
                bool[] predicateResult = new bool[source.Count];
                for (int i = 0; i < source.Count; i++)
                {
                    predicateResult[i] = predicate(source[i], result[i]);
                    if (predicateResult[i])
                    {
                        count++;
                    }
                }

                var oldResult = result;
                result = new TResult[count];
                count = 0;
                for (int i = 0; i < source.Count; i++)
                {
                    if (predicateResult[i])
                    {
                        result[count++] = oldResult[i];
                    }
                }
            }
            else
            {
                int count = 0;
                for (int i = 0; i < source.Count; i++)
                {
                    var item = source[i];
                    if (!ReferenceEquals(item, null))
                    {
                        var resultItem = selector(item);
                        if (!ReferenceEquals(resultItem, null))
                        {
                            result[count++] = resultItem;
                        }
                    }
                }

                var oldResult = result;
                result = new TResult[count];
                count = 0;
                for (int i = 0; i < result.Length; i++)
                {
                    result[count++] = oldResult[i];
                }
            }

            return result;
        }
        
        public static TResult[] SelectArray<TSource, TResult>(
            this TSource[] source,
            Func<TSource, TResult> selector,
            Func<TSource, TResult, bool> predicate = null)
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

            if (null != predicate)
            {
                for (int i = 0; i < source.Length; i++)
                {
                    var item = source[i];
                    result[i] = ReferenceEquals(item, null) ? default(TResult) : selector(item);
                }

                int count = 0;
                bool[] predicateResult = new bool[source.Length];
                for (int i = 0; i < source.Length; i++)
                {
                    predicateResult[i] = predicate(source[i], result[i]);
                    if (predicateResult[i])
                    {
                        count++;
                    }
                }

                var oldResult = result;
                result = new TResult[count];
                count = 0;
                for (int i = 0; i < source.Length; i++)
                {
                    if (predicateResult[i])
                    {
                        result[count++] = oldResult[i];
                    }
                }
            }
            else
            {
                int count = 0;
                for (int i = 0; i < source.Length; i++)
                {
                    var item = source[i];
                    if (!ReferenceEquals(item, null))
                    {
                        var resultItem = selector(item);
                        if (!ReferenceEquals(resultItem, null))
                        {
                            result[count++] = resultItem;
                        }
                    }
                }

                var oldResult = result;
                result = new TResult[count];
                count = 0;
                for (int i = 0; i < result.Length; i++)
                {
                    result[count++] = oldResult[i];
                }
            }

            return result;
        }

        #endregion Select
        
        #region SelectMany

        public static List<TResult> SelectManyList<TSource, TResult>(
            this List<TSource> source,
            Action<TSource, List<TResult>> selector)
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

            source.ForEach(item => selector(item, result));

            return result;
        }
        
        public static List<TResult> SelectManyList<TSource, TResult>(
            this TSource[] source,
            Action<TSource, List<TResult>> selector)
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

            source.ForEach(item => selector(item, result));

            return result;
        }

        #endregion SelectMany

        #region Foreach
        
        /// <summary>
        /// 遍历跳出
        /// </summary>
        /// <param name="source"></param>
        /// <param name="action">遍历函数,返回是否跳出遍历</param>
        /// <typeparam name="TSource"></typeparam>
        /// <exception cref="ArgumentNullException"></exception>
        public static void BreakForEach<TSource>(this List<TSource> source, Func<TSource, bool> action)
        {
            if (ReferenceEquals(source, null))
            {
                throw new ArgumentNullException("source array is null");
            }
        
            if (ReferenceEquals(action, null))
            {
                throw new ArgumentNullException("action is null");
            }
            
            for (int i = 0; i < source.Count; i++)
            {
                if (action(source[i]))
                {
                    break;
                }
            }
        }
        
        /// <summary>
        /// 遍历跳出
        /// </summary>
        /// <param name="source"></param>
        /// <param name="action">遍历函数,返回是否跳出遍历</param>
        /// <typeparam name="TSource"></typeparam>
        /// <exception cref="ArgumentNullException"></exception>
        public static void BreakForEach<TSource>(this TSource[] source, Func<TSource, bool> action)
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
                if (action(source[i]))
                {
                    break;
                }
            }
        }
        
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

        #region Contains
        
        public static bool Contains<TSource>(
            this TSource[] source,
            Func<TSource, bool> predicate)
        {
            if (ReferenceEquals(source, null))
            {
                throw new ArgumentNullException("source list is null");
            }

            if (ReferenceEquals(predicate, null))
            {
                throw new ArgumentNullException("selector is null");
            }

            bool contains = false;
            source.BreakForEach(item =>
            {
                contains = predicate(item);
                return contains;
            });
            return contains;
        }
        
        public static bool Contains<TSource>(
            this List<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (ReferenceEquals(source, null))
            {
                throw new ArgumentNullException("source list is null");
            }

            if (ReferenceEquals(predicate, null))
            {
                throw new ArgumentNullException("selector is null");
            }

            bool contains = false;
            source.BreakForEach(item =>
            {
                contains = predicate(item);
                return contains;
            });
            return contains;
        }

        #endregion Contains

        #region Concat
        
        public static TSource[] ConcatArray<TSource>(
            this TSource[] first,
            TSource[] second,
            Func<TSource, bool> predicate)
        {
            if (ReferenceEquals(second, null))
            {
                throw new ArgumentNullException("second list is null");
            }

            TSource[] resultArray = null;
            if (null == predicate)
            {
                resultArray = new TSource[first.Length + second.Length];
                for (int i = 0; i < first.Length; i++)
                {
                    resultArray[i] = first[i];
                }
                for (int i = 0; i < second.Length; i++)
                {
                    resultArray[first.Length + i] = second[i];
                }
                return resultArray;
            }
            
            bool[] result = new bool[first.Length + second.Length];
            int count = 0;
            for (int i = 0; i < first.Length; i++)
            {
                result[i] = predicate(first[i]);
                if (result[i])
                {
                    count++;
                }
            }
            for (int i = 0; i < second.Length; i++)
            {
                result[first.Length + i] = predicate(second[i]);
                if (result[first.Length + i])
                {
                    count++;
                }
            }
            resultArray = new TSource[count];
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i])
                {
                    resultArray[^count] = first[i];
                    count--;
                }
            }
            return resultArray;
        }
        
        #endregion Contains
    }
}