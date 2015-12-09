using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class LuaBehaviourWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Awake", Awake),
			new LuaMethod("InitLuaFile", InitLuaFile),
			new LuaMethod("Start", Start),
			new LuaMethod("OnEnable", OnEnable),
			new LuaMethod("OnClick", OnClick),
			new LuaMethod("OnHold", OnHold),
			new LuaMethod("OnCommand", OnCommand),
			new LuaMethod("New", _CreateLuaBehaviour),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("luaFilename", get_luaFilename, set_luaFilename),
		};

		LuaScriptMgr.RegisterLib(L, "LuaBehaviour", typeof(LuaBehaviour), regs, fields, typeof(LuaComponent));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateLuaBehaviour(IntPtr L)
	{
		LuaDLL.luaL_error(L, "LuaBehaviour class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(LuaBehaviour);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_luaFilename(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name luaFilename");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index luaFilename on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.luaFilename);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_luaFilename(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name luaFilename");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index luaFilename on a nil value");
			}
		}

		obj.luaFilename = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Awake(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		obj.Awake();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitLuaFile(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		obj.InitLuaFile();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Start(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		obj.Start();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnEnable(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		obj.OnEnable();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnClick(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		obj.OnClick();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnHold(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.OnHold(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnCommand(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		object[] objs1 = LuaScriptMgr.GetArrayObject<object>(L, 3);
		obj.OnCommand(arg0,objs1);
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

