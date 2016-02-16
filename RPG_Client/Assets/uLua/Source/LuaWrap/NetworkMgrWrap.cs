using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class NetworkMgrWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Update", Update),
			new LuaMethod("Connect", Connect),
			new LuaMethod("Send", Send),
			new LuaMethod("OnConnect", OnConnect),
			new LuaMethod("New", _CreateNetworkMgr),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("instance", get_instance, set_instance),
		};

		LuaScriptMgr.RegisterLib(L, "NetworkMgr", typeof(NetworkMgr), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateNetworkMgr(IntPtr L)
	{
		LuaDLL.luaL_error(L, "NetworkMgr class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(NetworkMgr);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_instance(IntPtr L)
	{
		LuaScriptMgr.Push(L, NetworkMgr.instance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_instance(IntPtr L)
	{
		NetworkMgr.instance = (NetworkMgr)LuaScriptMgr.GetUnityObject(L, 3, typeof(NetworkMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Update(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		NetworkMgr obj = (NetworkMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "NetworkMgr");
		obj.Update();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Connect(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		NetworkMgr obj = (NetworkMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "NetworkMgr");
		ServerEntity arg0 = (ServerEntity)LuaScriptMgr.GetNetObject(L, 2, typeof(ServerEntity));
		DefAction arg1 = null;
		LuaTypes funcType3 = LuaDLL.lua_type(L, 3);

		if (funcType3 != LuaTypes.LUA_TFUNCTION)
		{
			 arg1 = (DefAction)LuaScriptMgr.GetNetObject(L, 3, typeof(DefAction));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 3);
			arg1 = () =>
			{
				func.Call();
			};
		}

		obj.Connect(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Send(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		NetworkMgr obj = (NetworkMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "NetworkMgr");
		MsgPacker arg0 = (MsgPacker)LuaScriptMgr.GetNetObject(L, 2, typeof(MsgPacker));
		obj.Send(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnConnect(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		NetworkMgr obj = (NetworkMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "NetworkMgr");
		StateObj arg0 = (StateObj)LuaScriptMgr.GetNetObject(L, 2, typeof(StateObj));
		obj.OnConnect(arg0);
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

