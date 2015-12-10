using System;
using UnityEngine;
using LuaInterface;

public class UIToolsWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Log", Log),
			new LuaMethod("LogError", LogError),
			new LuaMethod("LogWarning", LogWarning),
			new LuaMethod("TweenPos", TweenPos),
			new LuaMethod("isValidString", isValidString),
			new LuaMethod("GetLuaPathInEditor", GetLuaPathInEditor),
			new LuaMethod("isLuaFileExits", isLuaFileExits),
			new LuaMethod("Compile", Compile),
			new LuaMethod("New", _CreateUITools),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("keys", get_keys, set_keys),
			new LuaField("values", get_values, set_values),
			new LuaField("log", get_log, null),
			new LuaField("logError", get_logError, null),
			new LuaField("logWarning", get_logWarning, null),
		};

		LuaScriptMgr.RegisterLib(L, "UITools", typeof(UITools), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUITools(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			UITools obj = new UITools();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UITools.New");
		}

		return 0;
	}

	static Type classType = typeof(UITools);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_keys(IntPtr L)
	{
		LuaScriptMgr.PushArray(L, UITools.keys);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_values(IntPtr L)
	{
		LuaScriptMgr.PushArray(L, UITools.values);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_log(IntPtr L)
	{
		LuaScriptMgr.Push(L, UITools.log);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_logError(IntPtr L)
	{
		LuaScriptMgr.Push(L, UITools.logError);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_logWarning(IntPtr L)
	{
		LuaScriptMgr.Push(L, UITools.logWarning);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_keys(IntPtr L)
	{
		UITools.keys = LuaScriptMgr.GetArrayString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_values(IntPtr L)
	{
		UITools.values = LuaScriptMgr.GetArrayString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Log(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		UITools.Log(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogError(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		UITools.LogError(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogWarning(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		UITools.LogWarning(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TweenPos(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		UITools obj = (UITools)LuaScriptMgr.GetNetObjectSelf(L, 1, "UITools");
		LuaBehaviour arg0 = (LuaBehaviour)LuaScriptMgr.GetUnityObject(L, 2, typeof(LuaBehaviour));
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
		Vector3 arg2 = LuaScriptMgr.GetVector3(L, 4);
		obj.TweenPos(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int isValidString(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		bool o = UITools.isValidString(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetLuaPathInEditor(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = UITools.GetLuaPathInEditor();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int isLuaFileExits(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		bool o = UITools.isLuaFileExits(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Compile(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		UITools.Compile(arg0);
		return 0;
	}
}

