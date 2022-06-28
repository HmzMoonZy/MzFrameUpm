namespace MzFrame
{
    public struct NetErrMsg
    {
        private const string ErrConnectReason = "连接服务器失败";
        private const string ErrReceiveReason = "无法解析的消息";
        private const string ErrCloseReason = "关闭连接错误";
        
        public short Code;

        public string Reason;

        private NetErrMsg(short c, string r)
        {
            this.Code = c;
            this.Reason = r;
        }
        
        public static NetErrMsg ConnectErr => new NetErrMsg(0, ErrConnectReason);
        
        public static NetErrMsg ReceiveErr => new NetErrMsg(1, ErrReceiveReason);
        
        public static NetErrMsg CloseErr => new NetErrMsg(2, ErrCloseReason);
    }
}