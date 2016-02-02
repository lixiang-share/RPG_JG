using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class SimpleFramework_Manager_NetworkMgrWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("OnInit", OnInit),
			new LuaMethod("Unload", Unload),
			new LuaMethod("CallMethod", CallMethod),
			new LuaMethod("AddEvent", AddEvent),
			new LuaMethod("SendConnect", SendConnect),
			new LuaMethod("SendMessage", SendMessage),
			new LuaMethod("New", _CreateSimpleFramework_Manager_NetworkMgr),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "SimpleFramework.Manager.NetworkMgr", typeof(SimpleFramework.Manager.NetworkMgr), regs, fields, typeof(LuaComponent));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSimpleFramework_Manager_NetworkMgr(IntPtr L)
	{
		LuaDLL.luaL_error(L, "SimpleFramework.Manager.NetworkMgr class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(SimpleFramework.Manager.NetworkMgr);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnInit(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SimpleFramework.Manager.NetworkMgr obj = (SimpleFramework.Manager.NetworkMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.NetworkMgr");
		obj.OnInit();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Unload(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SimpleFramework.Manager.NetworkMgr obj = (SimpleFramework.Manager.NetworkMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.NetworkMgr");
		obj.Unload();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CallMethod(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);
		SimpleFramework.Manager.NetworkMgr obj = (SimpleFramework.Manager.NetworkMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.NetworkMgr");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		object[] objs1 = LuaScriptMgr.GetParamsObject(L, 3, count - 2);
		object[] o = obj.CallMethod(arg0,objs1);
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddEvent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
		SimpleFramework.ByteBuffer arg1 = (SimpleFramework.ByteBuffer)LuaScriptMgr.GetNetObject(L, 2, typeof(SimpleFramework.ByteBuffer));
		SimpleFramework.Manager.NetworkMgr.AddEvent(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SendConnect(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SimpleFramework.Manager.NetworkMgr obj = (SimpleFramework.Manager.NetworkMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.NetworkMgr");
		obj.SendConnect();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SendMessage(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.NetworkMgr obj = (SimpleFramework.Manager.NetworkMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.NetworkMgr");
		SimpleFramework.ByteBuffer arg0 = (SimpleFramework.ByteBuffer)LuaScriptMgr.GetNetObject(L, 2, typeof(SimpleFramework.ByteBuffer));
		obj.SendMessage(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Lua_Eq(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Object arg0 = LuaScriptMgr.GetLuaObject(L, 1) as Object;
		Object arg1 = LuaScriptMgr.GetLuaObject(L, 2) as Object;
		bool o = arg0 == arg1;
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

