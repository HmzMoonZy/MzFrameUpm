using MzFrame;

public static class NetMessageBuilder
{

    public static NetMessage Request(EnumOp op, string data)
    {
        NetMessage netMsg = new NetMessage()
        {
            op =  (short)op,
            sub = (short)EnumSub.Request,
            data = data,
        };

        return netMsg;
    }
}