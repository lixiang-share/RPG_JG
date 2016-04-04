using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class SkillMgrWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Init", Init),
			new LuaMethod("ComposeSkill", ComposeSkill),
			new LuaMethod("GetTaskByID", GetTaskByID),
			new LuaMethod("New", _CreateSkillMgr),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("Instance", get_Instance, set_Instance),
		};

		LuaScriptMgr.RegisterLib(L, "SkillMgr", typeof(SkillMgr), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSkillMgr(IntPtr L)
	{
		LuaDLL.luaL_error(L, "SkillMgr class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(SkillMgr);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Instance(IntPtr L)
	{
		LuaScriptMgr.Push(L, SkillMgr.Instance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Instance(IntPtr L)
	{
		SkillMgr.Instance = (SkillMgr)LuaScriptMgr.GetUnityObject(L, 3, typeof(SkillMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Init(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SkillMgr obj = (SkillMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SkillMgr");
		obj.Init();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ComposeSkill(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SkillMgr obj = (SkillMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SkillMgr");
		SkillItem arg0 = (SkillItem)LuaScriptMgr.GetNetObject(L, 2, typeof(SkillItem));
		SkillItem o = obj.ComposeSkill(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTaskByID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SkillMgr obj = (SkillMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SkillMgr");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		SkillItem o = obj.GetTaskByID(arg0);
		LuaScriptMgr.PushObject(L, o);
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

