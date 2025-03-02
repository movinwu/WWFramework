/*------------------------------
 * 脚本名称: DateEx
 * 创建者: movin
 * 创建日期: 2025/02/23
 * 来源: https://github.com/dotnet-easy/easy-dotnet
------------------------------*/

using System;
using System.Collections.Generic;
using System.Globalization;

namespace WWFramework
{
    /// <summary>
    /// 日期拓展
    /// </summary>
    public static class DateTimeEx
    {
        /// <summary>
        /// 获取指定日期所在周的第一天的日期。
        /// </summary>
        /// <param name="date">指定日期。</param>
        /// <param name="cultureInfo">所属文化</param>
        /// <returns>指定日期所在周的第一天的日期。</returns>
        public static DateTime GetFirstDayOfWeek(this DateTime date, CultureInfo cultureInfo)
        {
            DayOfWeek firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            int offset = firstDayOfWeek - date.DayOfWeek;
            if (offset > 0)
            {
                offset -= 7;
            }
            return date.AddDays(offset).Date;
        }

        /// <summary>
        /// 获取指定日期所在月份的第一天的日期。
        /// </summary>
        /// <param name="date">指定日期。</param>
        /// <returns>指定日期所在月份的第一天的日期。</returns>
        public static DateTime GetFirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// 获取指定日期所在季度的第一天的日期。
        /// </summary>
        /// <param name="date">指定日期。</param>
        /// <returns>指定日期所在季度的第一天的日期。</returns>
        public static DateTime GetFirstDayOfQuarter(this DateTime date)
        {
            int quarter = (date.Month - 1) / 3 + 1;
            return new DateTime(date.Year, (quarter - 1) * 3 + 1, 1);
        }

        /// <summary>
        /// 获取指定日期所在年份的第一天的日期。
        /// </summary>
        /// <param name="date">指定日期。</param>
        /// <returns>指定日期所在年份的第一天的日期。</returns>
        public static DateTime GetFirstDayOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 1, 1);
        }

        /// <summary>
        /// 计算两个日期之间的天数差。
        /// </summary>
        /// <param name="date1">第一个日期。</param>
        /// <param name="date2">第二个日期。</param>
        /// <returns>两个日期之间的天数差。</returns>
        public static int GetDaysBetween(this DateTime date1, DateTime date2)
        {
            TimeSpan span = date2 - date1;
            return span.Days;
        }

        /// <summary>
        /// 计算两个日期之间的工作日数差。
        /// </summary>
        /// <param name="date1">第一个日期。</param>
        /// <param name="date2">第二个日期。</param>
        /// <returns>两个日期之间的工作日数差。</returns>
        public static int GetWorkDaysBetween(this DateTime date1, DateTime date2)
        {
            int count = 0;
            DateTime temp = date1;
            while (temp.Date != date2.Date)
            {
                if (temp.DayOfWeek != DayOfWeek.Saturday && temp.DayOfWeek != DayOfWeek.Sunday)
                {
                    count++;
                }
                temp = temp.AddDays(1);
            }
            return count;
        }

        /// <summary>
        /// 判断指定日期是否是工作日。
        /// </summary>
        /// <param name="date">指定日期。</param>
        /// <returns>如果是工作日，则返回 true；否则返回 false。</returns>
        public static bool IsWorkDay(this DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
        }

        /// <summary>
        /// 获取指定日期所在周的所有日期。
        /// </summary>
        /// <param name="date">指定日期。</param>
        /// <param name="cultureInfo">所属文化</param>
        /// <returns>指定日期所在周的所有日期。</returns>
        public static List<DateTime> GetWeekDays(this DateTime date, CultureInfo cultureInfo)
        {
            DayOfWeek firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            int offset = firstDayOfWeek - date.DayOfWeek;
            if (offset > 0)
            {
                offset -= 7;
            }
            DateTime firstDay = date.AddDays(offset).Date;
            List<DateTime> days = new List<DateTime>(7);
            for (int i = 0; i < 7; i++)
            {
                days.Add(firstDay.AddDays(i));
            }
            return days;
        }

        /// <summary>
        /// 获取指定日期所在月份的所有日期。
        /// </summary>
        /// <param name="date">指定日期。</param>
        /// <returns>指定日期所在月份的所有日期。</returns>
        public static List<DateTime> GetMonthDays(this DateTime date)
        {
            DateTime firstDay = new DateTime(date.Year, date.Month, 1);
            DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
            List<DateTime> days = new List<DateTime>(31);
            for (DateTime i = firstDay; i <= lastDay; i = i.AddDays(1))
            {
                days.Add(i);
            }
            return days;
        }

        /// <summary>
        /// 获取指定日期所在季度的所有日期。
        /// </summary>
        /// <param name="date">指定日期。</param>
        /// <returns>指定日期所在季度的所有日期。</returns>
        public static List<DateTime> GetQuarterDays(this DateTime date)
        {
            DateTime firstDay = GetFirstDayOfQuarter(date);
            DateTime lastDay = firstDay.AddMonths(3).AddDays(-1);
            List<DateTime> days = new List<DateTime>(93);
            for (DateTime i = firstDay; i <= lastDay; i = i.AddDays(1))
            {
                days.Add(i);
            }
            return days;
        }

        /// <summary>
        /// 获取指定日期所在年份的所有日期。
        /// </summary>
        /// <param name="date">指定日期。</param>
        /// <returns>指定日期所在年份的所有日期。</returns>
        public static List<DateTime> GetYearDays(this DateTime date)
        {
            DateTime firstDay = new DateTime(date.Year, 1, 1);
            DateTime lastDay = new DateTime(date.Year, 12, 31);
            List<DateTime> days = new List<DateTime>(366);
            for (DateTime i = firstDay; i <= lastDay; i = i.AddDays(1))
            {
                days.Add(i);
            }
            return days;
        }

        /// <summary>
        /// 将 DateTime 类型转换为时间戳（毫秒级）
        /// </summary>
        /// <param name="dateTime">DateTime 类型</param>
        /// <returns>转换后的时间戳（毫秒级）</returns>
        public static long ConvertToTimestamp(this DateTime dateTime)
        {
            TimeSpan ts = dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return (long)ts.TotalMilliseconds;
        }

        /// <summary>
        /// 将 DateTime 类型转换为时间戳（秒级）
        /// </summary>
        /// <param name="dateTime">DateTime 类型</param>
        /// <returns>转换后的时间戳（秒级）</returns>
        public static long ConvertToTimestampSeconds(this DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }
    }
}