using System;
using System.Text;
using MzFrame;
using UnityEngine;
public class NetDemo : MonoBehaviour
{
    private GameServer _gameServer;

    public void Awake()
    {
     
    }

    public void Start()
    {
        _gameServer = new GameServer();   
    }

    private void Update()
    {
        _gameServer.Update();
    }

    public void Connect()
    {
        _gameServer.Connect( suc =>
        {
            if (suc)
            {
                Debug.Log("连接服务器成功");
            }
            else
            {
                Debug.Log("连接服务器失败");
            }
        });
    }
    
    public void SendMsg()
    {
        _gameServer.Login("mz001", "926484", message =>
        {
            if (message.sub == (short)EnumSub.Suc)
            {
                Debug.Log("登陆成功!");
            }
            else
            {
                Debug.Log("登陆失败!");
                Debug.Log(message.data);
            }
        });
    }

    private void OnDestroy()
    {
        _gameServer.Close();
    }
    
    

}