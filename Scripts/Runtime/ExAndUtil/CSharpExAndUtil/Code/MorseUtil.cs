/*------------------------------
 * 脚本名称: MorseUtil
 * 创建者: movin
 * 创建日期: 2025/02/16
 * 来源: https://github.com/dotnet-easy/easy-dotnet
------------------------------*/

using System;
using System.Collections.Generic;

namespace WWFramework
{
    /// <summary>
    /// Morse 电码工具类
    /// </summary>
    public static class MorseUtil
    {
        /// <summary>
        /// Morse 电码分割字符
        /// </summary>
        private const char MORSE_SPLIT_CHAR = ' ';
        
        private static string[] _morseEncodeTable;
        /// <summary>
        /// Morse 电码表
        /// </summary>
        public static string[] MorseEncodeTable
        {
            get
            {
                if (null == _morseEncodeTable)
                {
                    _morseEncodeTable = new string[128];
                    
                    // 字母 A-Z
                    _morseEncodeTable['A'] = ".-";
                    _morseEncodeTable['B'] = "-...";
                    _morseEncodeTable['C'] = "-.-.";
                    _morseEncodeTable['D'] = "-..";
                    _morseEncodeTable['E'] = ".";
                    _morseEncodeTable['F'] = "..-.";
                    _morseEncodeTable['G'] = "--.";
                    _morseEncodeTable['H'] = "....";
                    _morseEncodeTable['I'] = "..";
                    _morseEncodeTable['J'] = ".---";
                    _morseEncodeTable['K'] = "-.-";
                    _morseEncodeTable['L'] = ".-..";
                    _morseEncodeTable['M'] = "--";
                    _morseEncodeTable['N'] = "-.";
                    _morseEncodeTable['O'] = "---";
                    _morseEncodeTable['P'] = ".--.";
                    _morseEncodeTable['Q'] = "--.-";
                    _morseEncodeTable['R'] = ".-.";
                    _morseEncodeTable['S'] = "...";
                    _morseEncodeTable['T'] = "-";
                    _morseEncodeTable['U'] = "..-";
                    _morseEncodeTable['V'] = "...-";
                    _morseEncodeTable['W'] = ".--";
                    _morseEncodeTable['X'] = "-..-";
                    _morseEncodeTable['Y'] = "-.--";
                    _morseEncodeTable['Z'] = "--..";
        
                    // 数字 0-9
                    _morseEncodeTable['0'] = "-----";
                    _morseEncodeTable['1'] = ".----";
                    _morseEncodeTable['2'] = "..---";
                    _morseEncodeTable['3'] = "...--";
                    _morseEncodeTable['4'] = "....-";
                    _morseEncodeTable['5'] = ".....";
                    _morseEncodeTable['6'] = "-....";
                    _morseEncodeTable['7'] = "--...";
                    _morseEncodeTable['8'] = "---..";
                    _morseEncodeTable['9'] = "----.";
        
                    // 标点符号
                    _morseEncodeTable['.'] = ".-.-.-";  // 句号
                    _morseEncodeTable[','] = "--..--";  // 逗号
                    _morseEncodeTable['?'] = "..--..";  // 问号
                    _morseEncodeTable['\''] = ".----."; // 单引号
                    _morseEncodeTable['!'] = "-.-.--";  // 感叹号
                    _morseEncodeTable['/'] = "-..-.";   // 斜杠
                    _morseEncodeTable['('] = "-.--.";   // 左括号
                    _morseEncodeTable[')'] = "-.--.-";  // 右括号
                    _morseEncodeTable['&'] = ".-...";   // and 符号
                    _morseEncodeTable[':'] = "---...";  // 冒号
                    _morseEncodeTable[';'] = "-.-.-.";  // 分号
                    _morseEncodeTable['='] = "-...-";   // 等号
                    _morseEncodeTable['+'] = ".-.-.";  // 加号
                    _morseEncodeTable['-'] = "-....-"; // 减号
                    _morseEncodeTable['_'] = "..--.-";  // 下划线
                    _morseEncodeTable['"'] = ".-..-.";  // 双引号
                    _morseEncodeTable['$'] = "...-..-"; // 美元符号
                    _morseEncodeTable['@'] = ".--.-.";  // at 符号
                }

                return _morseEncodeTable;
            }
        }

        private static Dictionary<string, char> _morseDecodeTable;
        /// <summary>
        /// Morse 解密电码表
        /// </summary>
        public static Dictionary<string, char> MorseDecodeTable
        {
            get
            {
                if (null == _morseDecodeTable)
                {
                    _morseDecodeTable = new Dictionary<string, char>(128);
                    for (int i = 0; i < MorseEncodeTable.Length; ++i)
                    {
                        var code = MorseEncodeTable[i];
                        if (!string.IsNullOrEmpty(code))
                        {
                            _morseDecodeTable.Add(code, (char)i);
                        }
                    }
                }

                return _morseDecodeTable;
            }
        }

        /// <summary>
        /// 将给定的字符串转换为 Morse 电码字符串。
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的 Morse 电码字符串</returns>
        public static string Encode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            List<string> morseCodes = new List<string>();
            foreach (char c in str.ToUpper())
            {
                if (!string.IsNullOrEmpty(MorseEncodeTable[c]))
                {
                    morseCodes.Add(MorseEncodeTable[c]);
                }
            }
            return string.Join(MORSE_SPLIT_CHAR, morseCodes);
        }


        /// <summary>
        /// 将给定的 Morse 电码字符串转换为原始字符串。
        /// </summary>
        /// <param name="morseCode">要转换的 Morse 电码字符串</param>
        /// <returns>转换后的原始字符串</returns>
        public static string Decode(string morseCode)
        {
            if (string.IsNullOrEmpty(morseCode))
            {
                return string.Empty;
            }

            string[] codes = morseCode.Split(new[] { MORSE_SPLIT_CHAR }, StringSplitOptions.RemoveEmptyEntries);
            
            return new string(codes.SelectArray(code =>
            {
                if (MorseDecodeTable.TryGetValue(code, out char c))
                {
                    return c;
                }
                return '-';
            }));
        }

    }
}
