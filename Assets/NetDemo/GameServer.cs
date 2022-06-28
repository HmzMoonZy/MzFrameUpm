using System;
using System.Text;
using MzFrame;
using UnityEngine;

public class GameServer
{
    MzSocket _server;

    public GameServer()
    {
        _server = new MzSocket(NetConfig.Default, BytesToNetMessage, NetMessageToString);
    }

    public void Update()
    {
        _server.Update();
    }

    public void Connect(Action<bool> callback)
    {
        _server.Connect("127.0.0.1", 9264, callback);
    }

    public void Login(string playerid, string pw, Action<NetMessage> callback)
    {
        var d =  new LoginRequest()
        {
            playerid = playerid,
            pw = pw,
        };
        var msg = NetMessageBuilder.Request(EnumOp.Login, JsonUtility.ToJson(d));
        
        _server.RegisterEvent(((short) (EnumOp.Login)).ToString(), callback);
        
        Send(ref msg);
    }

    public void Close() => _server.Close();
    
    private void Send(ref NetMessage msg)
    {
        _server.Send(ref msg);
    }
    
    
    
    private static NetMessage BytesToNetMessage(byte[] b)
    {
        var json = Encoding.UTF8.GetString(b);
        var netMessage = JsonUtility.FromJson<NetMessage>(json);
        return netMessage;
    }

    private static string NetMessageToString(NetMessage nmsg)
    {
        return JsonUtility.ToJson(nmsg);
    }
}

