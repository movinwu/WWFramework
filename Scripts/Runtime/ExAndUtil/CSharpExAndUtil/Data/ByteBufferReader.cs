/*------------------------------
 * 脚本名称: ByteBufferReader
 * 创建者: movin
 * 创建日期: 2025/05/07
------------------------------*/

#if UNITY_PS3 || UNITY_WII || UNITY_WIIU
    #define BIG_ENDIAN // 大端序平台
#endif

using System;

namespace WWFramework
{
    /// <summary>
    /// byte数据读取类
    /// <para> 提供各种整形，浮点型及布尔的读取，并支持了数组读取，当前最多支持二维数组 </para>
    /// <para> 写入顺序和读取顺序统一为小端序，避免跨平台出现字节序问题，因此对大端序平台进行特殊处理，大端序平台逐字节读取，并按照小端序规则拼接 </para>
    /// </summary>
    public sealed class ByteBufferReader
    {
        /// <summary>
        /// 缓存数据
        /// </summary>
        private readonly byte[] _buffer;

        /// <summary>
        /// 数据指针(读取指针)
        /// </summary>
        private int _readPointer;

        /// <summary>
        /// 数组当前长度
        /// </summary>
        private readonly int _length;
        
        public ByteBufferReader(int capacity = 1024)
        {
            _buffer = new byte[capacity];
            _readPointer = 0;
            _length = capacity;
        }
        
        public ByteBufferReader(byte[] buffer)
        {
            _buffer = buffer;
            _readPointer = 0;
            _length = buffer.Length;
        }

        /// <summary>
        /// 重置读取
        /// </summary>
        public void Reset()
        {
            _readPointer = 0;
        }

        /// <summary>
        /// 指针前进
        /// </summary>
        /// <param name="advance">新指针位置</param>
        /// <returns>原来的指针位置</returns>
        private int Advance(int advance)
        {
            if (_readPointer + advance > _length)
            {
                throw new IndexOutOfRangeException();
            }

            var pointer = _readPointer;
            _readPointer += advance;
            return pointer;
        }

        public unsafe bool ReadBool()
        {
            var pointer = Advance(1);
            fixed (byte* ptr = _buffer)
            {
                return *(ptr + pointer) != 0;
            }
        }

        public unsafe byte ReadByte()
        {
            var pointer = Advance(1);
            fixed (byte* ptr = _buffer)
            {
                return *(ptr + pointer);
            }
        }
        
        public unsafe sbyte ReadSbyte()
        {
            var pointer = Advance(1);
            fixed (byte* ptr = _buffer)
            {
                return (sbyte)*(ptr + pointer);
            }
        }

        public unsafe short ReadInt16()
        {
            var pointer = Advance(2);
            fixed (byte* ptr = _buffer)
            {
#if BIG_ENDIAN
                var readPtr = ptr + pointer;
                return (short)(readPtr[0] | (readPtr[1] << 8));
#else
                var shortPtr = (short*)ptr + pointer;
                return *shortPtr;
#endif
            }
        }
        
        public unsafe ushort ReadUInt16()
        {
            var pointer = Advance(2);
            fixed (byte* ptr = _buffer)
            {
#if BIG_ENDIAN
                var readPtr = ptr + pointer;
                return (ushort)(readPtr[0] | (readPtr[1] << 8));
#else
                var ushortPtr = (ushort*)(ptr + pointer);
                return *ushortPtr;
#endif
            }
        }
        
        public unsafe int ReadInt32()
        {
            var pointer = Advance(4);
            fixed (byte* ptr = _buffer)
            {
#if BIG_ENDIAN
                var readPtr = ptr + pointer;
                return readPtr[0] | (readPtr[1] << 8) | (readPtr[2] << 16) | (readPtr[3] << 24);
#else
                var intPtr = (int*)(ptr + pointer);
                return *intPtr;
#endif
            }
        }
        
        public unsafe uint ReadUInt32()
        {
            var pointer = Advance(4);
            fixed (byte* ptr = _buffer)
            {
#if BIG_ENDIAN
                var readPtr = ptr + pointer;
                return (uint)(readPtr[0] | (readPtr[1] << 8) | (readPtr[2] << 16) | (readPtr[3] << 24));
#else
                var uintPtr = (uint*)(ptr + pointer);
                return *uintPtr;
#endif
            }
        }

