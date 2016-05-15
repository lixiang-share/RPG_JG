using System;
using System.Collections.Generic;
using System.Collections;
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
			new LuaMethod("Parse", Parse),
			new LuaMethod("DoDelay", DoDelay),
			new LuaMethod("OnEnable", OnEnable),
			new LuaMethod("CallLuaMethod", CallLuaMethod),
			new LuaMethod("ExcuteCommand", ExcuteCommand),
			new LuaMethod("Start", Start),
			new LuaMethod("OnClick", OnClick),
			new LuaMethod("OnHold", OnHold),
			new LuaMethod("OnCommand", OnCommand),
			new LuaMethod("OnDisable", OnDisable),
			new LuaMethod("ReceiveData", ReceiveData),
			new LuaMethod("SendMsg", SendMsg),
			new LuaMethod("CreateMsg", CreateMsg),
			new LuaMethod("SetMsgType", SetMsgType),
			new LuaMethod("AddInt", AddInt),
			new LuaMethod("AddBool", AddBool),
			new LuaMethod("AddFloat", AddFloat),
			new LuaMethod("AddString", AddString),
			new LuaMethod("Send", Send),
			new LuaMethod("set_Item", set_Item),
			new LuaMethod("get_Item", get_Item),
			new LuaMethod("S", S),
			new LuaMethod("G", G),
			new LuaMethod("C", C),
			new LuaMethod("GetChild", GetChild),
			new LuaMethod("Child", Child),
			new LuaMethod("SetX", SetX),
			new LuaMethod("SetY", SetY),
			new LuaMethod("SetPos", SetPos),
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
			new LuaField("doStringLuaFile", get_doStringLuaFile, set_doStringLuaFile),
			new LuaField("isDoString", get_isDoString, set_isDoString),
			new LuaField("lua_OnClick", get_lua_OnClick, set_lua_OnClick),
			new LuaField("lua_OnDisable", get_lua_OnDisable, set_lua_OnDisable),
			new LuaField("lua_OnCommand", get_lua_OnCommand, set_lua_OnCommand),
			new LuaField("lua_Awake", get_lua_Awake, set_lua_Awake),
			new LuaField("lua_Start", get_lua_Start, set_lua_Start),
			new LuaField("lus_OnFirstEnable", get_lus_OnFirstEnable, set_lus_OnFirstEnable),
			new LuaField("lua_OnEnable", get_lua_OnEnable, set_lua_OnEnable),
			new LuaField("lua_OnHold", get_lua_OnHold, set_lua_OnHold),
			new LuaField("lua_OnReceiveData", get_lua_OnReceiveData, set_lua_OnReceiveData),
			new LuaField("lua_OnFirstEnable", get_lua_OnFirstEnable, set_lua_OnFirstEnable),
			new LuaField("LuaMgr", get_LuaMgr, null),
			new LuaField("ResManager", get_ResManager, null),
			new LuaField("NetManager", get_NetManager, null),
			new LuaField("AudioMgr", get_AudioMgr, null),
			new LuaField("GameMgr", get_GameMgr, null),
			new LuaField("PlayerMgr", get_PlayerMgr, null),
			new LuaField("Packer", get_Packer, set_Packer),
			new LuaField("Value", get_Value, set_Value),
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
	static int get_doStringLuaFile(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name doStringLuaFile");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index doStringLuaFile on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.doStringLuaFile);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isDoString(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name isDoString");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index isDoString on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.isDoString);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lua_OnClick(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lua_OnClick");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lua_OnClick on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.lua_OnClick);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lua_OnDisable(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lua_OnDisable");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lua_OnDisable on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.lua_OnDisable);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lua_OnCommand(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lua_OnCommand");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lua_OnCommand on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.lua_OnCommand);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lua_Awake(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lua_Awake");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lua_Awake on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.lua_Awake);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lua_Start(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lua_Start");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lua_Start on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.lua_Start);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lus_OnFirstEnable(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lus_OnFirstEnable");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lus_OnFirstEnable on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.lus_OnFirstEnable);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lua_OnEnable(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lua_OnEnable");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lua_OnEnable on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.lua_OnEnable);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lua_OnHold(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lua_OnHold");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lua_OnHold on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.lua_OnHold);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lua_OnReceiveData(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lua_OnReceiveData");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lua_OnReceiveData on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.lua_OnReceiveData);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lua_OnFirstEnable(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lua_OnFirstEnable");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lua_OnFirstEnable on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.lua_OnFirstEnable);
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

		LuaScriptMgr.Push(L, obj.AudioMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_GameMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name GameMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index GameMgr on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.GameMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_PlayerMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name PlayerMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index PlayerMgr on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.PlayerMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Packer(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Packer");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Packer on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.Packer);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Value(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Value");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Value on a nil value");
			}
		}

		LuaScriptMgr.PushVarObject(L, obj.Value);
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
	static int set_doStringLuaFile(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name doStringLuaFile");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index doStringLuaFile on a nil value");
			}
		}

		obj.doStringLuaFile = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_isDoString(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name isDoString");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index isDoString on a nil value");
			}
		}

		obj.isDoString = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_lua_OnClick(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lua_OnClick");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lua_OnClick on a nil value");
			}
		}

		obj.lua_OnClick = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_lua_OnDisable(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lua_OnDisable");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lua_OnDisable on a nil value");
			}
		}

		obj.lua_OnDisable = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_lua_OnCommand(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lua_OnCommand");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lua_OnCommand on a nil value");
			}
		}

		obj.lua_OnCommand = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_lua_Awake(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lua_Awake");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lua_Awake on a nil value");
			}
		}

		obj.lua_Awake = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_lua_Start(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lua_Start");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lua_Start on a nil value");
			}
		}

		obj.lua_Start = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_lus_OnFirstEnable(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lus_OnFirstEnable");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lus_OnFirstEnable on a nil value");
			}
		}

		obj.lus_OnFirstEnable = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_lua_OnEnable(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lua_OnEnable");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lua_OnEnable on a nil value");
			}
		}

		obj.lua_OnEnable = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_lua_OnHold(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lua_OnHold");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lua_OnHold on a nil value");
			}
		}

		obj.lua_OnHold = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_lua_OnReceiveData(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lua_OnReceiveData");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lua_OnReceiveData on a nil value");
			}
		}

		obj.lua_OnReceiveData = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_lua_OnFirstEnable(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lua_OnFirstEnable");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lua_OnFirstEnable on a nil value");
			}
		}

		obj.lua_OnFirstEnable = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Packer(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Packer");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Packer on a nil value");
			}
		}

		obj.Packer = (MsgPacker)LuaScriptMgr.GetNetObject(L, 3, typeof(MsgPacker));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Value(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaBehaviour obj = (LuaBehaviour)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Value");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Value on a nil value");
			}
		}

		obj.Value = LuaScriptMgr.GetVarObject(L, 3);
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
	static int Parse(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		IList arg0 = (IList)LuaScriptMgr.GetNetObject(L, 2, typeof(IList));
		obj.Parse(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DoDelay(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
		object[] objs2 = LuaScriptMgr.GetParamsObject(L, 4, count - 3);
		obj.DoDelay(arg0,arg1,objs2);
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
	static int CallLuaMethod(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		object[] objs1 = LuaScriptMgr.GetParamsObject(L, 3, count - 2);
		object[] o = obj.CallLuaMethod(arg0,objs1);
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ExcuteCommand(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		object[] objs1 = LuaScriptMgr.GetParamsObject(L, 3, count - 2);
		object[] o = obj.ExcuteCommand(arg0,objs1);
		LuaScriptMgr.PushArray(L, o);
		return 1;
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
	static int ReceiveData(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		object arg0 = LuaScriptMgr.GetVarObject(L, 2);
		obj.ReceiveData(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SendMsg(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		MsgPacker arg0 = (MsgPacker)LuaScriptMgr.GetNetObject(L, 2, typeof(MsgPacker));
		obj.SendMsg(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CreateMsg(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		LuaBehaviour o = obj.CreateMsg();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetMsgType(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		LuaBehaviour o = obj.SetMsgType(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddInt(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		LuaBehaviour o = obj.AddInt(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddBool(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		LuaBehaviour o = obj.AddBool(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddFloat(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 2);
		LuaBehaviour o = obj.AddFloat(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddString(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		LuaBehaviour o = obj.AddString(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Send(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		obj.Send();
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
	static int C(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		Component o = obj.C(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetChild(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		LuaBehaviour o = obj.GetChild(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Child(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		GameObject o = obj.Child(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetX(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
			float arg0 = (float)LuaScriptMgr.GetNumber(L, 2);
			obj.SetX(arg0);
			return 0;
		}
		else if (count == 3)
		{
			LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
			float arg0 = (float)LuaScriptMgr.GetNumber(L, 2);
			bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
			obj.SetX(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: LuaBehaviour.SetX");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetY(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
			float arg0 = (float)LuaScriptMgr.GetNumber(L, 2);
			obj.SetY(arg0);
			return 0;
		}
		else if (count == 3)
		{
			LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
			float arg0 = (float)LuaScriptMgr.GetNumber(L, 2);
			bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
			obj.SetY(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: LuaBehaviour.SetY");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPos(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 5);
		LuaBehaviour obj = (LuaBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "LuaBehaviour");
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 2);
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 4);
		bool arg3 = LuaScriptMgr.GetBoolean(L, 5);
		obj.SetPos(arg0,arg1,arg2,arg3);
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

