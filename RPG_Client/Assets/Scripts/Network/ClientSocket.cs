using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;

public class ClientSocket {
    private TcpClient tcpClient;
    private bool isInit;
    private NetEventCallBack defEventCallBack;
	public void Init(NetEventCallBack defEventCallBack)
    {
        if (!isInit)
        {
            this.defEventCallBack = defEventCallBack;
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
            Debug.LogError("Init ClientSocket first");
            return;
        }
        state.Client = tcpClient.Client;
        tcpClient.BeginConnect(ip, port, new AsyncCallback(state.OnConnect), state);
    }

    public void Send(MsgEntity msg,StateObj state)
    {
        byte[] buff = CommonUtils.SerializerMsg(msg);
        state.Client = tcpClient.Client;
        try
        {
            tcpClient.Client.BeginSend(buff, 0, buff.Length, 0, new AsyncCallback(OnSendCallBack), state);
        }
        catch (Exception)
        {
            Debug.Log("receive data timeout^=....");
        }
        
    }

    private void OnSendCallBack(IAsyncResult iar)
    {
        StateObj state = (StateObj)iar.AsyncState;
        if (state.OnReceive != null) state.OnSend(iar);

        state.Client.BeginReceive(state.Buff, 0, state.Buff.Length, 0, new AsyncCallback(OnReceive), state);
    }

    private void OnReceive(IAsyncResult ar)
    {
        
    }

    private void OnReceiveResetData(IAsyncResult ar)
    {

    }
}
