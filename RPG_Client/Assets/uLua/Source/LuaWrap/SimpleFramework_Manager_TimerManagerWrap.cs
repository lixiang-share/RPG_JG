using System;
using LuaInterface;

public class SimpleFramework_Manager_TimerManagerWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("StartTimer", StartTimer),
			new LuaMethod("StopTimer", StopTimer),
			new LuaMethod("AddTimerEvent", AddTimerEvent),
			new LuaMethod("RemoveTimerEvent", RemoveTimerEvent),
			new LuaMethod("StopTimerEvent", StopTimerEvent),
			new LuaMethod("ResumeTimerEvent", ResumeTimerEvent),
			new LuaMethod("New", _CreateSimpleFramework_Manager_TimerManager),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("Interval", get_Interval, set_Interval),
		};

		LuaScriptMgr.RegisterLib(L, "SimpleFramework.Manager.TimerManager", typeof(SimpleFramework.Manager.TimerManager), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSimpleFramework_Manager_TimerManager(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			SimpleFramework.Manager.TimerManager obj = new SimpleFramework.Manager.TimerManager();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SimpleFramework.Manager.TimerManager.New");
		}

		return 0;
	}

	static Type classType = typeof(SimpleFramework.Manager.TimerManager);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Interval(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SimpleFramework.Manager.TimerManager obj = (SimpleFramework.Manager.TimerManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Interval");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Interval on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Interval);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Interval(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SimpleFramework.Manager.TimerManager obj = (SimpleFramework.Manager.TimerManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Interval");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Interval on a nil value");
			}
		}

		obj.Interval = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StartTimer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.TimerManager obj = (SimpleFramework.Manager.TimerManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "SimpleFramework.Manager.TimerManager");
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 2);
		obj.StartTimer(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StopTimer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SimpleFramework.Manager.TimerManager obj = (SimpleFramework.Manager.TimerManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "SimpleFramework.Manager.TimerManager");
		obj.StopTimer();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddTimerEvent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.TimerManager obj = (SimpleFramework.Manager.TimerManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "SimpleFramework.Manager.TimerManager");
		SimpleFramework.Manager.TimerInfo arg0 = (SimpleFramework.Manager.TimerInfo)LuaScriptMgr.GetNetObject(L, 2, typeof(SimpleFramework.Manager.TimerInfo));
		obj.AddTimerEvent(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveTimerEvent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.TimerManager obj = (SimpleFramework.Manager.TimerManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "SimpleFramework.Manager.TimerManager");
		SimpleFramework.Manager.TimerInfo arg0 = (SimpleFramework.Manager.TimerInfo)LuaScriptMgr.GetNetObject(L, 2, typeof(SimpleFramework.Manager.TimerInfo));
		obj.RemoveTimerEvent(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StopTimerEvent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.TimerManager obj = (SimpleFramework.Manager.TimerManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "SimpleFramework.Manager.TimerManager");
		SimpleFramework.Manager.TimerInfo arg0 = (SimpleFramework.Manager.TimerInfo)LuaScriptMgr.GetNetObject(L, 2, typeof(SimpleFramework.Manager.TimerInfo));
		obj.StopTimerEvent(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ResumeTimerEvent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.TimerManager obj = (SimpleFramework.Manager.TimerManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "SimpleFramework.Manager.TimerManager");
		SimpleFramework.Manager.TimerInfo arg0 = (SimpleFramework.Manager.TimerInfo)LuaScriptMgr.GetNetObject(L, 2, typeof(SimpleFramework.Manager.TimerInfo));
		obj.ResumeTimerEvent(arg0);
		return 0;
	}
}

