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
			new LuaMethod("Send", Send),
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
	static int Send(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.NetworkMgr obj = (SimpleFramework.Manager.NetworkMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.NetworkMgr");
		MsgPacker arg0 = (MsgPacker)LuaScriptMgr.GetNetObject(L, 2, typeof(MsgPacker));
		obj.Send(arg0);
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

