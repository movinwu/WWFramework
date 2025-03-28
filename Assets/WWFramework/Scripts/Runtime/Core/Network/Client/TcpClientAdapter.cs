/*------------------------------
 * 脚本名称: TcpClient
 * 创建者: movin
 * 创建日期: 2025/03/26
------------------------------*/

using System;
using System.Net;
using Cysharp.Threading.Tasks;
using NetCoreServer;

namespace WWFramework
{
    /// <summary>
    /// tcp客户端连接
    /// </summary>
    public class TcpClientAdapter : TcpClient, IClientAdapter
    {
        /// <summary>
        /// 默认缓冲区长度
        /// </summary>
        private const int DefaultBufferLength = 128;
        
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
        

        /// <inheritdoc/>
        public Action<byte[], int> OnPacketReceived { get; set; }

        /// <inheritdoc/>
        public Action OnUnexpectedDisconnect { get; set; }

        /// <summary>
        /// 是否客户端发起断开连接
        /// </summary>
        private bool _clientDisconnected;

        /// <inheritdoc/>
        public TcpClientAdapter(IPAddress address, int port) : base(address, port)
        {
        }

        /// <inheritdoc/>
        public TcpClientAdapter(string address, int port) : base(address, port)
        {
        }

        /// <inheritdoc/>
        public TcpClientAdapter(IPEndPoint endpoint) : base(endpoint)
        {
        }

        protected override void OnDisconnected()
        {
            base.OnDisconnected();
            OnUnexpectedDisconnect?.Invoke();
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
                    Array.Copy(_buffer, _bufferPointer, newBuffer, 0, _bufferSize);
                }
                _buffer = newBuffer;
                _bufferPointer = 0;
            }
            // 将新接收到的数据复制到缓冲区
            if (_bufferPointer + _bufferSize + size >= _buffer.Length)
            {
                if (_bufferPointer + _bufferSize >= _buffer.Length)
                {
                    Array.Copy(buffer, (int)offset, _buffer, _bufferPointer + _bufferSize - _buffer.Length, (int)size);
                }
                else
                {
                    var length1 = _buffer.Length - _bufferPointer - _bufferSize;
                    var length2 = size - length1;
                    Array.Copy(buffer, (int)offset, _buffer, _bufferPointer + _bufferSize, length1);
                    Array.Copy(buffer, (int)offset + length1, _buffer, 0, length2);
                }
            }
            else
            {
                Array.Copy(buffer, (int)offset, _buffer, _bufferPointer, (int)size);
            }
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
                            _packetLength |= (_buffer[index] << (24 - i * 8));
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
                    if (_packetLength > _packet.Length)
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

        /// <inheritdoc/>
        public async UniTask<bool> AsyncConnect(int connectTime)
        {
            if (this.ConnectAsync())
            {
                var connectTask = UniTask.WaitUntil(() => this.IsConnected);
                var timeoutTask = UniTask.Delay(connectTime);
                await UniTask.WhenAny(connectTask, timeoutTask);
                if (this.IsConnected)
                {
                    Log.LogDebug(sb =>
                    {
                        sb.Append("网络连接成功");
                    }, ELogType.Network);
                    return true;
                }
                else
                {
                    Log.LogError(sb =>
                    {
                        sb.Append("网络连接超时");
                    }, ELogType.Network);
                    // 确保断开连接
                    await AsyncDisconnect();

                    return false;
                }
            }
            else
            {
                Log.LogError(sb =>
                {
                    sb.Append("发起网络连接失败");
                }, ELogType.Network);
                return false;
            }
        }

        /// <inheritdoc/>
        public async UniTask<bool> AsyncSend(byte[] buffer)
        {
            if (SendAsync(buffer))
            {
                return true;
            }

            return false;
        }
        
        /// <inheritdoc/>
        public async UniTask<bool> AsyncReconnect(int reconnectTime)
        {
            // 确保断开连接
            await AsyncDisconnect();
            // 重新发起连接
            await AsyncConnect(reconnectTime);
            
            if (this.IsConnected)
            {
                Log.LogDebug(sb =>
                {
                    sb.Append("网络重连成功");
                }, ELogType.Network);
                return true;
            }

            // 确保断开连接
            await AsyncDisconnect();
            return false;
        }
        
        /// <inheritdoc/>
        public async UniTask<bool> AsyncDisconnect()
        {
            if (this.DisconnectAsync())
            {
                await UniTask.WaitUntil(() => !this.IsConnected && !this.IsConnecting);
            }
            return !this.IsConnected && !this.IsConnecting;
        }
    }
}
