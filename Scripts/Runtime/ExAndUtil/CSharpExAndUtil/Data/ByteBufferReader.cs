/*------------------------------
 * 脚本名称: ByteBuffer
 * 创建者: movin
 * 创建日期: 2025/05/07
------------------------------*/

using System;

namespace WWFramework
{
    /// <summary>
    /// byte数据读取类
    /// <para> 提供各种整形，浮点型及布尔的读取，并支持了数组读取，当前最多支持二维数组 </para>
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

        /// <summary>
        /// 可变长度的32位无符号整型数字,用于根据数字大小动态使用不同长度的字节数存储数字。
        /// <para> 每个字节第一位为0表示没有后续字节，为1表示有后续字节 </para>
        /// <para> 1字节： 0-127 </para>
        /// <para> 2字节： 128-16383 </para>
        /// <para> 3字节： 16384-2097151 </para>
        /// <para> 4字节： 2097152-268435455 </para>
        /// </summary>
        /// <returns></returns>
        public uint ReadDynamicUInt32()
        {
            uint result = 0;

            for (int i = 0; i < 4; i++)
            {
                var pointer = Advance(1);
                var b = _buffer[pointer];
                result |= (uint)(b & 0x7F) << (i * 7);    // 后7位为有效数字，第一位为标志位
                if ((b >> 7) == 0)
                {
                    break;
                }
            }
            
            return result;
        }

        public bool ReadBool()
        {
            var pointer = Advance(1);
            return _buffer[pointer] != 0;
        }

        public byte ReadByte()
        {
            var pointer = Advance(1);
            return _buffer[pointer];
        }
        
        public sbyte ReadSbyte()
        {
            var pointer = Advance(1);
            return (sbyte)_buffer[pointer];
        }

        public short ReadInt16()
        {
            var pointer = Advance(2);
            return (short)(_buffer[pointer] | 
                           (_buffer[pointer + 1] << 8));
        }
        
        public ushort ReadUInt16()
        {
            var pointer = Advance(2);
            return (ushort)(_buffer[pointer] |
                            (_buffer[pointer + 1] << 8));
        }
        
        public int ReadInt32()
        {
            var pointer = Advance(4);
            return _buffer[pointer] | 
                   (_buffer[pointer + 1] << 8) |
                   (_buffer[pointer + 2] << 16) |
                   (_buffer[pointer + 3] << 24);
        }
        
        public uint ReadUInt32()
        {
            var pointer = Advance(4);
            return (uint)(_buffer[pointer] |
                          (_buffer[pointer + 1] << 8) | 
                          (_buffer[pointer + 2] << 16) |
                          (_buffer[pointer + 3] << 24));
        }

        public long ReadInt64()
        {
            var pointer = Advance(8);
            return (long)_buffer[pointer] | 
                   ((long)_buffer[pointer + 1] << 8) | 
                   ((long)_buffer[pointer + 2] << 16) | 
                   ((long)_buffer[pointer + 3] << 24) |
                   ((long)_buffer[pointer + 4] << 32) | 
                   ((long)_buffer[pointer + 5] << 40) |
                   ((long)_buffer[pointer + 6] << 48) | 
                   ((long)_buffer[pointer + 7] << 56);
        }

        public ulong ReadUInt64()
        {
            var pointer = Advance(8);
            return (ulong)_buffer[pointer] | 
                   ((ulong)_buffer[pointer + 1] << 8) |
                   ((ulong)_buffer[pointer + 2] << 16) | 
                   ((ulong)_buffer[pointer + 3] << 24) |
                   ((ulong)_buffer[pointer + 4] << 32) |
                   ((ulong)_buffer[pointer + 5] << 40) |
                   ((ulong)_buffer[pointer + 6] << 48) |
                   ((ulong)_buffer[pointer + 7] << 56);
        }

        public unsafe float ReadSingle()
        {
            var pointer = Advance(4);
            var num = (uint)(_buffer[pointer] |
                          (_buffer[pointer + 1] << 8) | 
                          (_buffer[pointer + 2] << 16) |
                          (_buffer[pointer + 3] << 24));
            return *(float*)&num;
        }
        
        public unsafe double ReadDouble()
        {
            var pointer = Advance(8);
            var num = (ulong)_buffer[pointer] |
                             ((ulong)_buffer[pointer + 1] << 8) |
                             ((ulong)_buffer[pointer + 2] << 16) |
                             ((ulong)_buffer[pointer + 3] << 24) |
                             ((ulong)_buffer[pointer + 4] << 32) |
                             ((ulong)_buffer[pointer + 5] << 40) |
                             ((ulong)_buffer[pointer + 6] << 48) |
                             ((ulong)_buffer[pointer + 7] << 56);
            return *(double*)&num;
        }

