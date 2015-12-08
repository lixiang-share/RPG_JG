using UnityEngine;
using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SimpleFramework {
    public class LuaBehaviour : LuaComponent {
        protected static bool initialize = false;

        private string data = null;
        private AssetBundle bundle = null;
        private List<LuaFunction> buttons = new List<LuaFunction>();

        public string lauFilename;
        private string tableName;

        protected void Awake() {
            InitLuaFile();
            CallMethod("Awake", gameObject);
        }

        private void InitLuaFile()
        {
            if (lauFilename != null)
            {
                tableName = lauFilename.Substring(lauFilename.LastIndexOf('/')+1).Trim();
                LuaManager.DoFile(lauFilename);
                initialize = true;
            }
        }

        protected void Start() {

            CallMethod("Start");
        }

        protected void OnClick() {
            CallMethod("OnClick");
        }
        
        /// <summary>
        /// 执行Lua方法
        /// </summary>
        protected object[] CallMethod(string func, params object[] args) {
            if (!initialize) return null;
            return Util.CallMethod(tableName, func, args);
        }

    }
}