using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class PlayerMainCityCtrlWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("DoTask", DoTask),
			new LuaMethod("ClaimTask", ClaimTask),
			new LuaMethod("AcceptTask", AcceptTask),
			new LuaMethod("NextTask", NextTask),
			new LuaMethod("ClaimRewards", ClaimRewards),
			new LuaMethod("Finish", Finish),
			new LuaMethod("New", _CreatePlayerMainCityCtrl),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("minResponseVal", get_minResponseVal, set_minResponseVal),
			new LuaField("isAbleMove", get_isAbleMove, set_isAbleMove),
			new LuaField("Instance", get_Instance, set_Instance),
			new LuaField("speed", get_speed, set_speed),
			new LuaField("go", get_go, set_go),
		};

		LuaScriptMgr.RegisterLib(L, "PlayerMainCityCtrl", typeof(PlayerMainCityCtrl), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreatePlayerMainCityCtrl(IntPtr L)
	{
		LuaDLL.luaL_error(L, "PlayerMainCityCtrl class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(PlayerMainCityCtrl);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_minResponseVal(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PlayerMainCityCtrl obj = (PlayerMainCityCtrl)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name minResponseVal");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index minResponseVal on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.minResponseVal);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isAbleMove(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PlayerMainCityCtrl obj = (PlayerMainCityCtrl)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name isAbleMove");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index isAbleMove on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.isAbleMove);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Instance(IntPtr L)
	{
		LuaScriptMgr.Push(L, PlayerMainCityCtrl.Instance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_speed(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PlayerMainCityCtrl obj = (PlayerMainCityCtrl)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name speed");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index speed on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.speed);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_go(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PlayerMainCityCtrl obj = (PlayerMainCityCtrl)o;

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
	static int set_minResponseVal(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PlayerMainCityCtrl obj = (PlayerMainCityCtrl)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name minResponseVal");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index minResponseVal on a nil value");
			}
		}

		obj.minResponseVal = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_isAbleMove(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PlayerMainCityCtrl obj = (PlayerMainCityCtrl)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name isAbleMove");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index isAbleMove on a nil value");
			}
		}

		obj.isAbleMove = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Instance(IntPtr L)
	{
		PlayerMainCityCtrl.Instance = (PlayerMainCityCtrl)LuaScriptMgr.GetUnityObject(L, 3, typeof(PlayerMainCityCtrl));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_speed(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PlayerMainCityCtrl obj = (PlayerMainCityCtrl)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name speed");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index speed on a nil value");
			}
		}

		obj.speed = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_go(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PlayerMainCityCtrl obj = (PlayerMainCityCtrl)o;

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
	static int DoTask(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		PlayerMainCityCtrl obj = (PlayerMainCityCtrl)LuaScriptMgr.GetUnityObjectSelf(L, 1, "PlayerMainCityCtrl");
		TaskEntity arg0 = (TaskEntity)LuaScriptMgr.GetNetObject(L, 2, typeof(TaskEntity));
		obj.DoTask(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClaimTask(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		PlayerMainCityCtrl obj = (PlayerMainCityCtrl)LuaScriptMgr.GetUnityObjectSelf(L, 1, "PlayerMainCityCtrl");
		TaskEntity arg0 = (TaskEntity)LuaScriptMgr.GetNetObject(L, 2, typeof(TaskEntity));
		obj.ClaimTask(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AcceptTask(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		PlayerMainCityCtrl obj = (PlayerMainCityCtrl)LuaScriptMgr.GetUnityObjectSelf(L, 1, "PlayerMainCityCtrl");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		obj.AcceptTask(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int NextTask(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		PlayerMainCityCtrl obj = (PlayerMainCityCtrl)LuaScriptMgr.GetUnityObjectSelf(L, 1, "PlayerMainCityCtrl");
		TaskEntity arg0 = (TaskEntity)LuaScriptMgr.GetNetObject(L, 2, typeof(TaskEntity));
		obj.NextTask(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClaimRewards(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		PlayerMainCityCtrl obj = (PlayerMainCityCtrl)LuaScriptMgr.GetUnityObjectSelf(L, 1, "PlayerMainCityCtrl");
		TaskEntity arg0 = (TaskEntity)LuaScriptMgr.GetNetObject(L, 2, typeof(TaskEntity));
		obj.ClaimRewards(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Finish(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		PlayerMainCityCtrl obj = (PlayerMainCityCtrl)LuaScriptMgr.GetUnityObjectSelf(L, 1, "PlayerMainCityCtrl");
		TaskEntity arg0 = (TaskEntity)LuaScriptMgr.GetNetObject(L, 2, typeof(TaskEntity));
		obj.Finish(arg0);
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