        public void ReadString(out string value)
        {
            var length = ReadUInt16();
            var pointer = Advance(length);
            value = System.Text.Encoding.UTF8.GetString(_buffer, pointer, length);
        }

        public void ReadByteArray(out byte[] value)
        {
            var length = (int)ReadDynamicUInt32();
            var pointer = Advance(length);
            value = new byte[length];
            Buffer.BlockCopy(_buffer, pointer, value, 0, length);
        }

        public void ReadSByteArray(out sbyte[] value)
        {
            var length = (int)ReadDynamicUInt32();
            var pointer = Advance(length);
            value = new sbyte[length];
            for (var i = 0; i < length; i++)
            {
                value[i] = (sbyte)_buffer[pointer + i];
            }
        }
        
        public void ReadInt16Array(out short[] value)
        {
            var length = (int)ReadDynamicUInt32();
            var pointer = Advance(length * 2);
            value = new short[length];
            for (var i = 0; i < length; i++)
            {
                var shortValue = (short)(_buffer[pointer + i * 2] |
                                         _buffer[pointer + i * 2 + 1] << 8);
                value[i] = shortValue;
            }
        }
        
        public void ReadUInt16Array(out ushort[] value)
        { 
            var length = (int)ReadDynamicUInt32();
            var pointer = Advance(length * 2);
            value = new ushort[length];
            for (var i = 0; i < length; i++)
            {
                var ushortValue = (ushort)(_buffer[pointer + i * 2] |
                                           _buffer[pointer + i * 2 + 1] << 8);
                value[i] = ushortValue;
            }
        }

        public void ReadInt32Array(out int[] value)
        {
            var length = (int)ReadDynamicUInt32();
            var pointer = Advance(length * 4);
            value = new int[length];
            for (var i = 0; i < length; i++)
            {
                var intValue = _buffer[pointer + i * 4] | 
                               _buffer[pointer + i * 4 + 1] << 8 |
                               _buffer[pointer + i * 4 + 2] << 16 |
                               _buffer[pointer + i * 4 + 3] << 24;
                value[i] = intValue;
            }
        }
        
        public void ReadUInt32Array(out uint[] value)
        {
            var length = (int)ReadDynamicUInt32();
            var pointer = Advance(length * 4);
            value = new uint[length];
            for (var i = 0; i < length; i++)
            {
                var uintValue = (uint)(_buffer[pointer] |
                                       (_buffer[pointer + 1] << 8) |
                                       (_buffer[pointer + 2] << 16) |
                                       (_buffer[pointer + 3] << 24));
                value[i] = uintValue;
            }
        }

        public void ReadInt64Array(out long[] value)
        {
            var length = (int)ReadDynamicUInt32();
            var pointer = Advance(length * 8);
            value = new long[length];
            for (var i = 0; i < length; i++)
            {
                var longValue = (long)_buffer[pointer] | 
                                ((long)_buffer[pointer + 1] << 8) | 
                                ((long)_buffer[pointer + 2] << 16) | 
                                ((long)_buffer[pointer + 3] << 24) |
                                ((long)_buffer[pointer + 4] << 32) | 
                                ((long)_buffer[pointer + 5] << 40) |
                                ((long)_buffer[pointer + 6] << 48) | 
                                ((long)_buffer[pointer + 7] << 56);
                value[i] = longValue;
            }
        }
        
        public void ReadUInt64Array(out ulong[] value)
        {
            var length = (int)ReadDynamicUInt32();
            var pointer = Advance(length * 8);
            value = new ulong[length];
            for (var i = 0; i < length; i++)
            {
                var ulongValue = (ulong)_buffer[pointer] |
                              ((ulong)_buffer[pointer + 1] << 8) |
                              ((ulong)_buffer[pointer + 2] << 16) |
                              ((ulong)_buffer[pointer + 3] << 24) |
                              ((ulong)_buffer[pointer + 4] << 32) |
                              ((ulong)_buffer[pointer + 5] << 40) |
                              ((ulong)_buffer[pointer + 6] << 48) |
                              ((ulong)_buffer[pointer + 7] << 56);
                value[i] = ulongValue;
            }
        }

        public unsafe void ReadSingleArray(out float[] value)
        {
            var length = (int)ReadDynamicUInt32();
            var pointer = Advance(length * 4);
            value = new float[length];
            for (var i = 0; i < length; i++)
            {
                var uintValue = (uint)(_buffer[pointer] |
                                       (_buffer[pointer + 1] << 8) |
                                       (_buffer[pointer + 2] << 16) |
                                       (_buffer[pointer + 3] << 24));
                value[i] = *(float*)&uintValue;
            }
        }
        
