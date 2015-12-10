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
        public string tableName;
        public string domain;
        public static Dictionary<string, LuaBehaviour> Domains = new Dictionary<string, LuaBehaviour>();
        public virtual  void Awake() {
            InitLuaFile();
            CallMethod("Awake", gameObject);
        }

        public virtual  void InitLuaFile()
        {
            if (UITools.isValidString(domain))
            {
                if (Domains.ContainsKey(domain))
                {
                    Debug.LogError("Domain : "+domain+"  has been exits");
                }
                else
                {
                    Domains.Add(domain, this);
                }
            }
            if (UITools.isValidString(luaFilename)&&UITools.isLuaFileExits(luaFilename))
            {

                    int startIndex = luaFilename.LastIndexOf('/')+1;
                    int endIndex = luaFilename.LastIndexOf('.');
                    tableName = luaFilename.Substring(startIndex , endIndex-startIndex);
                    LuaMgr.DoFile(luaFilename);
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
            CallMethod("OnEnable");
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
