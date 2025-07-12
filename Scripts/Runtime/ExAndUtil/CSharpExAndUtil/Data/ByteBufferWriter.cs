/*------------------------------
 * 脚本名称: ByteBufferWriter
 * 创建者: movin
 * 创建日期: 2025/05/07
------------------------------*/

#if UNITY_PS3 || UNITY_WII || UNITY_WIIU
    #define BIG_ENDIAN // 大端序平台
#endif

using System;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// byte数据写入类
    /// <para> 提供各种整形，浮点型及布尔的写入，并支持了数组写入，当前最多支持二维数组 </para>
    /// <para> 写入顺序和读取顺序统一为小端序，避免跨平台出现字节序问题，因此对大端序平台进行特殊处理，大端序平台按照小端序平台的顺序逐字节写入 </para>
    /// </summary>
    public sealed class ByteBufferWriter
    {
        /// <summary>
        /// 缓存数据
        /// </summary>
        private byte[] _buffer;

        /// <summary>
        /// 缓存数据
        /// </summary>
        public byte[] SourceBuffer => _buffer;

        /// <summary>
        /// 数据指针(写入指针)
        /// </summary>
        private int _writePointer;

        /// <summary>
        /// 数据指针(写入指针)
        /// </summary>
        public int WritePointer
        {
            get => _writePointer;
            set
            {
                _writePointer = 0;
                Advance(Mathf.Max(value, 0));
            }
        }

        /// <summary>
        /// 数组当前长度
        /// </summary>
        private int _length;
        
        public ByteBufferWriter(int capacity = 1024)
        {
            _buffer = new byte[capacity];
            _writePointer = 0;
            _length = capacity;
        }
        
        public ByteBufferWriter(byte[] buffer)
        {
            _buffer = buffer;
            _writePointer = 0;
            _length = buffer.Length;
        }

        /// <summary>
        /// 重置写入指针
        /// </summary>
        public void ResetWriter()
        {
            _writePointer = 0;
            _length = _buffer.Length;
        }

        /// <summary>
        /// 导出当前写入的所有数据数组
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            var data = new byte[_writePointer];
            System.Buffer.BlockCopy(_buffer, 0, data, 0, _writePointer);
            return data;
        }

        /// <summary>
        /// 指针前进
        /// </summary>
        /// <param name="advance">新指针位置</param>
        /// <returns>原来的指针位置</returns>
        private int Advance(int advance)
        {
            if (_writePointer + advance > _length)
            {
                var newLength = Mathf.Max(4, _length * 2);
                var newBuffer = new byte[newLength];
                System.Buffer.BlockCopy(_buffer, 0, newBuffer, 0, _writePointer);
                _buffer = newBuffer;
            }

            var pointer = _writePointer;
            _writePointer += advance;
            return pointer;
        }

        public unsafe void WriteBool(bool data)
        {
            var pointer = Advance(1);
            fixed (byte* ptr = _buffer)
            {
                *(bool*)(ptr + pointer) = data;
            }
        }

        public unsafe void WriteByte(byte data)
        {
            var pointer = Advance(1);
            fixed (byte* ptr = _buffer)
            {
                *(ptr + pointer) = data;
            }
        }
        
        public unsafe void WriteSbyte(sbyte data)
        {
            var pointer = Advance(1);
            fixed (byte* ptr = _buffer)
            {
                *(sbyte*)(ptr + pointer) = data;
            }
        }

        public unsafe void WriteInt16(short data)
        {
            var pointer = Advance(2);
            fixed (byte* ptr = _buffer)
            {
#if BIG_ENDIAN
                byte* p = ptr + pointer;
                p[0] = (byte)data;
                p[1] = (byte)(data >> 8);
#else
                *(short*)(ptr + pointer) = data;
#endif
            }
        }
    
        public unsafe void WriteUInt16(ushort data)
        {
            var pointer = Advance(2);
            fixed (byte* ptr = _buffer)
            {
#if BIG_ENDIAN
                byte* p = ptr + pointer;
                p[0] = (byte)data;
                p[1] = (byte)(data >> 8);
#else
                *(ushort*)(ptr + pointer) = data;
#endif
            }
        }
    
        public unsafe void WriteInt32(int data)
        {
            var pointer = Advance(4);
            fixed (byte* ptr = _buffer)
            {
#if BIG_ENDIAN
                byte* p = ptr + pointer;
                p[0] = (byte)data;
                p[1] = (byte)(data >> 8);
                p[2] = (byte)(data >> 16);
                p[3] = (byte)(data >> 24);
#else
                *(int*)(ptr + pointer) = data;
#endif
            }
        }
    
        public unsafe void WriteUInt32(uint data)
        {
            var pointer = Advance(4);
            fixed (byte* ptr = _buffer)
            {
#if BIG_ENDIAN
                byte* p = ptr + pointer;
                p[0] = (byte)data;
                p[1] = (byte)(data >> 8);
                p[2] = (byte)(data >> 16);
                p[3] = (byte)(data >> 24);
#else
                *(uint*)(ptr + pointer) = data;
#endif
            }
        }

        public unsafe void WriteInt64(long data)
        {
            var pointer = Advance(8);
            fixed (byte* ptr = _buffer)
            {
#if BIG_ENDIAN
                byte* p = ptr + pointer;
                p[0] = (byte)data;
                p[1] = (byte)(data >> 8);
                p[2] = (byte)(data >> 16);
                p[3] = (byte)(data >> 24);
                p[4] = (byte)(data >> 32);
                p[5] = (byte)(data >> 40);
                p[6] = (byte)(data >> 48);
                p[7] = (byte)(data >> 56);
#else
                *(long*)(ptr + pointer) = data;
#endif
            }
        }
    
        public unsafe void WriteUInt64(ulong data)
        {
            var pointer = Advance(8);
            fixed (byte* ptr = _buffer)
            {
#if BIG_ENDIAN
                byte* p = ptr + pointer;
                p[0] = (byte)data;
                p[1] = (byte)(data >> 8);
                p[2] = (byte)(data >> 16);
                p[3] = (byte)(data >> 24);
                p[4] = (byte)(data >> 32);
                p[5] = (byte)(data >> 40);
                p[6] = (byte)(data >> 48);
                p[7] = (byte)(data >> 56);
#else
                *(ulong*)(ptr + pointer) = data;
#endif
            }
        }

        public unsafe void WriteSingle(float data)
        {
            var pointer = Advance(4);
            fixed (byte* ptr = _buffer)
            {
#if BIG_ENDIAN
                uint num = *(uint*)&data;
                byte* p = ptr + pointer;
                p[0] = (byte)num;
                p[1] = (byte)(num >> 8);
                p[2] = (byte)(num >> 16);
                p[3] = (byte)(num >> 24);
#else
                *(float*)(ptr + pointer) = data;
#endif
            }
        }
    
        public unsafe void WriteDouble(double data)
        {
            var pointer = Advance(8);
            fixed (byte* ptr = _buffer)
            {
#if BIG_ENDIAN
                ulong num = *(ulong*)&data;
                byte* p = ptr + pointer;
                p[0] = (byte)num;
                p[1] = (byte)(num >> 8);
                p[2] = (byte)(num >> 16);
                p[3] = (byte)(num >> 24);
                p[4] = (byte)(num >> 32);
                p[5] = (byte)(num >> 40);
                p[6] = (byte)(num >> 48);
                p[7] = (byte)(num >> 56);
#else
                *(double*)(ptr + pointer) = data;
#endif
            }
        }

        public unsafe void WriteString(string value)
        {
            var pointer = Advance(4 + value.Length << 2);  // 使用UTF-8编码时，一个字符可能占用4个字节，这里保证最坏情况下也不会越界
            var length = System.Text.Encoding.UTF8.GetBytes(value, 0, value.Length, _buffer, pointer + 4);
            fixed (byte* ptr = _buffer)
            {
#if BIG_ENDIAN
                byte* p = ptr + pointer;
                p[0] = (byte)length;
                p[1] = (byte)(length >> 8);
                p[2] = (byte)(length >> 16);
                p[3] = (byte)(length >> 24);
#else
                *(int*)(ptr + pointer) = length;
#endif
            }
            // 重置已经不正确的写入指针
            _writePointer = pointer + 4 + length;
        }

        public unsafe void WriteByteArray(byte[] value)
        {
            if (null == value || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }

            var length = value.Length;
            var pointer = Advance(length + 4);
            fixed (byte* srcPtr = value)
            fixed (byte* destPtr = _buffer)
            {
#if BIG_ENDIAN
                byte* p = destPtr + pointer;
                p[0] = (byte)length;
                p[1] = (byte)(length >> 8);
                p[2] = (byte)(length >> 16);
                p[3] = (byte)(length >> 24);
#else
                *(int*)(destPtr + pointer) = length;
#endif
                Buffer.MemoryCopy(srcPtr, destPtr + pointer + 4, length, length);
            }
        }

        public unsafe void WriteSByteArray(sbyte[] value)
        {
            if (null == value || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }

            var length = value.Length;
            var pointer = Advance(length + 4);
            fixed (sbyte* srcPtr = value)
            fixed (byte* destPtr = _buffer)
            {
#if BIG_ENDIAN
                byte* p = destPtr + pointer;
                p[0] = (byte)length;
                p[1] = (byte)(length >> 8);
                p[2] = (byte)(length >> 16);
                p[3] = (byte)(length >> 24);
#else
                *(int*)(destPtr + pointer) = length;
#endif
                Buffer.MemoryCopy(srcPtr, destPtr + pointer + 4, length, length);
            }
        }
        
        public unsafe void WriteInt16Array(short[] value)
        {
            if (null == value || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }

            var length = value.Length;
            var arrayLength = length << 1;
            var pointer = Advance(arrayLength + 4);
            fixed (short* srcPtr = value)
            fixed (byte* destPtr = _buffer)
            {
#if BIG_ENDIAN
                byte* p = destPtr + pointer;
                p[0] = (byte)length;
                p[1] = (byte)(length >> 8);
                p[2] = (byte)(length >> 16);
                p[3] = (byte)(length >> 24);
                for (int i = 0; i < arrayLength; i += 2)
                {
                    var num = *(srcPtr + (i >> 1));
                    p[i + 4] = (byte)num;
                    p[i + 5] = (byte)(num >> 8);
                }
#else
                *(int*)(destPtr + pointer) = length;
                Buffer.MemoryCopy(srcPtr, destPtr + pointer + 4, arrayLength, arrayLength);
#endif
            }
        }

        public unsafe void WriteUInt16Array(ushort[] value)
        {
            if (null == value || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }

            var length = value.Length;
            var arrayLength = length << 1;
            var pointer = Advance(arrayLength + 4);
            fixed (ushort* srcPtr = value)
            fixed (byte* destPtr = _buffer)
            {
#if BIG_ENDIAN
                byte* p = destPtr + pointer;
                p[0] = (byte)length;
                p[1] = (byte)(length >> 8);
                p[2] = (byte)(length >> 16);
                p[3] = (byte)(length >> 24);
                for (int i = 0; i < arrayLength; i += 2)
                {
                    var num = *(srcPtr + (i >> 1));
                    p[i + 4] = (byte)num;
                    p[i + 5] = (byte)(num >> 8);
                }
#else
                *(int*)(destPtr + pointer) = length;
                Buffer.MemoryCopy(srcPtr, destPtr + pointer + 4, arrayLength, arrayLength);
#endif
            }
        }

        public unsafe void WriteInt32Array(int[] value)
        {
            if (null == value || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }

            var length = value.Length;
            var arrayLength = length << 2;
            var pointer = Advance(arrayLength + 4);
            fixed (int* srcPtr = value)
            fixed (byte* destPtr = _buffer)
            {
#if BIG_ENDIAN
                byte* p = destPtr + pointer;
                p[0] = (byte)length;
                p[1] = (byte)(length >> 8);
                p[2] = (byte)(length >> 16);
                p[3] = (byte)(length >> 24);
                for (int i = 0; i < arrayLength; i += 4)
                {
                    var num = *(srcPtr + (i >> 2));
                    p[i + 4] = (byte)num;
                    p[i + 5] = (byte)(num >> 8);
                    p[i + 6] = (byte)(num >> 16);
                    p[i + 7] = (byte)(num >> 24);
                }
#else
                *(int*)(destPtr + pointer) = length;
                Buffer.MemoryCopy(srcPtr, destPtr + pointer + 4, arrayLength, arrayLength);
#endif
            }
        }
        
        public unsafe void WriteUInt32Array(uint[] value)
        {
            if (null == value || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }

            var length = value.Length;
            var arrayLength = length << 2;
            var pointer = Advance(arrayLength + 4);
            fixed (uint* srcPtr = value)
            fixed (byte* destPtr = _buffer)
            {
#if BIG_ENDIAN
                byte* p = destPtr + pointer;
                p[0] = (byte)length;
                p[1] = (byte)(length >> 8);
                p[2] = (byte)(length >> 16);
                p[3] = (byte)(length >> 24);
                for (int i = 0; i < arrayLength; i += 4)
                {
                    var num = *(srcPtr + (i >> 2));
                    p[i + 4] = (byte)num;
                    p[i + 5] = (byte)(num >> 8);
                    p[i + 6] = (byte)(num >> 16);
                    p[i + 7] = (byte)(num >> 24);
                }
#else
                *(int*)(destPtr + pointer) = length;
                Buffer.MemoryCopy(srcPtr, destPtr + pointer + 4, arrayLength, arrayLength);
#endif
            }
        }

        public unsafe void WriteInt64Array(long[] value)
        {
            if (null == value || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }

            var length = value.Length;
            var arrayLength = length << 3;
            var pointer = Advance(arrayLength + 4);
            fixed (long* srcPtr = value)
            fixed (byte* destPtr = _buffer)
            {
#if BIG_ENDIAN
                byte* p = destPtr + pointer;
                p[0] = (byte)length;
                p[1] = (byte)(length >> 8);
                p[2] = (byte)(length >> 16);
                p[3] = (byte)(length >> 24);
                for (int i = 0; i < arrayLength; i += 8)
                {
                    var num = *(srcPtr + (i >> 3));
                    p[i + 4] = (byte)num;
                    p[i + 5] = (byte)(num >> 8);
                    p[i + 6] = (byte)(num >> 16);
                    p[i + 7] = (byte)(num >> 24);
                    p[i + 8] = (byte)(num >> 32);
                    p[i + 9] = (byte)(num >> 40);
                    p[i + 10] = (byte)(num >> 48);
                    p[i + 11] = (byte)(num >> 56);
                }
#else
                *(int*)(destPtr + pointer) = length;
                Buffer.MemoryCopy(srcPtr, destPtr + pointer + 4, arrayLength, arrayLength);
#endif
            }
        }
        
        public unsafe void WriteUInt64Array(ulong[] value)
        {
            if (null == value || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }

            var length = value.Length;
            var arrayLength = length << 3;
            var pointer = Advance(arrayLength + 4);
            fixed (ulong* srcPtr = value)
            fixed (byte* destPtr = _buffer)
            {
#if BIG_ENDIAN
                byte* p = destPtr + pointer;
                p[0] = (byte)length;
                p[1] = (byte)(length >> 8);
                p[2] = (byte)(length >> 16);
                p[3] = (byte)(length >> 24);
                for (int i = 0; i < arrayLength; i += 8)
                {
                    var num = *(srcPtr + (i >> 3));
                    p[i + 4] = (byte)num;
                    p[i + 5] = (byte)(num >> 8);
                    p[i + 6] = (byte)(num >> 16);
                    p[i + 7] = (byte)(num >> 24);
                    p[i + 8] = (byte)(num >> 32);
                    p[i + 9] = (byte)(num >> 40);
                    p[i + 10] = (byte)(num >> 48);
                    p[i + 11] = (byte)(num >> 56);
                }
#else
                *(int*)(destPtr + pointer) = length;
                Buffer.MemoryCopy(srcPtr, destPtr + pointer + 4, arrayLength, arrayLength);
#endif
            }
        }

        public unsafe void WriteSingleArray(float[] value)
        {
            if (null == value || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }

            var length = value.Length;
            var arrayLength = length << 2;
            var pointer = Advance(arrayLength + 4);
            fixed (float* srcPtr = value)
            fixed (byte* destPtr = _buffer)
            {
#if BIG_ENDIAN
                byte* p = destPtr + pointer;
                p[0] = (byte)length;
                p[1] = (byte)(length >> 8);
                p[2] = (byte)(length >> 16);
                p[3] = (byte)(length >> 24);
                for (int i = 0; i < arrayLength; i += 4)
                {
                    var num = *(uint*)(srcPtr + (i >> 2));
                    p[i + 4] = (byte)num;
                    p[i + 5] = (byte)(num >> 8);
                    p[i + 6] = (byte)(num >> 16);
                    p[i + 7] = (byte)(num >> 24);
                }
#else
                *(int*)(destPtr + pointer) = length;
                Buffer.MemoryCopy(srcPtr, destPtr + pointer + 4, arrayLength, arrayLength);
#endif
            }
        }
        
        public unsafe void WriteDoubleArray(double[] value)
        {
            if (null == value || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }

            var length = value.Length;
            var arrayLength = length << 3;
            var pointer = Advance(arrayLength + 4);
            fixed (double* srcPtr = value)
            fixed (byte* destPtr = _buffer)
            {
#if BIG_ENDIAN
                byte* p = destPtr + pointer;
                p[0] = (byte)length;
                p[1] = (byte)(length >> 8);
                p[2] = (byte)(length >> 16);
                p[3] = (byte)(length >> 24);
                for (int i = 0; i < arrayLength; i += 8)
                {
                    var num = *(ulong*)(srcPtr + (i >> 3));
                    p[i + 4] = (byte)num;
                    p[i + 5] = (byte)(num >> 8);
                    p[i + 6] = (byte)(num >> 16);
                    p[i + 7] = (byte)(num >> 24);
                    p[i + 8] = (byte)(num >> 32);
                    p[i + 9] = (byte)(num >> 40);
                    p[i + 10] = (byte)(num >> 48);
                    p[i + 11] = (byte)(num >> 56);
                }
#else
                *(int*)(destPtr + pointer) = length;
                Buffer.MemoryCopy(srcPtr, destPtr + pointer + 4, arrayLength, arrayLength);
#endif
            }
        }

        public unsafe void WriteBoolArray(bool[] value)
        {
            if (null == value || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }

            var length = value.Length;
            var pointer = Advance(length + 4);
            fixed (bool* srcPtr = value)
            fixed (byte* destPtr = _buffer)
            {
                *(int*)(destPtr + pointer) = length;
                Buffer.MemoryCopy(srcPtr, destPtr + pointer + 4, length, length);
            }
        }

        public void WriteStringArray(string[] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }
            WriteInt32(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteString(value[i]);
            }
        }

        public void WriteByteArrayArray(byte[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }
            WriteInt32(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteByteArray(value[i]);
            }
        }

        public void WriteSByteArrayArray(sbyte[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }
            WriteInt32(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteSByteArray(value[i]);
            }
        }
        
        public void WriteInt16ArrayArray(short[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }
            WriteInt32(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteInt16Array(value[i]);
            }
        }
        
        public void WriteUInt16ArrayArray(ushort[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }
            WriteInt32(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteUInt16Array(value[i]);
            }
        }
        
        public void WriteInt32ArrayArray(int[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }
            WriteInt32(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteInt32Array(value[i]);
            }
        }

        public void WriteUInt32ArrayArray(uint[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }
            WriteInt32(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteUInt32Array(value[i]);
            }
        }
        
        public void WriteInt64ArrayArray(long[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }
            WriteInt32(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteInt64Array(value[i]);
            }
        }
        
        public void WriteUInt64ArrayArray(ulong[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }
            WriteInt32(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteUInt64Array(value[i]);
            }
        }

        public void WriteSingleArrayArray(float[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }
            WriteInt32(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteSingleArray(value[i]);
            }
        }
        
        public void WriteDoubleArrayArray(double[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }
            WriteInt32(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteDoubleArray(value[i]);
            }
        }

        public void WriteBoolArrayArray(bool[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }
            WriteInt32(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteBoolArray(value[i]);
            }
        }

        public void WriteStringArrayArray(string[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteInt32(0);
                return;
            }
            WriteInt32(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteStringArray(value[i]);
            }
        }
    }
}