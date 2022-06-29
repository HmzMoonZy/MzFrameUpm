using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;


namespace MzFrame
{
    /// <summary>
    /// 封装好的Socket, 用于连接服务器
    /// TODO 考虑接口实现
    /// </summary>
    public class MzSocket
    {
        public Action<NetErrMsg> OnNetErr;

        /// <summary>
        /// 接收到字节数组,MzNet会帮你切割包头,保证数组可用.
        /// </summary>
        public Func<byte[], NetMessage> OnReceiveBytes;

        /// <summary>
        /// 将 即将发送的 NetMessage 序列化为字符串(通常是json)
        /// </summary>
        public Func<NetMessage, string> OnSerialization;

        /// <summary>
        /// 缓存区
        /// </summary>
        private Byte[] _buffer;

        /// <summary>
        /// 读缓存区的尾指针, 表示缓存区中准备解析的数据长度.
        /// </summary>
        private int _bufferPoint = 0;

        /// <summary>
        /// 采用的配置
        /// </summary>
        private readonly NetConfig _config;

        /// <summary>
        /// 用于连接的Socket
        /// </summary>
        private readonly Socket _socket;

        private MsgDistribution _router;

        #region 构造方法

        public MzSocket(NetConfig config)
        {
            this._config = config;
            _buffer = new Byte[config.BufferSize];
            _socket = new Socket(config.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _router = new MsgDistribution(config.Tick);
        }
        
        public MzSocket(NetConfig config,  Func<byte[], NetMessage> receiveBytes) : this(config)
        {
            this.OnReceiveBytes = receiveBytes;
        }

        public MzSocket(NetConfig config, Func<byte[], NetMessage> receiveBytes, Func<NetMessage, string> serialization)
            : this(config, receiveBytes)
        {
            this.OnSerialization = serialization;
        }

        #endregion

        /// <summary>
        /// 连接服务器
        /// </summary>
        public void Connect(string ipAddress, int port, Action<bool> callback)
        {
            try
            {
                var ep = new IPEndPoint(IPAddress.Parse(ipAddress), port);

                // 连接
                _socket.Connect(ep);

                // 监听
                _socket.BeginReceive(_buffer, _bufferPoint, _buffer.Length - _bufferPoint, SocketFlags.None,
                    ReceiveCallback, _buffer);

                // 回调
                callback?.Invoke(true);
            }
            catch (Exception e)
            {
                OnNetErr?.Invoke(NetErrMsg.ConnectErr);
                callback?.Invoke(false);
            }
        }

        #region Send 发送函数

        /*public void Send(ref NetMessage msg)
        {
            var packet = NetPacker.ToPacket(ref msg);
            _socket.Send(packet);
        }

        public void Send(short op, short sub, string data)
        {
            var msg = new NetMessage() {op = op, sub = sub, data = data,};
            Send(ref msg);
        }

        public void Send(ref NetMessage msg, Func<NetMessage, string> process)
        {
            string str = process.Invoke(msg);
            SendWithPacket(str);
        }*/

        public void Send(ref NetMessage msg)
        {
            if (OnSerialization != null)
            {
                SendWithLength(OnSerialization.Invoke(msg));
            }
            else
            {
               _socket.Send(NetPacker.ToPacket(ref msg));
            }
        }

        /// <summary>
        /// 将 smg 转成字节数组后直接发送，不作任何修改
        /// </summary>
        /// <param name="msg"></param>
        public void SendWithoutPacket(string msg)
        {
            var msgBuffer = NetPacker.StringToBytes(msg);
            _socket.Send(msgBuffer);
        }

        public void SendWithLength(string msg)
        {
            var msgBuffer = NetPacker.StringToBytes(msg);
            _socket.Send(NetPacker.ConcatHeadWithLength(msgBuffer));
        }

        #endregion

        #region 事件

        public void Update()
        {
            _router.Update();
        }
        
        public void RegisterEvent(string token, Action<NetMessage> @event, bool triggerOnce = false)
        {
            if (triggerOnce)
            {
                _router.AddOnceListener(token, @event);
            }
            else
            {
                _router.AddListener(token, @event);
            }
        }

        public void DeregisterEvent(string token)
        {
            _router.DelListener(token);
        }
        

        #endregion

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            try
            {
                _socket.Close();
                _router.ClearAll();
                _buffer = null;
                _bufferPoint = -1;
            }
            catch (Exception e)
            {
                OnNetErr?.Invoke(NetErrMsg.CloseErr);
            }
        }

        /// <summary>
        /// 当有数据回调
        /// </summary>
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                int count = _socket.EndReceive(ar);
                _bufferPoint += count;

                ProcessPacket();

                // 持续监听
                _socket.BeginReceive(_buffer, _bufferPoint, _buffer.Length - _bufferPoint, SocketFlags.None,
                    ReceiveCallback, _buffer);
            }
            catch (Exception e)
            {
                OnNetErr?.Invoke(NetErrMsg.ReceiveErr);
            }
        }

        /// <summary>
        /// 处理消息缓存区的字节
        /// </summary>
        private void ProcessPacket()
        {
            var msgBuffer = NetPacker.UnPacket(_buffer, _bufferPoint, _config.EnableEncryption, out int len);
            if (msgBuffer == null) return;
            // 交付给路由
            _router.AddMsg(OnReceiveBytes.Invoke(msgBuffer));
            
            //将剩余未解析的消息复制到buffer首,并将指针提前
            int count = _bufferPoint - len;
            Array.Copy(_buffer, sizeof(int) + len, _buffer, 0, count);
            _bufferPoint = count;
            
            //继续解析剩余消息
            if (_bufferPoint > 0) ProcessPacket();
        }
    }
}