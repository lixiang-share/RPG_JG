using System;
using LuaInterface;

public class SimpleFramework_LuaHelperWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("GetType", GetType),
			new LuaMethod("GetPanelManager", GetPanelManager),
			new LuaMethod("GetResManager", GetResManager),
			new LuaMethod("GetNetManager", GetNetManager),
			new LuaMethod("GetMusicManager", GetMusicManager),
			new LuaMethod("Action", Action),
			new LuaMethod("VoidDelegate", VoidDelegate),
			new LuaMethod("OnCallLuaFunc", OnCallLuaFunc),
			new LuaMethod("OnJsonCallFunc", OnJsonCallFunc),
			new LuaMethod("New", _CreateSimpleFramework_LuaHelper),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaScriptMgr.RegisterLib(L, "SimpleFramework.LuaHelper", regs);
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSimpleFramework_LuaHelper(IntPtr L)
	{
		LuaDLL.luaL_error(L, "SimpleFramework.LuaHelper class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(SimpleFramework.LuaHelper);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetType(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		Type o = SimpleFramework.LuaHelper.GetType(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPanelManager(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		SimpleFramework.Manager.PanelManager o = SimpleFramework.LuaHelper.GetPanelManager();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetResManager(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		SimpleFramework.Manager.ResourceManager o = SimpleFramework.LuaHelper.GetResManager();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetNetManager(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		SimpleFramework.Manager.NetworkManager o = SimpleFramework.LuaHelper.GetNetManager();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMusicManager(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		SimpleFramework.Manager.MusicManager o = SimpleFramework.LuaHelper.GetMusicManager();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Action(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaFunction arg0 = LuaScriptMgr.GetLuaFunction(L, 1);
		Action o = SimpleFramework.LuaHelper.Action(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int VoidDelegate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaFunction arg0 = LuaScriptMgr.GetLuaFunction(L, 1);
		UIEventListener.VoidDelegate o = SimpleFramework.LuaHelper.VoidDelegate(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnCallLuaFunc(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaStringBuffer arg0 = LuaScriptMgr.GetStringBuffer(L, 1);
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 2);
		SimpleFramework.LuaHelper.OnCallLuaFunc(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnJsonCallFunc(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 2);
		SimpleFramework.LuaHelper.OnJsonCallFunc(arg0,arg1);
		return 0;
	}
}

