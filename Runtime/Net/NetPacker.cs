using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace MzFrame
{
    /// <summary>
    /// 网络包相关
    /// TODO 分析拆装箱和内存占用
    /// </summary>
    public static class NetPacker
    {
        #region 加密字符串

        /// <summary>
        /// 简单加密
        /// </summary>
        public static string Encryption(string str)
        {
            // 转 base64
            str = Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
            // 反转字符串
            StringBuilder sb = new StringBuilder(str.Length);
            foreach (var c in str.Reverse())
            {
                sb.Append(c);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 解密
        /// </summary>
        public static string Decrypt(string str)
        {
            StringBuilder sb = new StringBuilder(str.Length);
            foreach (var c in str.Reverse())
            {
                sb.Append(c);
            }

            var bytes = Convert.FromBase64String(sb.ToString());
            return Encoding.UTF8.GetString(bytes);
        }

        #endregion

        #region TCP 装包

        /// <summary>
        /// 将 msg 序列化为能够传输的网络包
        /// [2byte, 2byte, 2byte, s]
        /// </summary>
        public static byte[] ToPacket(ref NetMessage msg, bool encryption = false)
        {
            if (encryption) msg.data = Encryption(msg.data);

            var oBuffer = BitConverter.GetBytes(msg.op);
            Array.Reverse(oBuffer);
            var sBuffer = BitConverter.GetBytes(msg.sub);
            Array.Reverse(sBuffer);

            using MemoryStream mStream = new MemoryStream();
            using BinaryWriter bw = new BinaryWriter(mStream);
            bw.Write(oBuffer); // 2Byte
            bw.Write(sBuffer); // 2Byte
            //bw.Write(msg.Data);   // 会携带长度
            bw.Write(StringToBytes(msg.data)); // string byte

            byte[] msgBuff = new byte[(short) mStream.Length];

            // 长度检查
            Assert.IsTrue(mStream.Length + 2 <= ushort.MaxValue);
            Buffer.BlockCopy(mStream.GetBuffer(), 0, msgBuff, 0, (int) mStream.Length);

            return ConcatHeadWithLength(msgBuff);
        }

        /// <summary>
        /// 在 msg 前添加描述 msg 长度的两个字节 buffer
        /// </summary>
        public static byte[] ConcatHeadWithLength(byte[] msg, bool revers = true)
        {
            byte[] h = BitConverter.GetBytes((ushort) msg.Length); // 2 byte.
            if (revers) Array.Reverse(h);
            return h.Concat(msg).ToArray();
        }

        public static byte[] StringToBytes(string msg)
        {
            return Encoding.UTF8.GetBytes(msg);
        }

        #endregion

        #region TCP 拆包
        public static NetMessage UnPacket(byte[] readBuffer, int bufferCount, bool encryption, out int readLength)
        {
            readLength = 0;
            if (bufferCount <= 2) return null;

            //解析包头
            readLength = BitConverter.ToInt16(readBuffer, 0);
            var op = BitConverter.ToInt16(readBuffer, 3); 
            var sub = BitConverter.ToInt16(readBuffer, 5);


            if (bufferCount < readLength + 4) return null;

            //解析数据
            var msgBuffer = new byte[readLength];
            Array.Copy(readBuffer, 6, msgBuffer, 0, readLength);
            readLength += 6;

            return new NetMessage()
            {
                op =  op,
                sub =  sub,
                data = Encoding.UTF8.GetString(msgBuffer)
            };
        }

        #endregion
    }
}