using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class ScaleButtonWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("OnEnable", OnEnable),
			new LuaMethod("OnHold", OnHold),
			new LuaMethod("New", _CreateScaleButton),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("scaleFactor", get_scaleFactor, set_scaleFactor),
		};

		LuaScriptMgr.RegisterLib(L, "ScaleButton", typeof(ScaleButton), regs, fields, typeof(LuaBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateScaleButton(IntPtr L)
	{
		LuaDLL.luaL_error(L, "ScaleButton class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(ScaleButton);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_scaleFactor(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ScaleButton obj = (ScaleButton)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name scaleFactor");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index scaleFactor on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.scaleFactor);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_scaleFactor(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ScaleButton obj = (ScaleButton)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name scaleFactor");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index scaleFactor on a nil value");
			}
		}

		obj.scaleFactor = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnEnable(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		ScaleButton obj = (ScaleButton)LuaScriptMgr.GetUnityObjectSelf(L, 1, "ScaleButton");
		obj.OnEnable();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnHold(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		ScaleButton obj = (ScaleButton)LuaScriptMgr.GetUnityObjectSelf(L, 1, "ScaleButton");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.OnHold(arg0);
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

