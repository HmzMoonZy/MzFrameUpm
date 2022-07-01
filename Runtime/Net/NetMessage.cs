using System;

namespace MzFrame
{
    /// <summary>
    /// 网络消息,通常会被序列化后添加包头
    /// </summary>
    public class NetMessage
    {
        public short op;        // 业务识别码

        public short sub;       // 回复识别码

        public string data;     // 数据,通常是json

        public override string ToString()
        {
            return $"OpCode : {op}  || SubCode : {sub} || Data : {data}";
        }
    }
    
}