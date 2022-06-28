using System.Net.Sockets;

namespace MzFrame
{
    public struct NetConfig
    {
        /// <summary>
        ///缓存数组字节数
        /// </summary>
        public int BufferSize;

        /// <summary>
        /// 每秒消息处理数量
        /// </summary>
        public ushort Tick;

        /// <summary>
        /// AddressFamily, 暂时没想好怎么封装
        /// </summary>
        public AddressFamily AddressFamily;

        public bool EnableEncryption;

        public static NetConfig Default
        {
            get => new NetConfig()
            {
                BufferSize = 2048,
                Tick = 20,
                AddressFamily = AddressFamily.InterNetwork, // IPV4
                EnableEncryption = false,
            };
        }
    }
}