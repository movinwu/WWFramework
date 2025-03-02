/*------------------------------
 * 脚本名称: MathUtil
 * 创建者: movin
 * 创建日期: 2025/02/23
 * 来源: https://github.com/dotnet-easy/easy-dotnet
------------------------------*/

using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 数学工具类
    /// </summary>
    public class MathUtil
    {
        /// <summary>
        /// 计算两个整数的最大公约数
        /// </summary>
        /// <param name="a">第一个整数</param>
        /// <param name="b">第二个整数</param>
        /// <returns>最大公约数</returns>
        public static int Gcd(int a, int b)
        {
            if (b == 0)
            {
                return a;
            }
            else
            {
                return Gcd(b, a % b);
            }
        }

        /// <summary>
        /// 计算两个整数的最小公倍数
        /// </summary>
        /// <param name="a">第一个整数</param>
        /// <param name="b">第二个整数</param>
        /// <returns>最小公倍数</returns>
        public static int Lcm(int a, int b)
        {
            return a * b / Gcd(a, b);
        }

        /// <summary>
        /// 判断一个整数是否为质数
        /// </summary>
        /// <param name="n">要判断的整数</param>
        /// <returns>如果是质数，则返回 true；否则返回 false</returns>
        public static bool IsPrime(int n)
        {
            if (n <= 1)
            {
                return false;
            }

            for (int i = 2; i <= Mathf.Sqrt(n); i++)
            {
                if (n % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 计算两个浮点数的差的绝对值是否小于指定的精度
        /// </summary>
        /// <param name="a">第一个浮点数</param>
        /// <param name="b">第二个浮点数</param>
        /// <param name="eps">指定的精度</param>
        /// <returns>如果两个浮点数的差的绝对值小于指定的精度，则返回 true；否则返回 false</returns>
        public static bool ApproxEqual(float a, float b, double eps)
        {
            return Mathf.Abs(a - b) < eps;
        }

        /// <summary>
        /// 求一个整数的阶乘
        /// </summary>
        /// <param name="n">要求阶乘的整数</param>
        /// <returns>阶乘结果</returns>
        public static int Factorial(int n)
        {
            if (n <= 1)
            {
                return 1;
            }
            else
            {
                return n * Factorial(n - 1);
            }
        }

        /// <summary>
        /// 求一个整数的斐波那契数列的值
        /// </summary>
        /// <param name="n">要求斐波那契数列的整数</param>
        /// <returns>斐波那契数列的值</returns>
        public static int Fibonacci(int n)
        {
            if (n <= 1)
            {
                return n;
            }
            else
            {
                return Fibonacci(n - 1) + Fibonacci(n - 2);
            }
        }

        /// <summary>
        /// 求一个整数的二进制表示中 1 的个数
        /// </summary>
        /// <param name="n">要求二进制表示中 1 的个数的整数</param>
        /// <returns>二进制表示中 1 的个数</returns>
        public static int CountBits(int n)
        {
            int count = 0;

            while (n != 0)
            {
                count++;
                n &= n - 1;
            }

            return count;
        }

        /// <summary>
        /// 判断一个整数是否为完全平方数
        /// </summary>
        /// <param name="n">要判断的整数</param>
        /// <param name="sqrt">整数的平方根</param>
        /// <returns>如果是完全平方数，则返回 true；否则返回 false</returns>
        public static bool IsPerfectSquare(int n, out int sqrt)
        {
            if (n < 0)
            {
                sqrt = 0;
                return false;
            }
            sqrt = (int)Mathf.Sqrt(n);
            return sqrt * sqrt == n;
        }

        /// <summary>
        /// 计算一个整数的各个数位上数字的平方和，如果结果为 1，则返回 true；否则进行下一次计算，直到结果为 1 或者进入死循环为止
        /// </summary>
        /// <param name="n">要计算的整数</param>
        /// <returns>如果结果为 1，则返回 true；否则返回 false</returns>
        public static bool IsHappyNumber(int n)
        {
            int sum = n;

            while (true)
            {
                int digitsSum = 0;
                while (sum > 0)
                {
                    int digit = sum % 10;
                    digitsSum += digit * digit;
                    sum /= 10;
                }

                if (digitsSum == 1)
                {
                    return true;
                }
                else if (digitsSum == 4)
                {
                    return false;
                }

                sum = digitsSum;
            }
        }

        /// <summary>
        /// 计算两个整数的二进制表示中有多少位不同
        /// </summary>
        /// <param name="a">第一个整数</param>
        /// <param name="b">第二个整数</param>
        /// <returns>两个整数的二进制表示中有多少位不同</returns>
        public static int HammingDistance(int a, int b)
        {
            int count = 0;
            int xor = a ^ b;

            while (xor != 0)
            {
                count++;
                xor &= xor - 1;
            }

            return count;
        }

        /// <summary>
        /// 求一个整数的所有因子
        /// </summary>
        /// <param name="n">要求因子的整数</param>
        /// <returns>所有因子</returns>
        public static int[] GetAllFactors(int n)
        {
            int count = 0;

            for (int i = 1; i <= Mathf.Sqrt(n); i++)
            {
                if (n % i == 0)
                {
                    count++;
                    if (i != n / i)
                    {
                        count++;
                    }
                }
            }

            int[] factors = new int[count];
            int index = 0;

            for (int i = 1; i <= Mathf.Sqrt(n); i++)
            {
                if (n % i == 0)
                {
                    factors[index++] = i;
                    if (i != n / i)
                    {
                        factors[index++] = n / i;
                    }
                }
            }

            return factors;
        }

        /// <summary>
        /// 把一个数字转换为二进制字符串
        /// </summary>
        /// <param name="number">待转换的数字</param>
        /// <returns>该数字的二进制字符串</returns>
        public static string ToBinaryString(int number)
        {
            if (number == 0)
            {
                return "0";
            }

            string result = string.Empty;
            while (number > 0)
            {
                result = (number % 2).ToString() + result;
                number /= 2;
            }

            return result;
        }

        /// <summary>
        /// 把一个数字转换为八进制字符串
        /// </summary>
        /// <param name="number">待转换的数字</param>
        /// <returns>该数字的八进制字符串</returns>
        public static string ToOctalString(int number)
        {
            if (number == 0)
            {
                return "0";
            }

            string result = string.Empty;
            while (number > 0)
            {
                result = (number % 8).ToString() + result;
                number /= 8;
            }

            return result;
        }

        /// <summary>
        /// 把一个数字转换为十六进制字符串
        /// </summary>
        /// <param name="number">待转换的数字</param>
        /// <returns>该数字的十六进制字符串</returns>
        public static string ToHexString(int number)
        {
            if (number == 0)
            {
                return "0";
            }

            string result = string.Empty;
            while (number > 0)
            {
                int remainder = number % 16;
                if (remainder < 10)
                {
                    result = remainder.ToString() + result;
                }
                else
                {
                    result = (char)('A' + remainder - 10) + result;
                }
                number /= 16;
            }

            return result;
        }
    }
}