        public unsafe long ReadInt64()
        {
            var pointer = Advance(8);
            fixed (byte* ptr = _buffer)
            {
#if BIG_ENDIAN
                var readPtr = ptr + pointer;
                return (long)readPtr[0] | ((long)readPtr[1] << 8) | ((long)readPtr[2] << 16) | 
                       ((long)readPtr[3] << 24) | ((long)readPtr[4] << 32) | ((long)readPtr[5] << 40) |
                       ((long)readPtr[6] << 48) | ((long)readPtr[7] << 56);
#else
                var longPtr = (long*)(ptr + pointer);
                return *longPtr;
#endif
            }
        }

        public unsafe ulong ReadUInt64()
        {
            var pointer = Advance(8);
            fixed (byte* ptr = _buffer)
            {
#if BIG_ENDIAN
                var readPtr = ptr + pointer;
                return (ulong)readPtr[0] | ((ulong)readPtr[1] << 8) | ((ulong)readPtr[2] << 16) |
                       ((ulong)readPtr[3] << 24) | ((ulong)readPtr[4] << 32) | ((ulong)readPtr[5] << 40) |
                       ((ulong)readPtr[6] << 48) | ((ulong)readPtr[7] << 56);
#else
                var ulongPtr = (ulong*)(ptr + pointer);
                return *ulongPtr;
#endif
            }
        }

        public unsafe float ReadSingle()
        {
            var pointer = Advance(4);
            fixed (byte* ptr = _buffer)
            {
#if BIG_ENDIAN
                var readPtr = ptr + pointer;
                var num = (uint)(readPtr[0] | (readPtr[1] << 8) | (readPtr[2] << 16) | (readPtr[3] << 24));
                return *(float*)&num;
#else
                var floatPtr = (float*)(ptr + pointer);
                return *floatPtr;
#endif
            }
        }
        
        public unsafe double ReadDouble()
        {
            var pointer = Advance(8);
            fixed (byte* ptr = _buffer)
            {
#if BIG_ENDIAN
                var readPtr = ptr + pointer;
                var num = (ulong)(readPtr[0] | ((ulong)readPtr[1] << 8) | ((ulong)readPtr[2] << 16) |
                                 ((ulong)readPtr[3] << 24) | ((ulong)readPtr[4] << 32) | ((ulong)readPtr[5] << 40) |
                                 ((ulong)readPtr[6] << 48) | ((ulong)readPtr[7] << 56));
                return *(double*)&num;
#else
                var doublePtr = (double*)(ptr + pointer);
                return *doublePtr;
#endif
            }
        }

        public unsafe string ReadString()
        {
            var length = ReadInt32();
            var pointer = Advance(length);
            fixed (byte* ptr = _buffer)
            {
                return System.Text.Encoding.UTF8.GetString(ptr + pointer, length);
            }
        }

        public unsafe byte[] ReadByteArray()
        {
            var length = ReadInt32();
            var pointer = Advance(length);
            byte[] value = new byte[length];
            fixed (byte* srcPtr = _buffer)
            fixed (byte* destPtr = value)
            {
                byte* src = srcPtr + pointer;
                Buffer.MemoryCopy(src, destPtr, length, length);
            }
            return value;
        }

        public unsafe sbyte[] ReadSByteArray()
        {
            var length = ReadInt32();
            var pointer = Advance(length);
            sbyte[] value = new sbyte[length];
            fixed (byte* srcPtr = _buffer)
            fixed (sbyte* destPtr = value)
            {
                byte* src = srcPtr + pointer;
                Buffer.MemoryCopy(src, destPtr, length, length);
            }
            return value;
        }
        
        public unsafe short[] ReadInt16Array()
        {
            var length = ReadInt32();
            var arrayLen = length << 1;
            var pointer = Advance(arrayLen);
            short[] value = new short[length];
            fixed (byte* srcPtr = _buffer)
            fixed (short* destPtr = value)
            {
                byte* src = srcPtr + pointer;
#if BIG_ENDIAN
                for (int i = 0; i < arrayLen; i += 2)
                {
                    destPtr[i >> 1] = (short)(src[i] | (src[i + 1] << 8));
                }
#else
                Buffer.MemoryCopy(src, destPtr, arrayLen, arrayLen);
#endif
            }
            return value;
        }
        
        public unsafe ushort[] ReadUInt16Array()
        {
            var length = ReadInt32();
            var arrayLen = length << 1;
            var pointer = Advance(arrayLen);
            ushort[] value = new ushort[length];
            fixed (byte* srcPtr = _buffer)
            fixed (ushort* destPtr = value)
            {
                byte* src = srcPtr + pointer;
#if BIG_ENDIAN
                for (int i = 0; i < arrayLen; i += 2)
                {
                    destPtr[i >> 1] = (ushort)(src[i] | (src[i + 1] << 8));
                }
#else
                Buffer.MemoryCopy(src, destPtr, arrayLen, arrayLen);
#endif
            }
            return value;
        }

