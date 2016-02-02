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
            clientSocket.Init(DefNetCallBack);
        }

        void OnGUI()
        {
            if (GUILayout.Button("Connect"))
            {
                clientSocket.ConnectServer("127.0.0.1", 12345, (IAsyncResult iar)=>
                {
                    Debug.Log("Connected to Server");
                });
            }
        }


        public void OnInit() {

        }

        public void Unload() {

        }

        /// <summary>
        /// 执行Lua方法
        /// </summary>
        public object[] CallMethod(string func, params object[] args) {
            return null;
        }

        ///------------------------------------------------------------------------------------
        public static void AddEvent(int _event, ByteBuffer data) {
    
        }

        /// <summary>
        /// 交给Command，这里不想关心发给谁。
        /// </summary>
        void Update() {
            
        }

        /// <summary>
        /// 发送链接请求
        /// </summary>
        public void SendConnect() {
            
        }

        /// <summary>
        /// 发送SOCKET消息
        /// </summary>
        public void SendMessage(ByteBuffer buffer) {
          
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        void OnDestroy() {
           
        }



        public void DefNetCallBack(IAsyncResult iar)
        {

        }
    }
}