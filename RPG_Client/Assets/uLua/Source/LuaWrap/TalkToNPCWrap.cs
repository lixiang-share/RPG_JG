using System;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class TalkToNPCWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Talk", Talk),
			new LuaMethod("Next", Next),
			new LuaMethod("New", _CreateTalkToNPC),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("Instance", get_Instance, set_Instance),
			new LuaField("l_content", get_l_content, set_l_content),
			new LuaField("l_btnDesc", get_l_btnDesc, set_l_btnDesc),
		};

		LuaScriptMgr.RegisterLib(L, "TalkToNPC", typeof(TalkToNPC), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateTalkToNPC(IntPtr L)
	{
		LuaDLL.luaL_error(L, "TalkToNPC class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(TalkToNPC);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Instance(IntPtr L)
	{
		LuaScriptMgr.Push(L, TalkToNPC.Instance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_l_content(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		TalkToNPC obj = (TalkToNPC)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name l_content");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index l_content on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.l_content);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_l_btnDesc(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		TalkToNPC obj = (TalkToNPC)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name l_btnDesc");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index l_btnDesc on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.l_btnDesc);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Instance(IntPtr L)
	{
		TalkToNPC.Instance = (TalkToNPC)LuaScriptMgr.GetUnityObject(L, 3, typeof(TalkToNPC));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_l_content(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		TalkToNPC obj = (TalkToNPC)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name l_content");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index l_content on a nil value");
			}
		}

		obj.l_content = (UILabel)LuaScriptMgr.GetUnityObject(L, 3, typeof(UILabel));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_l_btnDesc(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		TalkToNPC obj = (TalkToNPC)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name l_btnDesc");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index l_btnDesc on a nil value");
			}
		}

		obj.l_btnDesc = (UILabel)LuaScriptMgr.GetUnityObject(L, 3, typeof(UILabel));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Talk(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		TalkToNPC obj = (TalkToNPC)LuaScriptMgr.GetUnityObjectSelf(L, 1, "TalkToNPC");
		List<string> arg0 = (List<string>)LuaScriptMgr.GetNetObject(L, 2, typeof(List<string>));
		OnTalkFinish arg1 = null;
		LuaTypes funcType3 = LuaDLL.lua_type(L, 3);

		if (funcType3 != LuaTypes.LUA_TFUNCTION)
		{
			 arg1 = (OnTalkFinish)LuaScriptMgr.GetNetObject(L, 3, typeof(OnTalkFinish));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 3);
			arg1 = () =>
			{
				func.Call();
			};
		}

		obj.Talk(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Next(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		TalkToNPC obj = (TalkToNPC)LuaScriptMgr.GetUnityObjectSelf(L, 1, "TalkToNPC");
		obj.Next();
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

