/*------------------------------
 * 脚本名称: DateTimeUtil
 * 创建者: movin
 * 创建日期: 2025/03/02
 * 来源: https://github.com/dotnet-easy/easy-dotnet
------------------------------*/

using System;

namespace WWFramework
{
    /// <summary>
    /// 时间工具类
    /// </summary>
    public static class DateTimeUtil
    {
        /// <summary>
        /// 将时间戳（秒级）转换为 DateTime 类型
        /// </summary>
        /// <param name="timestamp">时间戳（秒级）</param>
        /// <returns>转换后的 DateTime 类型</returns>
        public static DateTime ConvertToDateTimeSeconds(long timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp);
        }

        /// <summary>
        /// 将时间戳（毫秒级）转换为 DateTime 类型
        /// </summary>
        /// <param name="timestamp">时间戳（毫秒级）</param>
        /// <returns>转换后的 DateTime 类型</returns>
        public static DateTime ConvertToDateTime(long timestamp)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dt.AddMilliseconds(timestamp);
        }

        /// <summary>
        /// 获取一个指定范围内的随机日期时间
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <returns>随机日期时间</returns>
        public static DateTime GetRandomDateTime(DateTime minValue, DateTime maxValue)
        {
            TimeSpan timeSpan = maxValue - minValue;
            double totalSeconds = timeSpan.TotalSeconds;
            var random = new Random();
            int randomSeconds = random.Next(0, (int)totalSeconds + 1);
            return minValue.AddSeconds(randomSeconds);
        }
    }
}