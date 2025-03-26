/*------------------------------
 * 脚本名称: TcpClient
 * 创建者: movin
 * 创建日期: 2025/03/26
------------------------------*/

using System;
using System.Net;

namespace WWFramework
{
    /// <summary>
    /// tcp客户端连接
    /// </summary>
    public class TcpClient : NetCoreServer.TcpClient
    {
        /// <summary>
        /// 默认缓冲区长度
        /// </summary>
        private const int DefaultBufferLength = 1024 * 4;
        
        /// <summary>
        /// 接收缓冲区
        /// </summary>
        private byte[] _buffer = new byte[DefaultBufferLength];
        /// <summary>
        /// 缓冲区指针头
        /// </summary>
        private int _bufferPointer = 0;
        /// <summary>
        /// 当前缓冲区容量
        /// </summary>
        private int _bufferSize = 0;
        /// <summary>
        /// 数据包长度
        /// </summary>
        private int _packetLength = -1;
        /// <summary>
        /// 数据包
        /// </summary>
        private byte[] _packet = new byte[DefaultBufferLength];
        
        /// <summary>
        /// 数据包接收事件
        /// </summary>
        public System.Action<byte[], int> OnPacketReceived;

        /// <inheritdoc/>
        public TcpClient(IPAddress address, int port) : base(address, port)
        {
        }

        /// <inheritdoc/>
        public TcpClient(string address, int port) : base(address, port)
        {
        }

        /// <inheritdoc/>
        public TcpClient(IPEndPoint endpoint) : base(endpoint)
        {
        }

        /// <inheritdoc/>
        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            // 处理粘包/半包问题,目前使用4字节长度前缀解决
            // 是否需要扩容缓冲区
            if (_bufferSize + size > _buffer.Length)
            {
                // 扩容为原来的两倍
                byte[] newBuffer = new byte[_buffer.Length * 2];
                // 拷贝当前缓冲区数据
                if (_bufferPointer + _bufferSize >= _buffer.Length)
                {
                    var length1 = _buffer.Length - _bufferPointer;
                    var length2 = _bufferSize - length1;
                    Array.Copy(_buffer, _bufferPointer, newBuffer, 0, length1);
                    Array.Copy(_buffer, 0, newBuffer, length1, length2);
                }
                else
                {
                    Array.Copy(_buffer, _bufferPointer, newBuffer, 0, _buffer.Length - _bufferPointer);
                }
                _buffer = newBuffer;
                _bufferPointer = 0;
            }
            // 将新接收到的数据复制到缓冲区
            Array.Copy(buffer, (int)offset, _buffer, _bufferSize, (int)size);
            _bufferSize += (int)size;
            // 检查包头,包头检查完成后会自动检查包体
            CheckPacketHead();
            
            base.OnReceived(buffer, offset, size);

            // 检查包头
            void CheckPacketHead()
            {
                // 判断是否数据长度已经赋值(包头是否已经赋值)
                if (_packetLength >= 0)
                {
                    // 包头已经赋值,检查包体
                    CheckPacketBody();
                }
                else
                {
                    // 包头没有赋值,检查数据长度
                    if (_bufferSize >= 4)
                    {
                        // 读取数据长度
                        _packetLength = 0;
                        for (int i = 0; i < 4; i++)
                        {
                            var index = (_bufferPointer + i) % _buffer.Length;
                            _packetLength |= (_buffer[index] << (i * 8));
                        }
                        MovePointer(4);
                        // 检查包体
                        CheckPacketBody();
                    }
                }
            }
            
            // 检查包体
            void CheckPacketBody()
            {
                // 数据长度已经赋值,判断是否接收到完整的数据包
                if (_bufferSize >= _packetLength)
                {
                    // 检查数据包长度是否足够
                    if (_packetLength > _buffer.Length)
                    {
                        _packet = new byte[_packetLength];
                    }
                    // 从缓冲区中读取数据包
                    if (_bufferPointer + _packetLength >= _buffer.Length)
                    {
                        var length1 = _buffer.Length - _bufferPointer;
                        var length2 = _packetLength - length1;
                        Array.Copy(_buffer, _bufferPointer, _packet, 0, length1);
                        Array.Copy(_buffer, 0, _packet, length1, length2);
                    }
                    else
                    {
                        Array.Copy(_buffer, _bufferPointer, _packet, 0, _packetLength);
                    }
                    // 处理数据包
                    OnPacketReceived?.Invoke(_packet, _packetLength);
                    // 更新缓冲区偏移量和数据长度
                    MovePointer(_packetLength);
                    _packetLength = -1;
                    // 重新检查包头
                    CheckPacketHead();
                }
            }
            
            // 移动缓冲区指针
            void MovePointer(int moveLen)
            {
                if (moveLen > _bufferSize)
                {
                    throw new Exception("缓冲区指针越界");
                }
                _bufferSize -= moveLen;
                _bufferPointer += moveLen;
                _bufferPointer %= _buffer.Length;
            }
        }
    }
}
