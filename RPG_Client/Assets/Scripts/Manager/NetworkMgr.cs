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
                StateObj state = new StateObj(); 
                state.OnConnect = (StateObj s) => {
                    UITools.log("Connect ro Server");
                    MsgEntity msg = new MsgEntity();
                    clientSocket.Send(msg ,s);
                };
                clientSocket.ConnectServer("127.0.0.1", 12345, state);
            }
        }


        void Connect()
        {

        }
        void DisConnect()
        {

        }
        public void Send(MsgEntity msg)
        {

        }

        void OnConnect(StateObj state)
        {

        }
        void OnDisConnect(StateObj state)
        {

        }



    }
}