using System;
using System.Collections.Generic;
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
			new LuaMethod("OnDisable", OnDisable),
			new LuaMethod("set_Item", set_Item),
			new LuaMethod("get_Item", get_Item),
			new LuaMethod("S", S),
			new LuaMethod("G", G),
			new LuaMethod("New", _CreateLuaBehaviour),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("luaFilename", get_luaFilename, set_luaFilename),
			new LuaField("tableName", get_tableName, set_tableName),
			new LuaField("domain", get_domain, set_domain),
			new LuaField("Domains", get_Domains, set_Domains),
			new LuaField("varDict", get_varDict, set_varDict),
			new LuaField("varList", get_varList, set_varList),
			new LuaField("facade", get_facade, null),
			new LuaField("LuaMgr", get_LuaMgr, set_LuaMgr),
			new LuaField("ResManager", get_ResManager, null),
			new LuaField("NetManager", get_NetManager, null),
			new LuaField("AudioMgr", get_AudioMgr, null),
			new LuaField("TimerManger", get_TimerManger, null),
			new LuaField("ThreadManager", get_ThreadManager, null),
			new LuaField("Parent", get_Parent, null),
		};

		LuaScriptMgr.RegisterLib(L, "LuaBehaviour", typeof(LuaBehaviour), regs, fields, typeof(MonoBehaviour));
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
	static int get_tableName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name tableName");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index tableName on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.tableName);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_domain(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name domain");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index domain on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.domain);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Domains(IntPtr L)
	{
		LuaScriptMgr.PushObject(L, LuaBehaviour.Domains);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_varDict(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name varDict");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index varDict on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.varDict);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_varList(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name varList");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index varList on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.varList);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_facade(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name facade");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index facade on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.facade);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_LuaMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name LuaMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index LuaMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.LuaMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ResManager(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ResManager");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ResManager on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.ResManager);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_NetManager(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name NetManager");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index NetManager on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.NetManager);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AudioMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name AudioMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index AudioMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.AudioMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TimerManger(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name TimerManger");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index TimerManger on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.TimerManger);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ThreadManager(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ThreadManager");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ThreadManager on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.ThreadManager);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Parent(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Parent");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Parent on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Parent);
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
	static int set_tableName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name tableName");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index tableName on a nil value");
			}
		}

		obj.tableName = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_domain(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name domain");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index domain on a nil value");
			}
		}

		obj.domain = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Domains(IntPtr L)
	{
		LuaBehaviour.Domains = (Dictionary<string,LuaBehaviour>)LuaScriptMgr.GetNetObject(L, 3, typeof(Dictionary<string,LuaBehaviour>));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_varDict(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name varDict");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index varDict on a nil value");
			}
		}

		obj.varDict = (Dictionary<string,object>)LuaScriptMgr.GetNetObject(L, 3, typeof(Dictionary<string,object>));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_varList(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name varList");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index varList on a nil value");
			}
		}

		obj.varList = (List<ParamInspector>)LuaScriptMgr.GetNetObject(L, 3, typeof(List<ParamInspector>));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_LuaMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name LuaMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index LuaMgr on a nil value");
			}
		}

		obj.LuaMgr = (LuaManager)LuaScriptMgr.GetNetObject(L, 3, typeof(LuaManager));
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
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			obj.OnCommand(arg0);
			return 0;
		}
		else if (count == 3)
		{
			LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			object arg1 = LuaScriptMgr.GetVarObject(L, 3);
			obj.OnCommand(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: LuaBehaviour.OnCommand");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnDisable(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		obj.OnDisable();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Item(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		object arg1 = LuaScriptMgr.GetVarObject(L, 3);
		obj[arg0] = arg1;
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Item(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		object o = obj[arg0];
		LuaScriptMgr.PushVarObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int S(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		object arg1 = LuaScriptMgr.GetVarObject(L, 3);
		obj.S(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int G(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		object o = obj.G(arg0);
		LuaScriptMgr.PushVarObject(L, o);
		return 1;
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

