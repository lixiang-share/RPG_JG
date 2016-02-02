using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// 消息队列，解耦网络层和UI的消息依赖
/// </summary>
public class MsgQueue : MonoBehaviour {

    private IList<MsgEntity> msgQueue;
    private bool isLoopHandle = false;

	void Start () {
        
	}
	void Update () {
        if (isLoopHandle)
        {
            LoophandleMsg();
        }
	}
    void Init()
    {
        if (msgQueue == null) msgQueue = new List<MsgEntity>();
    }
    private void LoophandleMsg()
    {
        for (int i = 0; i < msgQueue.Count; i++)
        {
            msgQueue[i].Receiver.ReceiveData(msgQueue[i]);
        }
        msgQueue.Clear();
    }
    public void addMsg(MsgEntity msg)
    {
        if(msg!= null)
            msgQueue.Add(msg);
    }
    public void removeMsg(MsgEntity msg)
    {
        msgQueue.Remove(msg);
    }
    public void removeMsgAt(int index)
    {
        if (index < msgQueue.Count)
            msgQueue.RemoveAt(index);
    }

}
