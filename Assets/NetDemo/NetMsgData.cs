using System;

public enum EnumOp : Int16
{
    Login = 0,
    HeartBeat = 1,
}

public enum EnumSub : Int16
{
    Suc = 0,
    Request = 100,
    Fail = -1,
    Other = -100,
}

public struct LoginRequest
{
    public string playerid;
    public string pw;
}

public struct LoginResponse
{
    public string reason;
}

public struct HeartBeatRequest
{
    
}