        public unsafe int[] ReadInt32Array()
        {
            var length = ReadInt32();
            var arrayLen = length << 2;
            var pointer = Advance(arrayLen);
            int[] value = new int[length];
            fixed (byte* srcPtr = _buffer)
            fixed (int* destPtr = value)
            {
                byte* src = srcPtr + pointer;
#if BIG_ENDIAN
                for (int i = 0; i < arrayLen; i += 4)
                {
                    destPtr[i >> 2] = src[i] | (src[i + 1] << 8) | (src[i + 2] << 16) | (src[i + 3] << 24);
                }
#else
                Buffer.MemoryCopy(src, destPtr, arrayLen, arrayLen);
#endif
            }
            return value;
        }

        public unsafe uint[] ReadUInt32Array()
        {
            var length = ReadInt32();
            var arrayLen = length << 2;
            var pointer = Advance(arrayLen);
            uint[] value = new uint[length];
            fixed (byte* srcPtr = _buffer)
            fixed (uint* destPtr = value)
            {
                byte* src = srcPtr + pointer;
#if BIG_ENDIAN
                for (int i = 0; i < arrayLen; i += 4)
                {
                    destPtr[i >> 2] = (uint)(src[i] | (src[i + 1] << 8) | (src[i + 2] << 16) | (src[i + 3] << 24));
                }
#else
                Buffer.MemoryCopy(src, destPtr, arrayLen, arrayLen);
#endif
            }
            return value;
        }

        public unsafe long[] ReadInt64Array()
        {
            var length = ReadInt32();
            var arrayLen = length << 3;
            var pointer = Advance(arrayLen);
            long[] value = new long[length];
            fixed (byte* srcPtr = _buffer)
            fixed (long* destPtr = value)
            {
                byte* src = srcPtr + pointer;
#if BIG_ENDIAN
                for (int i = 0; i < arrayLen; i += 8)
                {
                    destPtr[i >> 3] = (long)src[i] | ((long)src[i + 1] << 8) | ((long)src[i + 2] << 16) | 
                             ((long)src[i + 3] << 24) | ((long)src[i + 4] << 32) | ((long)src[i + 5] << 40) |
                             ((long)src[i + 6] << 48) | ((long)src[i + 7] << 56);
                }
#else
                Buffer.MemoryCopy(src, destPtr, arrayLen, arrayLen);
#endif
            }
            return value;
        }

        public unsafe ulong[] ReadUInt64Array()
        {
            var length = ReadInt32();
            var arrayLen = length << 3;
            var pointer = Advance(arrayLen);
            ulong[] value = new ulong[length];
            fixed (byte* srcPtr = _buffer)
            fixed (ulong* destPtr = value)
            {
                byte* src = srcPtr + pointer;
#if BIG_ENDIAN
                for (int i = 0; i < arrayLen; i += 8)
                {
                    destPtr[i >> 3] = (ulong)src[i] | ((ulong)src[i + 1] << 8) | ((ulong)src[i + 2] << 16) |
                             ((ulong)src[i + 3] << 24) | ((ulong)src[i + 4] << 32) | ((ulong)src[i + 5] << 40) |
                             ((ulong)src[i + 6] << 48) | ((ulong)src[i + 7] << 56);
                }
#else
                Buffer.MemoryCopy(src, destPtr, arrayLen, arrayLen);
#endif
            }
            return value;
        }

        public unsafe float[] ReadSingleArray()
        {
            var length = ReadInt32();
            var arrayLen = length << 2;
            var pointer = Advance(arrayLen);
            float[] value = new float[length];
            fixed (byte* srcPtr = _buffer)
            fixed (float* destPtr = value)
            {
                byte* src = srcPtr + pointer;
#if BIG_ENDIAN
                for (int i = 0; i < arrayLen; i += 4)
                {
                    uint num = (uint)(src[i] | (src[i + 1] << 8) | (src[i + 2] << 16) | (src[i + 3] << 24));
                    destPtr[i >> 2] = *(float*)&num;
                }
#else
                Buffer.MemoryCopy(src, destPtr, arrayLen, arrayLen);
#endif
            }
            return value;
        }
        
