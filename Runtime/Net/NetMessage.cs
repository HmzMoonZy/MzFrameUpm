using System;

namespace MzFrame
{
    /// <summary>
    /// 网络消息,通常会被序列化后添加包头
    /// </summary>
    public struct NetMessage
    {
        public short op;        // 业务识别码

        public short sub;       // 回复识别码

        public string data;     // 数据,通常是json

        public static NetMessage Null()
        {
            return new NetMessage() {op = -1, sub = -1, data= string.Empty};
        }

        public static bool IsNull(NetMessage msg)
        {
            return msg.op == -1;
        }

        public override string ToString()
        {
            return $"OpCode : {op}  || SubCode : {sub} || Data : {data}";
        }
    }
    
}