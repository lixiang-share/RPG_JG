using UnityEngine;
using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleFramework;
using SimpleFramework.Manager;


public class LuaBehaviour : MonoBehaviour
{
    protected bool initialize = false;
    private string data = null;
    private AssetBundle bundle = null;
    private List<LuaFunction> buttons = new List<LuaFunction>();
    [HideInInspector]
    public string luaFilename;
    [HideInInspector]
    public string tableName;
    [HideInInspector]
    public string domain;
    public static Dictionary<string, LuaBehaviour> Domains = new Dictionary<string, LuaBehaviour>();
    public Dictionary<string , System.Object> varDict;
    public List<ParamInspector> varList;


    #region 各种管理器
    private AppFacade m_Facade;
    private LuaManager m_LuaMgr;
    private ResourceManager m_ResMgr;
    private NetworkMgr m_NetMgr;
    private AudioManager m_MusicMgr;
    private TimerManager m_TimerMgr;
    private ThreadManager m_ThreadMgr;
    #endregion

    public virtual void Awake()
    {
        InitLuaFile();
        CallMethod("Awake", gameObject);
    }

    public virtual void InitLuaFile()
    {
        if (UITools.isValidString(luaFilename) && UITools.isLuaFileExits(luaFilename) && !initialize)
        {

            int startIndex = luaFilename.LastIndexOf('/') + 1;
            int endIndex = luaFilename.LastIndexOf('.');
            tableName = luaFilename.Substring(startIndex, endIndex - startIndex);
            LuaMgr.DoFile(luaFilename);
            initialize = true;
            SetDomain();
            HandleParams();
        }
        UIEventListener.Get(gameObject).onPress += (go, b) =>
        {
            OnHold(b);
        };
    }


    protected void  SetDomain(){

        if (UITools.isValidString(domain))
        {
            if (Domains.ContainsKey(domain))
            {
                Debug.LogError("Domain : " + domain + "  has been exits");
            }
            else
            {
                Domains.Add(domain, this);
            }
        }
    }

    private void HandleParams()
    {
        if (varList != null)
        {
            foreach (ParamInspector p in varList)
            {
                this[p.Key] = p.Value;
            }
        }
        if (initialize)
        {
            LuaState luaState = LuaMgr.Lua;
            luaState[tableName + ".inst"] = this;
        }
    }
    #region  Lua事件封装
    public virtual void Start()
    {
        
        CallMethod("Start");
    }

    public virtual void OnEnable()
    {
        CallMethod("OnEnable");
    }

    public virtual void OnClick()
    {
        CallMethod("OnClick");
    }

    public virtual void OnHold(bool b)
    {
        CallMethod("OnHold", b);
    }

    public virtual void OnCommand(string command, System.Object o)
    {
        CallMethod("OnCommand",command , o);
    }

    public virtual void OnCommand(string command)
    {
        OnCommand(command, null);
    }

    public void OnDisable()
    {
        CallMethod("OnDisable");
    }


    /// <summary>
    /// 执行Lua方法
    /// </summary>
    protected object[] CallMethod(string func, params object[] args)
    {
        if (!initialize) return null;
        return Util.CallMethod(tableName, func, args);
    }
    #endregion

    #region 各种管理器获取方式
    public AppFacade facade
    {
        get
        {
            if (m_Facade == null)
            {
                m_Facade = AppFacade.Instance;
            }
            return m_Facade;
        }
    }

    public LuaManager LuaMgr
    {
        get
        {
            if (m_LuaMgr == null)
            {
                m_LuaMgr = LuaManager.Instance;//facade.GetManager<LuaManager>(ManagerName.Lua);
            }
            return m_LuaMgr;
        }
        set { m_LuaMgr = value; }
    }

    public ResourceManager ResManager
    {
        get
        {
            if (m_ResMgr == null)
            {
                m_ResMgr = facade.GetManager<ResourceManager>(ManagerName.Resource);
            }
            return m_ResMgr;
        }
    }

    public NetworkMgr NetManager
    {
        get
        {
            if (m_NetMgr == null)
            {
                m_NetMgr = facade.GetManager<NetworkMgr>(ManagerName.Network);
            }
            return m_NetMgr;
        }
    }

    public AudioManager AudioMgr
    {
        get
        {
            if (m_MusicMgr == null)
            {
                m_MusicMgr = AudioManager.Instance;//facade.GetManager<AudioManager>(ManagerName.Music);
            }
            return m_MusicMgr;
        }
    }

    public TimerManager TimerManger
    {
        get
        {
            if (m_TimerMgr == null)
            {
                m_TimerMgr = facade.GetManager<TimerManager>(ManagerName.Timer);
            }
            return m_TimerMgr;
        }
    }

    public ThreadManager ThreadManager
    {
        get
        {
            if (m_ThreadMgr == null)
            {
                m_ThreadMgr = facade.GetManager<ThreadManager>(ManagerName.Thread);
            }
            return m_ThreadMgr;
        }
    }
    #endregion


    ////下面封装各种常用属性和方法

    public object this[string key]
    {
        set
        {
            if(varDict == null) varDict = new Dictionary<string , System.Object>();
            if (varDict.ContainsKey(key))
            {
               // varDict.Remove(key);
                varDict[key] = value;
            }
            else
            {
                varDict.Add(key, value);
            }
        }
        get
        {
            if (varDict != null && varDict.ContainsKey(key))
            {
                return varDict[key];
            }
            return null;
        }
    }

    public void S(string key, object value)
    {
        this[key] = value;
    }

    public object G(string key)
    {
        return this[key];
    }

    public LuaBehaviour Parent
    {
        get
        {
            Transform parent = this.transform.parent;
            if (parent == null || parent == this.transform)
            {
                return null;
            }
            else
            {
                LuaBehaviour lb = parent.gameObject.GetComponent<LuaBehaviour>();
                if (lb == null)
                {
                    lb = parent.gameObject.AddComponent<LuaBehaviour>();
                }
                return lb;
            }
        }
    }

}
