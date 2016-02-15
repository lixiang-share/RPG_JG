using UnityEngine;
using LuaInterface;
using System.Collections;
using System;
using SimpleFramework;
using SimpleFramework.Manager;
using System.Collections.Generic;


public class LuaBehaviour : MonoBehaviour, IReceiveData
{
    #region 字段
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
    public Dictionary<string, System.Object> varDict;
    public List<ParamInspector> varList;
    private MsgPacker packer;
    private bool isFirstEnable = true;

    [HideInInspector]
    public string doStringLuaFile;
    [HideInInspector]
    public Boolean isDoString = false;
    [HideInInspector]
    public string lua_OnClick = "";
    [HideInInspector]
    public string lua_OnDisable = "";
    [HideInInspector]
    public string lua_OnCommand = "";
    [HideInInspector]
    public string lua_Awake = "";
    [HideInInspector]
    public string lua_Start = "";
    [HideInInspector]
    public string lus_OnFirstEnable;
    [HideInInspector]
    public string lua_OnEnable = "";
    [HideInInspector]
    public string lua_OnHold = "";
    [HideInInspector]
    public string lua_OnReceiveData = "";
    [HideInInspector]
    public string lua_OnFirstEnable = "";
    #endregion

    #region 各种管理器
    private AppFacade m_Facade;
    private LuaManager m_LuaMgr;
    private ResourceManager m_ResMgr;
    private NetworkMgr m_NetMgr;
    private AudioManager m_MusicMgr;
    private TimerManager m_TimerMgr;
    private ThreadManager m_ThreadMgr;


    #endregion

