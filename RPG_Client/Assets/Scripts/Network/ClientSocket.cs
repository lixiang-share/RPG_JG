using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;

public class ClientSocket {
    private TcpClient tcpClient;
    private bool isInit;
	public void Init()
    {
        if (!isInit)
        {
            tcpClient = new TcpClient();
            tcpClient.SendTimeout = 1000;
            tcpClient.ReceiveTimeout = 1000;
            tcpClient.SendBufferSize = 1014 * 1024;
            isInit = true;

        }
    }
    public void ConnectServer(string ip, int port,StateObj state)
    {
        if (!isInit)
        {
            UITools.logError("Init ClientSocket first");
            return;
        }
        state.Client = tcpClient.Client;
        try
        {
            tcpClient.BeginConnect(ip, port, new AsyncCallback(OnConnected), state);
        }
        catch (Exception)
        {
            state.OnConnectError(state);
        }
    }

    public void OnConnected(IAsyncResult iar)
    {
        StateObj state = (StateObj)iar.AsyncState;
        if(state != null) state.OnConnect(state);
    }

    public void Send(MsgEntity msg,StateObj state = null)
    {
        byte[] buff = MsgUtils.SerializerMsg(msg);
        if (state == null) state = new StateObj();
        state.Msg = msg;
        state.Client = tcpClient.Client;
        try
        {
            tcpClient.Client.BeginSend(buff, 0, buff.Length, 0, new AsyncCallback(OnSendCallBack), state);
        }
        catch (Exception)
        {
            UITools.log("send data timeout....");
        }
        
    }

    private void OnSendCallBack(IAsyncResult iar)
    {
        StateObj state = (StateObj)iar.AsyncState;
        state.OnSend(state);
        if (state.Msg.IsNeedRecv)
        {
            try
            {
                state.Client.BeginReceive(state.Buff, 0, state.Buff.Length, 0, new AsyncCallback(OnReceive), state);
            }
            catch (Exception)
            {
                state.OnRecvError(state);
            }
        }
        else
        {
            state.OnReceive(state);
        }
    }

    private void OnReceive(IAsyncResult iar)
    {
        StateObj state = (StateObj)iar.AsyncState;
        //读取的消息总长度
        int len = state.Client.EndReceive(iar);
        if(len < 4)
        {
            UITools.logError("invaild msg");
            state.OnRecvError(state);
            return;
        }
        byte[] buff = state.Buff;
        int msgRealLen = MsgUtils.DecodeMsgRealLen(buff, 0, 4);
        UITools.log("msg real length : "+msgRealLen);
        state.RecvLen = msgRealLen;
        state.RecvBuff.addRange<byte>(buff, AppConst.MsgHeadLen, len - 4);
        
        if(state.RecvLen - state.RecvBuff.Count > 0)
        {
            try
            {
                state.Client.BeginReceive(state.Buff, 0, state.Buff.Length, 0, new AsyncCallback(OnReceiveRestData), state);
            }
            catch (Exception)
            {
                state.OnRecvError(state);
                UITools.log("receive data timeout....");
            }
        }
        else
        {
            state.OnReceive(state);
        }

        
    }

    private void OnReceiveRestData(IAsyncResult iar)
    {
        StateObj state = (StateObj)iar.AsyncState;
        int len = state.Client.EndReceive(iar);
        byte[] buff = state.Buff;
        state.RecvBuff.addRange<byte>(buff , 0 , len);
        int contentRest = state.RecvLen - state.RecvBuff.Count;
        if(contentRest <= 0)
        {
            state.OnReceive(state);
            return;
        }
        try
        {
            state.Client.BeginReceive(state.Buff, 0, state.Buff.Length, 0, new AsyncCallback(OnReceiveRestData), state);
        }
        catch (Exception)
        {
            state.OnRecvError(state);
            UITools.log("receive data timeout....");
        }
    }


    public void CloseConnection(StateObj state)
    {
        try
        {
            tcpClient.Client.BeginDisconnect(true, new AsyncCallback(OnDisConnect), state);
        }
        catch (Exception)
        {
            UITools.logError("Close Connection error");
        }
    }

    private void OnDisConnect(IAsyncResult iar)
    {
        StateObj state = (StateObj)iar.AsyncState;
        state.OnDisConnect(state);
    }
}
