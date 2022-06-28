using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MzFrame
{
    using ProtolDelegate = Action<NetMessage>;
    
    public class MsgDistribution
    {
        public List<NetMessage> MsgList = new List<NetMessage>();
        
        private ushort _tick; //每帧最多处理的消息
        
        /// <summary>
        /// 持久回调
        /// </summary>
        private readonly Dictionary<string, ProtolDelegate> _eventDict = new Dictionary<string ,ProtolDelegate>();
        
        /// <summary>
        /// 单次回调
        /// </summary>
        private readonly Dictionary<string, ProtolDelegate> _onceDict = new Dictionary<string, ProtolDelegate>();

        public MsgDistribution(ushort tick)
        {
            this._tick = tick <= 0 ? UInt16.MaxValue : tick;
        }

        /// <summary>
        /// 处理消息队列.
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < _tick; i++)
            {
                if (MsgList.Count > 0)
                {
                    DispatchEvent(MsgList[0]);
                    lock (MsgList)
                    {
                        MsgList.RemoveAt(0);
                    }
                }
                else break;
            }
        }

        /// <summary>
        /// 消息分发 
        /// </summary>
        /// <param name="msg"></param>
        public void DispatchEvent(NetMessage msg)
        {
            var token = msg.op.ToString();

            if (_eventDict.ContainsKey(token))
            {
                _eventDict[token](msg);
            }

            if (_onceDict.ContainsKey(token))
            {
                _onceDict[token](msg);
                _onceDict[token] = null;
                _onceDict.Remove(token);
            }
        }

        public void AddMsg(NetMessage msg)
        {
            lock (MsgList)
            {
                MsgList.Add(msg);
            }
        }

        public void AddListener(string token, ProtolDelegate protDel)
        {
            if (_eventDict.ContainsKey(token))
                _eventDict[token] += protDel;
            else
                _eventDict[token] = protDel;

        }

        public void AddOnceListener(string token, ProtolDelegate protDel)
        {
            if (_onceDict.ContainsKey(token))
                _onceDict[token] += protDel;
            else
                _onceDict[token] = protDel;
        }

        public void DelListener(string token, ProtolDelegate protDel)
        {
            if (_eventDict.ContainsKey(token))
            {
                _eventDict[token] -= protDel;
                if (_eventDict[token] == null) _eventDict.Remove(token);
            }
        }

        public void DelOnceListener(string callback, ProtolDelegate protDel)
        {
            if (_onceDict.ContainsKey(callback))
            {
                _onceDict[callback] -= protDel;
                if (_onceDict[callback] == null) _onceDict.Remove(callback);
            }
        }

        public void ClearAll()
        {
            lock (MsgList)
            {
                MsgList.Clear();
                _onceDict.Clear();
                _eventDict.Clear();
            }
        }
    }
}
