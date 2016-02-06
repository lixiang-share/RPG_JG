using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;

namespace SimpleFramework.Manager {
    public class NetworkMgr : LuaComponent {

        private ClientSocket clientSocket;
        private bool isConnected = false;
        private MsgHandlerMgr handerMgr;



        void Awake() {
            Init();
        }
        void Init() {
            clientSocket = new ClientSocket();
            clientSocket.Init();
            handerMgr = new MsgHandlerMgr()
                .RegisterHander(new MsgFilterHandler());
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
                msg.setType(1);
                msg.add<string>("hello server");
                Send(msg);
            }
        }


        void Connect()
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
            UITools.log("receive msg compelete and decode msg");
            MsgUnPacker unpacker = MsgUtils.DeserializerMsg(state.RecvBuff.ToByteArray());
            handerMgr.HandleMsg(unpacker);

        }


    }
}