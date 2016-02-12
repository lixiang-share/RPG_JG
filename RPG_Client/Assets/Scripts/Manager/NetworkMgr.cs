using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;


public class NetworkMgr : MonoBehaviour
{

    private ClientSocket clientSocket;
    private bool isConnected = false;
    private MsgHandlerMgr handerMgr;
    private List<StateObj> eventQueue;
    public static NetworkMgr instance;



    void Awake()
    {
        instance = this;
        Init();
    }
    void Init()
    {
        eventQueue = new List<StateObj>();
        clientSocket = new ClientSocket();
        clientSocket.Init();
        handerMgr = new MsgHandlerMgr()
            .RegisterHander(new MsgFilterHandler())
            .RegisterHander(new MsgTransferHandler());
    }

    public void Update()
    {
        if (eventQueue.Count > 0)
        {
            for (int i = 0; i < eventQueue.Count; i++)
            {
                HandleMsg(eventQueue[i]);
            }
        }
        eventQueue.Clear();
    }
    
    void OnGUI()
    {
        if (GUILayout.Button("Connect"))
        {
            Connect();
        }
        if (GUILayout.Button("Send Data"))
        {

            MsgPacker msg = new MsgPacker();
            msg.SetType(MsgProtocol.Get_ServerList);
            Send(msg);
        }
    }
    public void Connect()
    {
        StateObj state = new StateObj();
        state.OnConnect = OnConnect;
        clientSocket.ConnectServer("127.0.0.1", 12345, state);
    }

    void DisConnect()
    {

    }
    public void Send(MsgPacker msg)
    {
        StateObj _state = new StateObj();
        _state.MsgType = msg.MsgType;
        _state.Receiver = msg.Receiver;
        _state.IsNeedRecv = msg.IsNeedRecv;
        _state.OnSend = OnSend;
        _state.OnReceive = OnReceiveData;
        _state.SendBuff = MsgUtils.SerializerMsg(msg);
        clientSocket.Send(_state);
    }

    void OnConnect(StateObj state)
    {
        UITools.log("Connect ro Server and start send msg");
    }
    void OnDisConnect(StateObj state)
    {

    }
    void OnSend(StateObj state)
    {
        UITools.log("send msg compelete and waitFor receive");
    }

    void OnReceiveData(StateObj state)
    {
        UITools.log("add msg to eventQueue");
        eventQueue.Add(state);
    }

    void HandleMsg(StateObj state){
        UITools.log("receive msg compelete and decode msg ---> recever : ");
        MsgUnPacker unpacker = MsgUtils.DeserializerMsg(state.RecvBuff.ToByteArray());
        unpacker.MsgType = state.MsgType;
        unpacker.Receiver = state.Receiver;
        handerMgr.HandleMsg(unpacker);
    }


}
