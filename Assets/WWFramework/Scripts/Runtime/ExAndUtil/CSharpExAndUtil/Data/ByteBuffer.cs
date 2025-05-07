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
    /// byte缓存数据类,封装了byte数组,方便数据的读写
    /// </summary>
    public sealed class ByteBuffer
    {
        /// <summary>
        /// 缓存数据
        /// </summary>
        private byte[] _buffer;

        /// <summary>
        /// 数据指针(读取指针)
        /// </summary>
        public int ReadPointer;

        /// <summary>
        /// 数据指针(写入指针)
        /// </summary>
        public int WritePointer;
        
        public ByteBuffer(int capacity = 1024)
        {
            _buffer = new byte[capacity];
            ReadPointer = 0;
        }
        
        public ByteBuffer(byte[] buffer)
        {
            _buffer = buffer;
        }

        /// <summary>
        /// 获取未读取的数据
        /// </summary>
        /// <returns></returns>
        public byte[] CopyUnreadBytes()
        {
            var length = Mathf.Max(WritePointer - ReadPointer, 0);
            var bytes = new byte[length];
            Buffer.BlockCopy(_buffer, ReadPointer, bytes, 0, length);
            return bytes;
        }

        /// <summary>
        /// 检查写入指针
        /// </summary>
        /// <param name="newPointer">新指针位置</param>
        private void CheckWritePointer(int newPointer)
        {
            if (_buffer.Length < newPointer)
            {
                byte[] newBuffer = new byte[newPointer];
                Buffer.BlockCopy(_buffer, 0, newBuffer, 0, _buffer.Length);
                _buffer = newBuffer;
            }
        }

        /// <summary>
        /// 检查读取指针
        /// </summary>
        /// <param name="newPointer">新指针位置</param>
        /// <returns></returns>
        private bool CheckReadPointer(int newPointer)
        {
            return newPointer <= WritePointer;
        }
        
        public void WriteByteArray(byte[] data, int offset = 0, int length = -1)
        {
            if (length < 0)
            {
                length = data.Length;
            }
            WriteInt(length);
            CheckWritePointer(WritePointer + length);
            Buffer.BlockCopy(data, offset, _buffer, WritePointer, length);
        }

        public void WriteByte(byte data)
        {
            CheckWritePointer(WritePointer + 1);
            _buffer[WritePointer++] = data;
        }

        public void WriteSbyte(sbyte data)
        {
            CheckWritePointer(WritePointer + 1);
            _buffer[WritePointer++] = (byte)data;
        }
        
        public void WriteShort(short data)
        {
            CheckWritePointer(WritePointer + 2);
            _buffer[WritePointer++] = (byte)(data >> 8);
            _buffer[WritePointer++] = (byte)data;
        }
        
        public void WriteUShort(ushort data)
        {
            CheckWritePointer(WritePointer + 2);
            _buffer[WritePointer++] = (byte)(data >> 8);
            _buffer[WritePointer++] = (byte)data;
        }
        
        public void WriteInt(int data)
        {
            CheckWritePointer(WritePointer + 4);
            _buffer[WritePointer++] = (byte)(data >> 24);
            _buffer[WritePointer++] = (byte)(data >> 16);
            _buffer[WritePointer++] = (byte)(data >> 8);
            _buffer[WritePointer++] = (byte)data;
        }
        
        public void WriteUInt(uint data)
        {
            CheckWritePointer(WritePointer + 4);
            _buffer[WritePointer++] = (byte)(data >> 24);
            _buffer[WritePointer++] = (byte)(data >> 16);
            _buffer[WritePointer++] = (byte)(data >> 8);
            _buffer[WritePointer++] = (byte)data;
        }
        
        public void WriteLong(long data)
        {
            CheckWritePointer(WritePointer + 8);
            _buffer[WritePointer++] = (byte)(data >> 56);
            _buffer[WritePointer++] = (byte)(data >> 48);
            _buffer[WritePointer++] = (byte)(data >> 40);
            _buffer[WritePointer++] = (byte)(data >> 32);
            _buffer[WritePointer++] = (byte)(data >> 24);
            _buffer[WritePointer++] = (byte)(data >> 16);
            _buffer[WritePointer++] = (byte)(data >> 8);
            _buffer[WritePointer++] = (byte)data;
        }
        
        public void WriteULong(ulong data)
        {
            CheckWritePointer(WritePointer + 8);
            _buffer[WritePointer++] = (byte)(data >> 56);
            _buffer[WritePointer++] = (byte)(data >> 48);
            _buffer[WritePointer++] = (byte)(data >> 40);
            _buffer[WritePointer++] = (byte)(data >> 32);
            _buffer[WritePointer++] = (byte)(data >> 24);
            _buffer[WritePointer++] = (byte)(data >> 16);
            _buffer[WritePointer++] = (byte)(data >> 8);
            _buffer[WritePointer++] = (byte)data;
        }
        
        public void WriteFloat(float data)
        {
            CheckWritePointer(WritePointer + 4);
            var bytes = BitConverter.GetBytes(data);
            if (BitConverter.IsLittleEndian)
            {
                _buffer[WritePointer++] = bytes[0];
                _buffer[WritePointer++] = bytes[1];
                _buffer[WritePointer++] = bytes[2];
                _buffer[WritePointer++] = bytes[3];
            }
            else
            {
                _buffer[WritePointer++] = bytes[3];
                _buffer[WritePointer++] = bytes[2];
                _buffer[WritePointer++] = bytes[1];
                _buffer[WritePointer++] = bytes[0];
            }
        }
        
        public void WriteDouble(double data)
        {
            CheckWritePointer(WritePointer + 8);
            var bytes = BitConverter.GetBytes(data);
            if (BitConverter.IsLittleEndian)
            {
                _buffer[WritePointer++] = bytes[0];
                _buffer[WritePointer++] = bytes[1];
                _buffer[WritePointer++] = bytes[2];
                _buffer[WritePointer++] = bytes[3];
                _buffer[WritePointer++] = bytes[4];
                _buffer[WritePointer++] = bytes[5];
                _buffer[WritePointer++] = bytes[6];
                _buffer[WritePointer++] = bytes[7];
            }
            else
            {
                _buffer[WritePointer++] = bytes[7];
                _buffer[WritePointer++] = bytes[6];
                _buffer[WritePointer++] = bytes[5];
                _buffer[WritePointer++] = bytes[4];
                _buffer[WritePointer++] = bytes[3];
                _buffer[WritePointer++] = bytes[2];
                _buffer[WritePointer++] = bytes[1];
                _buffer[WritePointer++] = bytes[0];
            }
        }
        
        public void WriteString(string data, Encoding encoding = null)
        {
            if (null == encoding)
            {
                encoding = Encoding.UTF8;
            }
            var bytes = encoding.GetBytes(data);
            WriteByteArray(bytes);
        }

        public byte[] ReadByteArray()
        {
            var length = ReadInt();
            if (CheckReadPointer(ReadPointer + length))
            {
                return Array.Empty<byte>();
            }
            var bytes = new byte[length];
            Buffer.BlockCopy(_buffer, ReadPointer, bytes, 0, length);
            ReadPointer += length;
            return bytes;
        }
        
        public byte ReadByte()
        {
            if (CheckReadPointer(ReadPointer + 1))
            {
                return 0;
            }
            return _buffer[ReadPointer++];
        }
        
        public sbyte ReadSbyte()
        {
            if (CheckReadPointer(ReadPointer + 1))
            {
                return 0;
            }
            return (sbyte)_buffer[ReadPointer++];
        }
        
        public short ReadShort()
        {
            if (CheckReadPointer(ReadPointer + 2))
            {
                return 0;
            }
            return (short)(
                (_buffer[ReadPointer++] << 8) 
                | _buffer[ReadPointer++]
                );
        }
        
        public ushort ReadUShort()
        {
            if (CheckReadPointer(ReadPointer + 2))
            {
                return 0;
            }
            return (ushort)(
                (_buffer[ReadPointer++] << 8) 
                | _buffer[ReadPointer++]
                );
        }
        
        public int ReadInt()
        {
            if (CheckReadPointer(ReadPointer + 4))
            {
                return 0;
            }
            return (
                (_buffer[ReadPointer++] << 24) 
                | (_buffer[ReadPointer++] << 16) 
                | (_buffer[ReadPointer++] << 8) 
                | _buffer[ReadPointer++]
                );
        }
        
        public uint ReadUInt()
        {
            if (CheckReadPointer(ReadPointer + 4))
            {
                return 0;
            }
            return (
                (uint)_buffer[ReadPointer++] << 24 
                | (uint)_buffer[ReadPointer++] << 16 
                | (uint)_buffer[ReadPointer++] << 8 
                | (uint)_buffer[ReadPointer++]
                );
        }
        
        public long ReadLong()
        {
            if (CheckReadPointer(ReadPointer + 8))
            {
                return 0;
            }

            return (
                (long)_buffer[ReadPointer++] << 56
                | (long)_buffer[ReadPointer++] << 48
                | (long)_buffer[ReadPointer++] << 40
                | (long)_buffer[ReadPointer++] << 32
                | (long)_buffer[ReadPointer++] << 24
                | (long)_buffer[ReadPointer++] << 16
                | (long)_buffer[ReadPointer++] << 8
                | (long)_buffer[ReadPointer++]
            );
        }
        
        public ulong ReadULong()
        {
            if (CheckReadPointer(ReadPointer + 8))
            {
                return 0;
            }

            return (
                (ulong)_buffer[ReadPointer++] << 56
                | (ulong)_buffer[ReadPointer++] << 48
                | (ulong)_buffer[ReadPointer++] << 40
                | (ulong)_buffer[ReadPointer++] << 32
                | (ulong)_buffer[ReadPointer++] << 24
                | (ulong)_buffer[ReadPointer++] << 16
                | (ulong)_buffer[ReadPointer++] << 8
                | (ulong)_buffer[ReadPointer++]
            );
        }
        
        public float ReadFloat()
        {
            if (CheckReadPointer(ReadPointer + 4))
            {
                return 0;
            }
            
            var bytes = new byte[4];
            if (BitConverter.IsLittleEndian)
            {
                bytes[0] = _buffer[ReadPointer++];
                bytes[1] = _buffer[ReadPointer++];
                bytes[2] = _buffer[ReadPointer++];
                bytes[3] = _buffer[ReadPointer++];
            }
            else
            {
                bytes[3] = _buffer[ReadPointer++];
                bytes[2] = _buffer[ReadPointer++];
                bytes[1] = _buffer[ReadPointer++];
                bytes[0] = _buffer[ReadPointer++];
            }
            return BitConverter.ToSingle(bytes, 0);
        }
        
        public double ReadDouble()
        {
            if (CheckReadPointer(ReadPointer + 8))
            {
                return 0;
            }
            
            var bytes = new byte[8];
            if (BitConverter.IsLittleEndian)
            {
                bytes[0] = _buffer[ReadPointer++];
                bytes[1] = _buffer[ReadPointer++];
                bytes[2] = _buffer[ReadPointer++];
                bytes[3] = _buffer[ReadPointer++];
                bytes[4] = _buffer[ReadPointer++];
                bytes[5] = _buffer[ReadPointer++];
                bytes[6] = _buffer[ReadPointer++];
                bytes[7] = _buffer[ReadPointer++];
            }
            else
            {
                bytes[7] = _buffer[ReadPointer++];
                bytes[6] = _buffer[ReadPointer++];
                bytes[5] = _buffer[ReadPointer++];
                bytes[4] = _buffer[ReadPointer++];
                bytes[3] = _buffer[ReadPointer++];
                bytes[2] = _buffer[ReadPointer++];
                bytes[1] = _buffer[ReadPointer++];
                bytes[0] = _buffer[ReadPointer++];
            }
            return BitConverter.ToDouble(bytes, 0);
        }
        
        public string ReadString(Encoding encoding = null)
        {
            if (null == encoding)
            {
                encoding = Encoding.UTF8;
            }

            var bytes = ReadByteArray();
            return encoding.GetString(bytes);
        }
    }
}