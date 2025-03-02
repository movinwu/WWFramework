/*------------------------------
 * 脚本名称: EscapeUtil
 * 创建者: movin
 * 创建日期: 2025/03/02
 * 来源: https://github.com/dotnet-easy/easy-dotnet
------------------------------*/

using System.Text.RegularExpressions;

namespace WWFramework
{
    /// <summary>
    /// 转义和反转义工具类
    /// </summary>
    public class EscapeUtil
    {
        /// <summary>
        /// 将字符串中的特殊字符进行转义
        /// </summary>
        /// <param name="str">需要转义的字符串</param>
        /// <returns>转义后的字符串</returns>
        public static string Escape(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            string escaped = Regex.Replace(str, @"[\a\b\f\n\r\t\v\\""]", m => {
                switch (m.Value)
                {
                    case "\a":
                        return @"\a";
                    case "\b":
                        return @"\b";
                    case "\f":
                        return @"\f";
                    case "\n":
                        return @"\n";
                    case "\r":
                        return @"\r";
                    case "\t":
                        return @"\t";
                    case "\v":
                        return @"\v";
                    case "\\":
                        return @"\\";
                    case "\"":
                        return @"\""";
                    default:
                        return m.Value;
                }
            });

            return escaped;
        }

        /// <summary>
        /// 将字符串中的转义字符还原成特殊字符
        /// </summary>
        /// <param name="str">需要还原的字符串</param>
        /// <returns>还原后的字符串</returns>
        public static string Unescape(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            string unescaped = Regex.Replace(str, @"\\[a-z""\\]", m => {
                switch (m.Value)
                {
                    case @"\a":
                        return "\a";
                    case @"\b":
                        return "\b";
                    case @"\f":
                        return "\f";
                    case @"\n":
                        return "\n";
                    case @"\r":
                        return "\r";
                    case @"\t":
                        return "\t";
                    case @"\v":
                        return "\v";
                    case @"\\":
                        return "\\";
                    case @"\""":
                        return "\"";
                    default:
                        return m.Value;
                }
            });

            return unescaped;
        }
    }
}