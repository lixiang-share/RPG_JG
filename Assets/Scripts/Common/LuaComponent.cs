using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleFramework;
using SimpleFramework.Manager;

public class LuaComponent : MonoBehaviour {
    private AppFacade m_Facade;
    private LuaManager m_LuaMgr;
    private ResourceManager m_ResMgr;
    private NetworkManager m_NetMgr;
    private AudioManager m_MusicMgr;
    private TimerManager m_TimerMgr;
    private ThreadManager m_ThreadMgr;

    public virtual void OnMessage(IMessage message) {
    }

    /// <summary>
    /// 注册消息
    /// </summary>
    /// <param name="view"></param>
    /// <param name="messages"></param>
    public void RegisterMessage(IView view, List<string> messages)
    {
        if (messages == null || messages.Count == 0) return;
        Controller.Instance.RegisterViewCommand(view, messages.ToArray());
    }

    /// <summary>
    /// 移除消息
    /// </summary>
    /// <param name="view"></param>
    /// <param name="messages"></param>
    public void RemoveMessage(IView view, List<string> messages)
    {
        if (messages == null || messages.Count == 0) return;
        Controller.Instance.RemoveViewCommand(view, messages.ToArray());
    }

    public AppFacade facade
    {
        get {
            if (m_Facade == null) {
                m_Facade = AppFacade.Instance;
            }
            return m_Facade;
        }
    }

    public LuaManager LuaMgr
    {
        get {
            if (m_LuaMgr == null) {
                m_LuaMgr = LuaManager.Instance;//facade.GetManager<LuaManager>(ManagerName.Lua);
            }
            return m_LuaMgr;
        }
        set { m_LuaMgr = value; }
    }

    public ResourceManager ResManager
    {
        get {
            if (m_ResMgr == null) {
                m_ResMgr = facade.GetManager<ResourceManager>(ManagerName.Resource);
            }
            return m_ResMgr;
        }
    }

    public NetworkManager NetManager
    {
        get {
            if (m_NetMgr == null) {
                m_NetMgr = facade.GetManager<NetworkManager>(ManagerName.Network);
            }
            return m_NetMgr;
        }
    }

    public AudioManager AudioMgr
    {
        get {
            if (m_MusicMgr == null) {
                m_MusicMgr = AudioManager.Instance;//facade.GetManager<AudioManager>(ManagerName.Music);
            }
            return m_MusicMgr;
        }
    }

    public TimerManager TimerManger
    {
        get {
            if (m_TimerMgr == null) {
                m_TimerMgr = facade.GetManager<TimerManager>(ManagerName.Timer);
            }
            return m_TimerMgr;
        }
    }

    public ThreadManager ThreadManager
    {
        get {
            if (m_ThreadMgr == null) {
                m_ThreadMgr = facade.GetManager<ThreadManager>(ManagerName.Thread);
            }
            return m_ThreadMgr;
        }
    }




    ////下面封装各种常用属性和方法

}
