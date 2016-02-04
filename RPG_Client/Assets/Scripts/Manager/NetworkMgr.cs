using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;

namespace SimpleFramework.Manager {
    public class NetworkMgr : LuaComponent {

        private ClientSocket clientSocket;
        private bool isConnected = false;
        void Awake() {
            Init();
        }
        void Init() {
            clientSocket = new ClientSocket();
            clientSocket.Init();
        }

        void OnGUI()
        {
            if (GUILayout.Button("Connect"))
            {
                Connect();
            }
            if (GUILayout.Button("Send Data"))
            {

                MsgEntity msg = new MsgEntity();
                msg.Content = "Hello server";
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
        public void Send(MsgEntity msg)
        {
            StateObj _state = new StateObj();
            _state.OnSend = OnSend;
            _state.OnReceive = OnReceiveData;
            clientSocket.Send(msg, _state);
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
            MsgUtils.DeserializerMsg(state.RecvBuff.ToArray());
        }


    }
}