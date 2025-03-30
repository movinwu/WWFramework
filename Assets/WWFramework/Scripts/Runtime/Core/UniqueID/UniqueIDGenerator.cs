/*------------------------------
 * 脚本名称: UniqueIDGenerator
 * 创建者: movin
 * 创建日期: 2025/03/30
------------------------------*/

using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 唯一ID生成器,确保生成唯一ID(提供ulong,long,int,uint四种类型的唯一id生成)
    /// </summary>
    public static class UniqueIDGenerator
    {
        /// <summary>
        /// 当前的ulong唯一id值
        /// </summary>
        private static ulong _ulongID = ulong.MinValue;

        /// <summary>
        /// 当前的long唯一id值
        /// </summary>
        private static long _longID = long.MinValue;

        /// <summary>
        /// 当前的int唯一id值
        /// </summary>
        private static int _intID = int.MinValue;

        /// <summary>
        /// 当前的uint唯一id值
        /// </summary>
        private static uint _uintID = uint.MinValue;

        /// <summary>
        /// ulong类型的唯一id值
        /// </summary>
        public static ulong ULongID
        {
            get => ++_ulongID;
            set
            {
                if (value > _ulongID)
                {
                    _ulongID = value;
                }
            }
        }

        /// <summary>
        /// long类型的唯一id值
        /// </summary>
        public static long LongID
        {
            get => ++_longID;
            set
            {
                if (value > _longID)
                {
                    _longID = value;
                }
            }
        }

        /// <summary>
        /// int类型的唯一id值
        /// </summary>
        public static int IntID
        {
            get => ++_intID;
            set
            {
                if (value > _intID)
                {
                    _intID = value;
                }
            }
        }

        /// <summary>
        /// uint类型的唯一id值
        /// </summary>
        public static uint UIntID
        {
            get => ++_uintID;
            set
            {
                if (value > _uintID)
                {
                    _uintID = value;
                }
            }
        }
    }
}