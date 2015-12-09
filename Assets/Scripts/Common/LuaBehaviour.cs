using UnityEngine;
using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleFramework;


    public class LuaBehaviour : LuaComponent {
        protected static bool initialize = false;

        private string data = null;
        private AssetBundle bundle = null;
        private List<LuaFunction> buttons = new List<LuaFunction>();

        public string luaFilename;
        private string tableName;

        public virtual  void Awake() {
            InitLuaFile();
            CallMethod("Awake", gameObject);
        }


        public virtual  void InitLuaFile()
        {
            if (luaFilename != null && luaFilename.Trim().Length != 0)
            {
                tableName = luaFilename.Substring(luaFilename.LastIndexOf('/')+1).Trim();
                LuaManager.DoFile(luaFilename);
                initialize = true;
            }
            UIEventListener.Get(gameObject).onPress += (go, b) =>
            {
                OnHold(b);
            };
        }

        public virtual  void Start() {

            CallMethod("Start");
        }

        public virtual void OnEnable()
        {
            CallMethod("OnEnable", null);
        }

        public virtual  void OnClick() {
            CallMethod("OnClick");
        }

        public virtual void OnHold(bool b)
        {
            CallMethod("OnHold", b);
        }

        public virtual void OnCommand(string command, params System.Object[] objs)
        {
            CallMethod("OnCommand", objs);
        }

        /// <summary>
        /// 执行Lua方法
        /// </summary>
        protected object[] CallMethod(string func, params object[] args) {
            if (!initialize) return null;
            return Util.CallMethod(tableName, func, args);
        }

        
    }