        public unsafe double[] ReadDoubleArray()
        {
            var length = ReadInt32();
            var arrayLen = length << 3;
            var pointer = Advance(arrayLen);
            double[] value = new double[length];
            fixed (byte* srcPtr = _buffer)
            fixed (double* destPtr = value)
            {
                byte* src = srcPtr + pointer;
#if BIG_ENDIAN
                for (int i = 0; i < arrayLen; i += 8)
                {
                    ulong num = (ulong)src[i] | ((ulong)src[i + 1] << 8) | ((ulong)src[i + 2] << 16) |
                               ((ulong)src[i + 3] << 24) | ((ulong)src[i + 4] << 32) | ((ulong)src[i + 5] << 40) |
                               ((ulong)src[i + 6] << 48) | ((ulong)src[i + 7] << 56);
                    destPtr[i >> 3] = *(double*)&num;
                }
#else
                Buffer.MemoryCopy(src, destPtr, arrayLen, arrayLen);
#endif
            }
            return value;
        }

        public unsafe bool[] ReadBoolArray()
        {
            var length = ReadInt32();
            var pointer = Advance(length);
            bool[] value = new bool[length];
            fixed (byte* srcPtr = _buffer)
            fixed (bool* destPtr = value)
            {
                byte* src = srcPtr + pointer;
                Buffer.MemoryCopy(src, destPtr, length, length);
            }
            return value;
        }

        public string[] ReadStringArray()
        {
            var length = ReadInt32();
            string[] value = new string[length];
            for (int i = 0; i < length; i++)
            {
                value[i] = ReadString();
            }
            return value;
        }

        public byte[][] ReadByteArrayArray()
        {
            var length = ReadInt32();
            byte[][] value = new byte[length][];
            for (int i = 0; i < length; i++)
            {
                value[i] = ReadByteArray();
            }
            return value;
        }

        public sbyte[][] ReadSByteArrayArray()
        {
            var length = ReadInt32();
            var value = new sbyte[length][];
            for (int i = 0; i < length; i++)
            {
                value[i] = ReadSByteArray();
            }
            return value;
        }
        
        public short[][] ReadInt16ArrayArray()
        {
            var length = ReadInt32();
            var value = new short[length][];
            for (int i = 0; i < length; i++)
            {
                value[i] = ReadInt16Array();
            }

            return value;
        }
        
        public ushort[][] ReadUInt16ArrayArray()
        {
            var length = ReadInt32();
            var value = new ushort[length][];
            for (int i = 0; i < length; i++)
            {
                value[i] = ReadUInt16Array();
            }
            return value;
        }
        
        public int[][] ReadInt32ArrayArray()
        {
            var length = ReadInt32();
            var value = new int[length][];
            for (int i = 0; i < length; i++)
            {
                value[i] = ReadInt32Array();
            }
            return value;
        }

        public uint[][] ReadUInt32ArrayArray()
        {
            var length = ReadInt32();
            var value = new uint[length][];
            for (int i = 0; i < length; i++)
            {
                value[i] = ReadUInt32Array();
            }
            return value;
        }
        
        public long[][] ReadInt64ArrayArray()
        {
            var length = ReadInt32();
            var value = new long[length][];
            for (int i = 0; i < length; i++)
            {
                value[i] = ReadInt64Array();
            }
            return value;
        }
        
        public ulong[][] ReadUInt64ArrayArray()
        {
            var length = ReadInt32();
            var value = new ulong[length][];
            for (int i = 0; i < length; i++)
            {
                value[i] = ReadUInt64Array();
            }
            return value;
        }

        public float[][] ReadSingleArrayArray()
        {
            var length = ReadInt32();
            var value = new float[length][];
            for (int i = 0; i < length; i++)
            {
                value[i] = ReadSingleArray();
            }
            return value;
        }
        
        public double[][] ReadDoubleArrayArray()
        {
            var length = ReadInt32();
            var value = new double[length][];
            for (int i = 0; i < length; i++)
            {
                value[i] = ReadDoubleArray();
            }
            return value;
        }

        public bool[][] ReadBoolArrayArray()
        {
            var length = ReadInt32();
            var value = new bool[length][];
            for (int i = 0; i < length; i++)
            {
                value[i] = ReadBoolArray();
            }
            return value;
        }

        public string[][] ReadStringArrayArray()
        {
            var length = ReadInt32();
            var value = new string[length][];
            for (int i = 0; i < length; i++)
            {
                value[i] = ReadStringArray();
            }
            return value;
        }
    }
}