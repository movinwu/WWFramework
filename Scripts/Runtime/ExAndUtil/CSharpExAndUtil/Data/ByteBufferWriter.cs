/*------------------------------
 * 脚本名称: ByteBuffer
 * 创建者: movin
 * 创建日期: 2025/05/07
------------------------------*/

using System;
using System.Text;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// byte数据写入类
    /// <para> 提供各种整形，浮点型及布尔的写入，并支持了数组写入，当前最多支持二维数组 </para>
    /// </summary>
    public sealed class ByteBufferWriter
    {
        /// <summary>
        /// 缓存数据
        /// </summary>
        private byte[] _buffer;

        /// <summary>
        /// 数据指针(写入指针)
        /// </summary>
        private int _writePointer;

        /// <summary>
        /// 数据指针(写入指针)
        /// </summary>
        public int WritePointer
        {
            get
            {
                return _writePointer;
            }
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

        /// <summary>
        /// 可变长度的32位无符号整型数字,用于根据数字大小动态使用不同长度的字节数写入数字。
        /// <para> 每个字节第一位为0表示没有后续字节，为1表示有后续字节 </para>
        /// <para> 1字节： 0-127 </para>
        /// <para> 2字节： 128-16383 </para>
        /// <para> 3字节： 16384-2097151 </para>
        /// <para> 4字节： 2097152-268435455 </para>
        /// </summary>
        /// <returns></returns>
        public void WriteDynamicUInt32(uint data)
        {
            // 存储字节数
            int byteCount;
            if (data > 2097151)
            {
                byteCount = 4;
            }
            else if (data > 16383)
            {
                byteCount = 3;
            }
            else if (data > 127)
            {
                byteCount = 2;
            }
            else
            {
                byteCount = 1;
            }
            
            for (int i = 0; i < byteCount; i++)
            {
                byte b = (byte)(data >> (i * 7));
                if (i == byteCount - 1)
                {
                    b &= 0x7F;
                }
                else
                {
                    b |= 0x80;
                }

                var pointer = Advance(1);
                _buffer[pointer] = b;
            }
        }

        public void WriteBool(bool data)
        {
            _buffer[Advance(1)] = data ? (byte)1 : (byte)0;
        }

        public void WriteByte(byte data)
        {
            var pointer = Advance(1);
            _buffer[pointer] = data;
        }
        
        public void WriteSbyte(sbyte data)
        {
            var pointer = Advance(1);
            _buffer[pointer] = (byte)data;
        }

        public void WriteInt16(short data)
        {
            var pointer = Advance(2);
            _buffer[pointer] = (byte)data;
            _buffer[pointer + 1] = (byte)(data >> 8);
        }
        
        public void WriteUInt16(ushort data)
        {
            var pointer = Advance(2);
            _buffer[pointer] = (byte)data;
            _buffer[pointer + 1] = (byte)(data >> 8);
        }
        
        public void WriteInt32(int data)
        {
            var pointer = Advance(4);
            _buffer[pointer] = (byte)data;
            _buffer[pointer + 1] = (byte)(data >> 8);
            _buffer[pointer + 2] = (byte)(data >> 16);
            _buffer[pointer + 3] = (byte)(data >> 24);
        }
        
        public void WriteUInt32(uint data)
        {
            var pointer = Advance(4);
            _buffer[pointer] = (byte)data;
            _buffer[pointer + 1] = (byte)(data >> 8);
            _buffer[pointer + 2] = (byte)(data >> 16);
            _buffer[pointer + 3] = (byte)(data >> 24);
        }

        public void WriteInt64(long data)
        {
            var pointer = Advance(8);
            _buffer[pointer] = (byte)data;
            _buffer[pointer + 1] = (byte)(data >> 8);
            _buffer[pointer + 2] = (byte)(data >> 16);
            _buffer[pointer + 3] = (byte)(data >> 24);
            _buffer[pointer + 4] = (byte)(data >> 32);
            _buffer[pointer + 5] = (byte)(data >> 40);
            _buffer[pointer + 6] = (byte)(data >> 48);
            _buffer[pointer + 7] = (byte)(data >> 56);
        }

        public void WriteUInt64(ulong data)
        {
            var pointer = Advance(8);
            _buffer[pointer] = (byte)data;
            _buffer[pointer + 1] = (byte)(data >> 8);
            _buffer[pointer + 2] = (byte)(data >> 16);
            _buffer[pointer + 3] = (byte)(data >> 24);
            _buffer[pointer + 4] = (byte)(data >> 32);
            _buffer[pointer + 5] = (byte)(data >> 40);
            _buffer[pointer + 6] = (byte)(data >> 48);
            _buffer[pointer + 7] = (byte)(data >> 56);
        }

        public unsafe void WriteSingle(float data)
        {
            var pointer = Advance(4);
            var num = *(uint*)&data;
            _buffer[pointer] = (byte)num;
            _buffer[pointer + 1] = (byte)(num >> 8);
            _buffer[pointer + 2] = (byte)(num >> 16);
            _buffer[pointer + 3] = (byte)(num >> 24);
        }
        
        public unsafe void WriteDouble(double data)
        {
            var pointer = Advance(8);
            var num = *(ulong*)&data;
            _buffer[pointer] = (byte)num;
            _buffer[pointer + 1] = (byte)(num >> 8);
            _buffer[pointer + 2] = (byte)(num >> 16);
            _buffer[pointer + 3] = (byte)(num >> 24);
            _buffer[pointer + 4] = (byte)(num >> 32);
            _buffer[pointer + 5] = (byte)(num >> 40);
            _buffer[pointer + 6] = (byte)(num >> 48);
            _buffer[pointer + 7] = (byte)(num >> 56);
        }

        public void WriteString(string value)
        {
            var pointer = Advance(2 + value.Length * 4);  // 使用UTF-8编码时，一个字符可能占用4个字节，这里保证最坏情况下也不会越界
            var length = System.Text.Encoding.UTF8.GetBytes(value, 0, value.Length, _buffer, pointer + 2);
            // 使用ushort存储长度，最多支持65535字节字符串长度
            _buffer[pointer] = (byte)length;
            _buffer[pointer + 1] = (byte)(length >> 8);
            // 重置已经不正确的写入指针
            _writePointer = pointer + 2 + length;
        }

        public void WriteByteArray(byte[] value)
        {
            if (null == value || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            var pointer = Advance(value.Length);
            Buffer.BlockCopy(value, 0, _buffer, pointer, value.Length);
        }

        public void WriteSByteArray(sbyte[] value)
        {
            if (null == value || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            var pointer = Advance(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                _buffer[pointer + i] = (byte)value[i];
            }
        }
        
        public void WriteInt16Array(short[] value)
        {
            if (null == value || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            var pointer = Advance(value.Length * 2);
            for (int i = 0; i < value.Length; i++)
            {
                var v = value[i];
                _buffer[pointer + i * 2] = (byte)v;
                _buffer[pointer + i * 2 + 1] = (byte)(v >> 8);
            }
        }
        
        public void WriteUInt16Array(ushort[] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            var pointer = Advance(value.Length * 2);
            for (int i = 0; i < value.Length; i++)
            {
                var v = value[i];
                _buffer[pointer + i * 2] = (byte)v;
                _buffer[pointer + i * 2 + 1] = (byte)(v >> 8);
            }
        }

        public void WriteInt32Array(int[] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            var pointer = Advance(value.Length * 4);
            for (int i = 0; i < value.Length; i++)
            {
                var v = value[i];
                _buffer[pointer + i * 4] = (byte)v;
                _buffer[pointer + i * 4 + 1] = (byte)(v >> 8);
                _buffer[pointer + i * 4 + 2] = (byte)(v >> 16);
                _buffer[pointer + i * 4 + 3] = (byte)(v >> 24);
            }
        }
        
        public void WriteUInt32Array(uint[] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            var pointer = Advance(value.Length * 4);
            for (int i = 0; i < value.Length; i++)
            {
                var v = value[i];
                _buffer[pointer + i * 4] = (byte)v;
                _buffer[pointer + i * 4 + 1] = (byte)(v >> 8);
                _buffer[pointer + i * 4 + 2] = (byte)(v >> 16);
                _buffer[pointer + i * 4 + 3] = (byte)(v >> 24);
            }
        }

        public void WriteInt64Array(long[] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            var pointer = Advance(value.Length * 8);
            for (int i = 0; i < value.Length; i++)
            {
                var v = value[i];
                _buffer[pointer + i * 8] = (byte)v;
                _buffer[pointer + i * 8 + 1] = (byte)(v >> 8);
                _buffer[pointer + i * 8 + 2] = (byte)(v >> 16);
                _buffer[pointer + i * 8 + 3] = (byte)(v >> 24);
                _buffer[pointer + i * 8 + 4] = (byte)(v >> 32);
                _buffer[pointer + i * 8 + 5] = (byte)(v >> 40);
                _buffer[pointer + i * 8 + 6] = (byte)(v >> 48);
                _buffer[pointer + i * 8 + 7] = (byte)(v >> 56);
            }
        }
        
        public void WriteUInt64Array(ulong[] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            var pointer = Advance(value.Length * 8);
            for (int i = 0; i < value.Length; i++)
            {
                var v = value[i];
                _buffer[pointer + i * 8] = (byte)v;
                _buffer[pointer + i * 8 + 1] = (byte)(v >> 8);
                _buffer[pointer + i * 8 + 2] = (byte)(v >> 16);
                _buffer[pointer + i * 8 + 3] = (byte)(v >> 24);
                _buffer[pointer + i * 8 + 4] = (byte)(v >> 32);
                _buffer[pointer + i * 8 + 5] = (byte)(v >> 40);
                _buffer[pointer + i * 8 + 6] = (byte)(v >> 48);
                _buffer[pointer + i * 8 + 7] = (byte)(v >> 56);
            }
        }

        public unsafe void WriteSingleArray(float[] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            var pointer = Advance(value.Length * 4);
            for (int i = 0; i < value.Length; i++)
            {
                var fv = value[i];
                var v = *(uint*)&fv;
                _buffer[pointer + i * 4] = (byte)v;
                _buffer[pointer + i * 4 + 1] = (byte)(v >> 8);
                _buffer[pointer + i * 4 + 2] = (byte)(v >> 16);
                _buffer[pointer + i * 4 + 3] = (byte)(v >> 24);
            }
        }
        
        public unsafe void WriteDoubleArray(double[] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            var pointer = Advance(value.Length * 8);
            for (int i = 0; i < value.Length; i++)
            {
                var dv = value[i];
                var v = *(ulong*)&dv;
                _buffer[pointer + i * 8] = (byte)v;
                _buffer[pointer + i * 8 + 1] = (byte)(v >> 8);
                _buffer[pointer + i * 8 + 2] = (byte)(v >> 16);
                _buffer[pointer + i * 8 + 3] = (byte)(v >> 24);
                _buffer[pointer + i * 8 + 4] = (byte)(v >> 32);
                _buffer[pointer + i * 8 + 5] = (byte)(v >> 40);
                _buffer[pointer + i * 8 + 6] = (byte)(v >> 48);
                _buffer[pointer + i * 8 + 7] = (byte)(v >> 56);
            }
        }

        public void WriteBoolArray(bool[] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            byte b = 0;
            for (int i = 0; i < value.Length; i++)
            {
                var v = value[i];
                if (v)
                {
                    b |= (byte)(1 << (i % 8));
                }

                if (i % 8 == 7 || i == value.Length - 1)
                {
                    var pointer = Advance(1);
                    _buffer[pointer] = b;
                    b = 0;
                }
            }
        }

        public void WriteStringArray(string[] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteString(value[i]);
            }
        }

        public void WriteByteArrayArray(byte[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteByteArray(value[i]);
            }
        }

        public void WriteSByteArrayArray(sbyte[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteSByteArray(value[i]);
            }
        }
        
        public void WriteInt16ArrayArray(short[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteInt16Array(value[i]);
            }
        }
        
        public void WriteUInt16ArrayArray(ushort[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteUInt16Array(value[i]);
            }
        }
        
        public void WriteInt32ArrayArray(int[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteInt32Array(value[i]);
            }
        }

        public void WriteUInt32ArrayArray(uint[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteUInt32Array(value[i]);
            }
        }
        
        public void WriteInt64ArrayArray(long[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteInt64Array(value[i]);
            }
        }
        
        public void WriteUInt64ArrayArray(ulong[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteUInt64Array(value[i]);
            }
        }

        public void WriteSingleArrayArray(float[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteSingleArray(value[i]);
            }
        }
        
        public void WriteDoubleArrayArray(double[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteDoubleArray(value[i]);
            }
        }

        public void WriteBoolArrayArray(bool[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteBoolArray(value[i]);
            }
        }

        public void WriteStringArrayArray(string[][] value)
        {
            if (value == null || value.Length == 0)
            {
                WriteDynamicUInt32(0);
                return;
            }
            WriteDynamicUInt32((uint)value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteStringArray(value[i]);
            }
        }
    }
}