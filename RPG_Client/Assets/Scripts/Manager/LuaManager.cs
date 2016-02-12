using UnityEngine;
using System.Collections;
using SimpleFramework;
using System.IO;
using LuaInterface;
public class LuaManager {


    private static LuaManager instance;

    public static LuaManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LuaManager();
            }
            return instance;
        }
    }

    private LuaScriptMgr luaMgr;

    public LuaState Lua
    {
        get
        {
            return luaMgr.lua;
        }
    }


    //TODO 此处有坑
    private LuaManager()
    {
        luaMgr = new LuaScriptMgr();
        //如果需要发布，此处必须当资源解析完成后执行，初始化环境信息
        Start();
    }

    public void DoFile(string filename)
    {
        string fullName = Util.LuaPath(filename);
        if (File.Exists(fullName))
        {
            UITools.Compile(fullName);
            
        }
        luaMgr.DoFile(filename);
    }

    public void Start()
    {
        luaMgr.Start();
    }

    public void FixedUpdate()
    {
        luaMgr.FixedUpdate();
    }

    public void LateUpdate()
    {
        luaMgr.LateUpdate();
    }

    public void Update()
    {
        luaMgr.Update();
    }

    public void Destroy()
    {
        luaMgr.Destroy();
    }

    public object[] CallLuaFunction(string funcName, params object[] args)
    {
        return luaMgr.CallLuaFunction(funcName, args);
    }

    public object[] DoString(string command)
    {
        return luaMgr.DoString(command);
    }


}
