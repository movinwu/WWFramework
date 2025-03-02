/*------------------------------
 * 脚本名称: StringEx
 * 创建者: movin
 * 创建日期: 2025/02/17
 * 来源: https://github.com/dotnet-easy/easy-dotnet
------------------------------*/

using System;
using System.Text.RegularExpressions;

namespace WWFramework
{
    /// <summary>
    /// 字符串拓展
    /// </summary>
    public static class StringEx
    {
        /// <summary>
        /// 移除字符串中的所有空格
        /// </summary>
        /// <param name="str">要处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        public static string RemoveAllSpaces(this string str)
        {
            return Regex.Replace(str, @"\s+", "");
        }

        /// <summary>
        /// 将字符串转换为驼峰命名法
        /// </summary>
        /// <param name="str">要处理的字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string ToCamelCase(this string str)
        {
            string[] words = str.Split(new char[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);
            string result = "";
            for (int i = 0; i < words.Length; i++)
            {
                if (i == 0)
                {
                    result += words[i].ToLower();
                }
                else
                {
                    result += words[i].Substring(0, 1).ToUpper() + words[i].Substring(1).ToLower();
                }
            }
            return result;
        }

        /// <summary>
        /// 将字符串转换为帕斯卡命名法（大驼峰命名法）
        /// </summary>
        /// <param name="str">要处理的字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string ToPascalCase(this string str)
        {
            string[] words = str.Split(new char[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);
            string result = "";
            for (int i = 0; i < words.Length; i++)
            {
                result += words[i].Substring(0, 1).ToUpper() + words[i].Substring(1).ToLower();
            }
            return result;
        }

        /// <summary>
        /// 将字符串转换为下划线命名法
        /// </summary>
        /// <param name="str">要处理的字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string ToSnakeCase(this string str)
        {
            string[] words = str.Split(new char[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);
            string result = "";
            for (int i = 0; i < words.Length; i++)
            {
                if (i == 0)
                {
                    result += words[i].ToLower();
                }
                else
                {
                    result += "_" + words[i].ToLower();
                }
            }
            return result;
        }

        /// <summary>
        /// 将字符串转换为连字符命名法（短横线命名法）
        /// </summary>
        /// <param name="str">要处理的字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string ToKebabCase(this string str)
        {
            string[] words = str.Split(new char[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);
            string result = "";
            for (int i = 0; i < words.Length; i++)
            {
                if (i == 0)
                {
                    result += words[i].ToLower();
                }
                else
                {
                    result += "-" + words[i].ToLower();
                }
            }
            return result;
        }

        /// <summary>
        /// 将字符串中的 HTML 标记去除
        /// </summary>
        /// <param name="str">要处理的字符串</param>
        /// <returns>去除 HTML 标记后的字符串</returns>
        public static string StripHtml(this string str)
        {
            return Regex.Replace(str, "<.*?>", "");
        }

        /// <summary>
        /// 比较两个字符串是否相等，忽略大小写和空格
        /// </summary>
        /// <param name="str1">第一个字符串</param>
        /// <param name="str2">第二个字符串</param>
        /// <returns>如果相等，则返回true，否则返回false</returns>
        public static bool EqualsIgnoreCaseAndWhiteSpace(this string str1, string str2)
        {
            return string.Equals(RemoveAllSpaces(str1), RemoveAllSpaces(str2), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 将字符串中的某些字符替换成指定的字符
        /// </summary>
        /// <param name="str">要处理的字符串</param>
        /// <param name="chars">要替换的字符数组</param>
        /// <param name="newChar">新的字符</param>
        /// <returns>处理后的字符串</returns>
        public static string ReplaceChars(this string str, char[] chars, char newChar)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                str = str.Replace(chars[i], newChar);
            }
            return str;
        }

        /// <summary>
        /// 将字符串中的某些子字符串替换成指定的子字符串
        /// </summary>
        /// <param name="str">要处理的字符串</param>
        /// <param name="oldValues">要替换的子字符串数组</param>
        /// <param name="newValue">新的子字符串</param>
        /// <returns>处理后的字符串</returns>
        public static string ReplaceStrings(this string str, string[] oldValues, string newValue)
        {
            for (int i = 0; i < oldValues.Length; i++)
            {
                str = str.Replace(oldValues[i], newValue);
            }
            return str;
        }

        /// <summary>
        /// 将字符串中的某些子字符串替换成指定的子字符串，忽略大小写
        /// </summary>
        /// <param name="str">要处理的字符串</param>
        /// <param name="oldValues">要替换的子字符串数组</param>
        /// <param name="newValue">新的子字符串</param>
        /// <returns>处理后的字符串</returns>
        public static string ReplaceStringsIgnoreCase(this string str, string[] oldValues, string newValue)
        {
            for (int i = 0; i < oldValues.Length; i++)
            {
                str = Regex.Replace(str, oldValues[i], newValue, RegexOptions.IgnoreCase);
            }
            return str;
        }

        /// <summary>
        /// 将字符串中的首字母大写
        /// </summary>
        /// <param name="str">要处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        public static string ToFirstLetterUpperCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            char[] chars = str.ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            return new string(chars);
        }

        /// <summary>
        /// 将字符串中的首字母小写
        /// </summary>
        /// <param name="str">要处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        public static string ToFirstLetterLowerCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            char[] chars = str.ToCharArray();
            chars[0] = char.ToLower(chars[0]);
            return new string(chars);
        }
    }
}