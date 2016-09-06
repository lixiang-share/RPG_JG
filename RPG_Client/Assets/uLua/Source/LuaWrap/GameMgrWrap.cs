using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class GameMgrWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("LoadFightScene", LoadFightScene),
			new LuaMethod("LoadGame", LoadGame),
			new LuaMethod("LoadMainCity", LoadMainCity),
			new LuaMethod("LoadScene", LoadScene),
			new LuaMethod("CheckExtractResource", CheckExtractResource),
			new LuaMethod("OnResourceInited", OnResourceInited),
			new LuaMethod("ConnectServer", ConnectServer),
			new LuaMethod("OnConnect", OnConnect),
			new LuaMethod("New", _CreateGameMgr),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("Instance", get_Instance, set_Instance),
			new LuaField("go", get_go, set_go),
		};

		LuaScriptMgr.RegisterLib(L, "GameMgr", typeof(GameMgr), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateGameMgr(IntPtr L)
	{
		LuaDLL.luaL_error(L, "GameMgr class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(GameMgr);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Instance(IntPtr L)
	{
		LuaScriptMgr.Push(L, GameMgr.Instance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_go(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GameMgr obj = (GameMgr)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name go");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index go on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.go);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Instance(IntPtr L)
	{
		GameMgr.Instance = (GameMgr)LuaScriptMgr.GetUnityObject(L, 3, typeof(GameMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_go(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GameMgr obj = (GameMgr)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name go");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index go on a nil value");
			}
		}

		obj.go = (GameObject)LuaScriptMgr.GetUnityObject(L, 3, typeof(GameObject));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadFightScene(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameMgr obj = (GameMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameMgr");
		obj.LoadFightScene();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadGame(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameMgr obj = (GameMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameMgr");
		obj.LoadGame();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadMainCity(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameMgr obj = (GameMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameMgr");
		obj.LoadMainCity();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadScene(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameMgr obj = (GameMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameMgr");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.LoadScene(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CheckExtractResource(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameMgr obj = (GameMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameMgr");
		obj.CheckExtractResource();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnResourceInited(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameMgr obj = (GameMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameMgr");
		obj.OnResourceInited();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ConnectServer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameMgr obj = (GameMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameMgr");
		obj.ConnectServer();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnConnect(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameMgr obj = (GameMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameMgr");
		obj.OnConnect();
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