        public unsafe void ReadDoubleArray(out double[] value)
        {
            var length = (int)ReadDynamicUInt32();
            var pointer = Advance(length * 8);
            value = new double[length];
            for (var i = 0; i < length; i++)
            {
                var ulongValue = (ulong)_buffer[pointer] |
                                 ((ulong)_buffer[pointer + 1] << 8) |
                                 ((ulong)_buffer[pointer + 2] << 16) |
                                 ((ulong)_buffer[pointer + 3] << 24) |
                                 ((ulong)_buffer[pointer + 4] << 32) |
                                 ((ulong)_buffer[pointer + 5] << 40) |
                                 ((ulong)_buffer[pointer + 6] << 48) |
                                 ((ulong)_buffer[pointer + 7] << 56);
                value[i] = *(double*)&ulongValue;
            }
        }

        public void ReadBoolArray(out bool[] value)
        {
            var length = (int)ReadDynamicUInt32();
            var pointer = Advance(length / 8 + (length % 8 == 0 ? 0 : 1));
            value = new bool[length];
            byte b = 0;
            for (int i = 0; i < length; i++)
            {
                if (i % 8 == 0)
                {
                    b = _buffer[pointer + i / 8];
                }
                value[i] = (b & (1 << (i % 8))) != 0;
            }
        }

        public void ReadStringArray(out string[] value)
        {
            var length = (int)ReadDynamicUInt32();
            value = new string[length];
            for (int i = 0; i < length; i++)
            {
                ReadString(out value[i]);
            }
        }

        public void ReadByteArrayArray(out byte[][] value)
        {
            var length = (int)ReadDynamicUInt32();
            value = new byte[length][];
            for (int i = 0; i < length; i++)
            {
                ReadByteArray(out value[i]);
            }
        }

        public void ReadSByteArrayArray(out sbyte[][] value)
        {
            var length = (int)ReadDynamicUInt32();
            value = new sbyte[length][];
            for (int i = 0; i < length; i++)
            {
                ReadSByteArray(out value[i]);
            }
        }
        
        public void ReadInt16ArrayArray(out short[][] value)
        {
            var length = (int)ReadDynamicUInt32();
            value = new short[length][];
            for (int i = 0; i < length; i++)
            {
                ReadInt16Array(out value[i]);
            }
        }
        
        public void ReadUInt16ArrayArray(out ushort[][] value)
        {
            var length = (int)ReadDynamicUInt32();
            value = new ushort[length][];
            for (int i = 0; i < length; i++)
            {
                ReadUInt16Array(out value[i]);
            }
        }
        
        public void ReadInt32ArrayArray(out int[][] value)
        {
            var length = (int)ReadDynamicUInt32();
            value = new int[length][];
            for (int i = 0; i < length; i++)
            {
                ReadInt32Array(out value[i]);
            }
        }

        public void ReadUInt32ArrayArray(out uint[][] value)
        {
            var length = (int)ReadDynamicUInt32();
            value = new uint[length][];
            for (int i = 0; i < length; i++)
            {
                ReadUInt32Array(out value[i]);
            }
        }
        
        public void ReadInt64ArrayArray(out long[][] value)
        {
            var length = (int)ReadDynamicUInt32();
            value = new long[length][];
            for (int i = 0; i < length; i++)
            {
                ReadInt64Array(out value[i]);
            }
        }
        
        public void ReadUInt64ArrayArray(out ulong[][] value)
        {
            var length = (int)ReadDynamicUInt32();
            value = new ulong[length][];
            for (int i = 0; i < length; i++)
            {
                ReadUInt64Array(out value[i]);
            }
        }

        public void ReadSingleArrayArray(out float[][] value)
        {
            var length = (int)ReadDynamicUInt32();
            value = new float[length][];
            for (int i = 0; i < length; i++)
            {
                ReadSingleArray(out value[i]);
            }
        }
        
        public void ReadDoubleArrayArray(out double[][] value)
        {
            var length = (int)ReadDynamicUInt32();
            value = new double[length][];
            for (int i = 0; i < length; i++)
            {
                ReadDoubleArray(out value[i]);
            }
        }

        public void ReadBoolArrayArray(out bool[][] value)
        {
            var length = (int)ReadDynamicUInt32();
            value = new bool[length][];
            for (int i = 0; i < length; i++)
            {
                ReadBoolArray(out value[i]);
            }
        }

        public void ReadStringArrayArray(out string[][] value)
        {
            var length = (int)ReadDynamicUInt32();
            value = new string[length][];
            for (int i = 0; i < length; i++)
            {
                ReadStringArray(out value[i]);
            }
        }
    }
}