    #region 初始化lua环境
    public virtual void Awake()
    {
        SetDomain();
        UIEventListener.Get(gameObject).onPress += (go, b) =>
        {
            OnHold(b);
        };
        if (!isDoString)
        {
            InitLuaFile();
            HandleParams();
            CallMethod("Awake", gameObject);
        }
        else
        {
            DoString(lua_Awake);
        }
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
        }
       
    }


    protected void SetDomain()
    {

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
    #endregion

    #region  Lua事件封装
    public virtual void Parse(IList list)
    {
    }

    public virtual void OnEnable()
    {
        if (!isDoString)
        {
            if (isFirstEnable)
            {
                isFirstEnable = false;
                CallMethod("OnFirstEnable");
            }
            else
            {
                CallMethod("lus_OnFirstEnable");
            }
        }
        else
        {
            if (isFirstEnable)
            {
                isFirstEnable = false;
                DoString(lua_OnFirstEnable);
            }
            else
            {
                DoString(lua_OnEnable);
            }
        }

    }

    public virtual object[] CallLuaMethod(string m_name, params object[] objs)
    {
        if (!isDoString)
        {
            return CallMethod(m_name, objs);
        }
        return null;
    }
    public virtual object[] ExcuteCommand(string command, params object[] objs)
    {
        return DoString(command, objs);
    }


    public virtual void Start()
    {
        if (!isDoString)
        {
            CallMethod("Start");
        }
        else
        {
            DoString(lua_Start);
        }
    }
    public virtual void OnClick()
    {

        if (!isDoString)
        {
            CallMethod("OnClick");
        }
        else
        {
            DoString(lua_OnClick);
        }
    }

    public virtual void OnHold(bool b)
    {
        if (!isDoString)
        {
            CallMethod("OnHold", b);
        }
        else
        {
            DoString(lua_OnHold);
        }
    }

    public virtual void OnCommand(string command, System.Object o)
    {
        if (!isDoString)
        {
            CallMethod("OnCommand", command, o);
        }
        else
        {
            DoString(lua_OnCommand, command, o);
        }
    }

    public virtual void OnCommand(string command)
    {
        OnCommand(command, null);
    }

    public virtual void OnDisable()
    {
        if (!isDoString)
        {
            CallMethod("OnDisable");
        }
        else
        {
            DoString(lua_OnDisable);
        }
    }

    public void ReceiveData(object msg)
    {
        if (!isDoString)
        {
            CallMethod("OnReceiveData" ,msg);
        }
        else
        {
            DoString(lua_OnReceiveData , msg);
        }
    }
    protected object[] DoString(string command)
    {
        command = "function func(inst)\n" + command + "\n end";
        LuaMgr.DoString(command);
        return LuaMgr.CallLuaFunction("func", this);
    }
    protected object[] DoString(string command, object param)
    {
        command = "function func(param , inst)\n" + command + "\n end";
        LuaMgr.DoString(command);
        return LuaMgr.CallLuaFunction("func", param , this);
    }
    protected object[] DoString(string command, object param, object paramEX)
    {
        command = "function func(param , paramEX , inst)\n" + command + "\n end";
        LuaMgr.DoString(command);
        return LuaMgr.CallLuaFunction("func", param, paramEX ,this);
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
                m_LuaMgr = LuaManager.Instance;
            }
            return m_LuaMgr;
        }
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
                m_NetMgr = NetworkMgr.instance;//facade.GetManager<NetworkMgr>(ManagerName.Network);
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
                m_MusicMgr = AudioManager.Instance;
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

    #region 消息发送
    public void SendMsg(MsgPacker msg)
    {
        NetManager.Send(msg);
    }

    public LuaBehaviour CreateMsg()
    {
        Packer = new MsgPacker();
        return this;
    }
    public LuaBehaviour SetMsgType(int msgType)
    {
        Packer.SetType(msgType);
        return this;
    }
    public LuaBehaviour AddInt(int i)
    {
        Packer.add<int>(i);
        return this;
    }
    public LuaBehaviour AddBool(bool b)
    {
        Packer.add<bool>(b);
        return this;
    }
    public LuaBehaviour AddFloat(float f)
    {
        Packer.add<float>(f);
        return this;
    }
    public LuaBehaviour AddString(string str)
    {
        Packer.add<string>(str);
        return this;
    }
    public void Send()
    {
        if (Packer != null)
        {
            Packer.Receiver = this;
            NetManager.Send(Packer);
            Packer = null;
        }
    }
    #endregion

    #region Set and Get 下面封装各种常用属性和方法
    public MsgPacker Packer
    {
        get { return packer; }
        set { packer = value; }
    }

    public object this[string key]
    {
        set
        {
            if (varDict == null) varDict = new Dictionary<string, System.Object>();
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

    public Component C(string name)
    {
        return gameObject.GetComponent(name);
    }

    public string Value
    {
        get
        {
            if (gameObject.GetComponent<UIInput>() != null)
            {
                return gameObject.GetComponent<UIInput>().value;
            }
            else if(gameObject.GetComponent<UILabel>() != null)
            {
                return gameObject.GetComponent<UILabel>().text;
            }
            else if (gameObject.GetComponent<UISprite>() != null)
            {
                return gameObject.GetComponent<UISprite>().spriteName;
            }
            return "";
        }
        set
        {
            if (gameObject.GetComponent<UIInput>() != null)
            {
                gameObject.GetComponent<UIInput>().value = value;
            }
            else if (gameObject.GetComponent<UILabel>() != null)
            {
                gameObject.GetComponent<UILabel>().text = value;
            }
            else if (gameObject.GetComponent<UISprite>() != null)
            {
                gameObject.GetComponent<UISprite>().spriteName = value;
            }
        }
    }

    
    public LuaBehaviour Parent
    {
        get
        {
            Transform parent = this.transform.parent;
            if (parent != null && parent.gameObject != null)
            {
                return UITools.Get<LuaBehaviour>(parent.gameObject);
            }
            return null;
        }
    }

    public LuaBehaviour GetChild(string name)
    {
        GameObject go = transform.FindChild(name).gameObject;
        if (go != null)
        {
            return UITools.Get<LuaBehaviour>(go);
        }
        return null;
    }
    public GameObject Child(string name)
    {
        return transform.FindChild(name).gameObject;
    }

    #endregion

}