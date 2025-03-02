/*------------------------------
 * 脚本名称: RotUtil
 * 创建者: movin
 * 创建日期: 2025/02/16
 * 来源: https://github.com/dotnet-easy/easy-dotnet
------------------------------*/

namespace WWFramework
{
    /// <summary>
    /// ROT 工具类
    /// </summary>
    public static class RotUtil
    {
        /// <summary>
        /// 将给定的字符串按照 ROT 加密算法进行加密。
        /// </summary>
        /// <param name="text">要加密的字符串</param>
        /// <param name="n">偏移量</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string text, int n)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            char[] charArray = text.ToCharArray();
            for (int i = 0; i < n; i++)
            {
                if (charArray[i] >= 'a' && charArray[i] <= 'z')
                {
                    charArray[i] = (char)((charArray[i] - 'a' + n) % 26 + 'a');
                }
                else if (charArray[i] >= 'A' && charArray[i] <= 'Z')
                {
                    charArray[i] = (char)((charArray[i] - 'A' + n) % 26 + 'A');
                }
            }
            return new string(charArray);
        }

        /// <summary>
        /// 将给定的字符串按照 ROT 加密算法进行解密。
        /// </summary>
        /// <param name="text">要解密的字符串</param>
        /// <param name="n">偏移量</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt(string text, int n)
        {
            return Encrypt(text, 26 - n);
        }
    }
}
