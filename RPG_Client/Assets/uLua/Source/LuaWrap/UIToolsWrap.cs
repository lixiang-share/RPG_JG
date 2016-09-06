using System;
using UnityEngine;
using System.Collections;
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
			new LuaMethod("SA", SA),
			new LuaMethod("D", D),
			new LuaMethod("GetLuaPathInEditor", GetLuaPathInEditor),
			new LuaMethod("isLuaFileExits", isLuaFileExits),
			new LuaMethod("Compile", Compile),
			new LuaMethod("ShowMsg", ShowMsg),
			new LuaMethod("Set", Set),
			new LuaMethod("SetColor", SetColor),
			new LuaMethod("DimSprite", DimSprite),
			new LuaMethod("ShowPanel", ShowPanel),
			new LuaMethod("ClosePanel", ClosePanel),
			new LuaMethod("HandlePanel", HandlePanel),
			new LuaMethod("TweenPos_X", TweenPos_X),
			new LuaMethod("TweenPos_Y", TweenPos_Y),
			new LuaMethod("TweenPos", TweenPos),
			new LuaMethod("Tween_Scale", Tween_Scale),
			new LuaMethod("isValidString", isValidString),
			new LuaMethod("StoreSessionKey", StoreSessionKey),
			new LuaMethod("GetSessionKey", GetSessionKey),
			new LuaMethod("StoreString", StoreString),
			new LuaMethod("StoreInt", StoreInt),
			new LuaMethod("StoreFloat", StoreFloat),
			new LuaMethod("GetString", GetString),
			new LuaMethod("GetInt", GetInt),
			new LuaMethod("GetFloat", GetFloat),
			new LuaMethod("MsgToServerList", MsgToServerList),
			new LuaMethod("MsgToRoleList", MsgToRoleList),
			new LuaMethod("MsgToTaskList", MsgToTaskList),
			new LuaMethod("MsgToPlayer", MsgToPlayer),
			new LuaMethod("MsgToEquipItem", MsgToEquipItem),
			new LuaMethod("MsgToSkillList", MsgToSkillList),
			new LuaMethod("BeginFight", BeginFight),
			new LuaMethod("EndFightAndGoMainCity", EndFightAndGoMainCity),
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

		LuaScriptMgr.RegisterLib(L, "UITools", typeof(UITools), regs, fields, null);
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUITools(IntPtr L)
	{
		LuaDLL.luaL_error(L, "UITools class does not have a constructor function");
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
		object arg0 = LuaScriptMgr.GetVarObject(L, 1);
		UITools.Log(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogError(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		object arg0 = LuaScriptMgr.GetVarObject(L, 1);
		UITools.LogError(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogWarning(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		object arg0 = LuaScriptMgr.GetVarObject(L, 1);
		UITools.LogWarning(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SA(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(GameObject), typeof(bool)))
		{
			GameObject arg0 = (GameObject)LuaScriptMgr.GetLuaObject(L, 1);
			bool arg1 = LuaDLL.lua_toboolean(L, 2);
			UITools.SA(arg0,arg1);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(LuaBehaviour), typeof(bool)))
		{
			LuaBehaviour arg0 = (LuaBehaviour)LuaScriptMgr.GetLuaObject(L, 1);
			bool arg1 = LuaDLL.lua_toboolean(L, 2);
			UITools.SA(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UITools.SA");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int D(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		LuaBehaviour o = UITools.D(arg0);
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

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowMsg(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		UITools.ShowMsg(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Set(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(LuaBehaviour), typeof(string)))
		{
			LuaBehaviour arg0 = (LuaBehaviour)LuaScriptMgr.GetLuaObject(L, 1);
			string arg1 = LuaScriptMgr.GetString(L, 2);
			UITools.Set(arg0,arg1);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(GameObject), typeof(object)))
		{
			GameObject arg0 = (GameObject)LuaScriptMgr.GetLuaObject(L, 1);
			object arg1 = LuaScriptMgr.GetVarObject(L, 2);
			UITools.Set(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UITools.Set");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetColor(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		LuaBehaviour arg0 = (LuaBehaviour)LuaScriptMgr.GetUnityObject(L, 1, typeof(LuaBehaviour));
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 3);
		float arg3 = (float)LuaScriptMgr.GetNumber(L, 4);
		UITools.SetColor(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DimSprite(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaBehaviour arg0 = (LuaBehaviour)LuaScriptMgr.GetUnityObject(L, 1, typeof(LuaBehaviour));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		UITools.DimSprite(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowPanel(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			LuaBehaviour arg0 = (LuaBehaviour)LuaScriptMgr.GetUnityObject(L, 1, typeof(LuaBehaviour));
			UITools.ShowPanel(arg0);
			return 0;
		}
		else if (count == 2)
		{
			LuaBehaviour arg0 = (LuaBehaviour)LuaScriptMgr.GetUnityObject(L, 1, typeof(LuaBehaviour));
			float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
			UITools.ShowPanel(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UITools.ShowPanel");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClosePanel(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			LuaBehaviour arg0 = (LuaBehaviour)LuaScriptMgr.GetUnityObject(L, 1, typeof(LuaBehaviour));
			UITools.ClosePanel(arg0);
			return 0;
		}
		else if (count == 2)
		{
			LuaBehaviour arg0 = (LuaBehaviour)LuaScriptMgr.GetUnityObject(L, 1, typeof(LuaBehaviour));
			float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
			UITools.ClosePanel(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UITools.ClosePanel");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HandlePanel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		LuaBehaviour arg0 = (LuaBehaviour)LuaScriptMgr.GetUnityObject(L, 1, typeof(LuaBehaviour));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 3);
		UITools.HandlePanel(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TweenPos_X(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			LuaBehaviour arg0 = (LuaBehaviour)LuaScriptMgr.GetUnityObject(L, 1, typeof(LuaBehaviour));
			float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
			UITools.TweenPos_X(arg0,arg1);
			return 0;
		}
		else if (count == 3)
		{
			LuaBehaviour arg0 = (LuaBehaviour)LuaScriptMgr.GetUnityObject(L, 1, typeof(LuaBehaviour));
			float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
			float arg2 = (float)LuaScriptMgr.GetNumber(L, 3);
			UITools.TweenPos_X(arg0,arg1,arg2);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UITools.TweenPos_X");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TweenPos_Y(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			LuaBehaviour arg0 = (LuaBehaviour)LuaScriptMgr.GetUnityObject(L, 1, typeof(LuaBehaviour));
			float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
			UITools.TweenPos_Y(arg0,arg1);
			return 0;
		}
		else if (count == 3)
		{
			LuaBehaviour arg0 = (LuaBehaviour)LuaScriptMgr.GetUnityObject(L, 1, typeof(LuaBehaviour));
			float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
			float arg2 = (float)LuaScriptMgr.GetNumber(L, 3);
			UITools.TweenPos_Y(arg0,arg1,arg2);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UITools.TweenPos_Y");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TweenPos(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		Vector3 arg1 = LuaScriptMgr.GetVector3(L, 2);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 3);
		UITools.TweenPos(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Tween_Scale(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 3)
		{
			LuaBehaviour arg0 = (LuaBehaviour)LuaScriptMgr.GetUnityObject(L, 1, typeof(LuaBehaviour));
			float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
			float arg2 = (float)LuaScriptMgr.GetNumber(L, 3);
			UITools.Tween_Scale(arg0,arg1,arg2);
			return 0;
		}
		else if (count == 5)
		{
			LuaBehaviour arg0 = (LuaBehaviour)LuaScriptMgr.GetUnityObject(L, 1, typeof(LuaBehaviour));
			float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
			float arg2 = (float)LuaScriptMgr.GetNumber(L, 3);
			float arg3 = (float)LuaScriptMgr.GetNumber(L, 4);
			float arg4 = (float)LuaScriptMgr.GetNumber(L, 5);
			UITools.Tween_Scale(arg0,arg1,arg2,arg3,arg4);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UITools.Tween_Scale");
		}

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
	static int StoreSessionKey(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		UITools.StoreSessionKey(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetSessionKey(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = UITools.GetSessionKey();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StoreString(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		UITools.StoreString(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StoreInt(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		UITools.StoreInt(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StoreFloat(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
		UITools.StoreFloat(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetString(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = UITools.GetString(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetInt(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		int o = UITools.GetInt(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetFloat(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		float o = UITools.GetFloat(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MsgToServerList(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MsgUnPacker arg0 = (MsgUnPacker)LuaScriptMgr.GetNetObject(L, 1, typeof(MsgUnPacker));
		IList o = UITools.MsgToServerList(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MsgToRoleList(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MsgUnPacker arg0 = (MsgUnPacker)LuaScriptMgr.GetNetObject(L, 1, typeof(MsgUnPacker));
		IList o = UITools.MsgToRoleList(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MsgToTaskList(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MsgUnPacker arg0 = (MsgUnPacker)LuaScriptMgr.GetNetObject(L, 1, typeof(MsgUnPacker));
		IList o = UITools.MsgToTaskList(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MsgToPlayer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MsgUnPacker arg0 = (MsgUnPacker)LuaScriptMgr.GetNetObject(L, 1, typeof(MsgUnPacker));
		Player o = UITools.MsgToPlayer(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MsgToEquipItem(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MsgUnPacker arg0 = (MsgUnPacker)LuaScriptMgr.GetNetObject(L, 1, typeof(MsgUnPacker));
		IList o = UITools.MsgToEquipItem(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MsgToSkillList(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MsgUnPacker arg0 = (MsgUnPacker)LuaScriptMgr.GetNetObject(L, 1, typeof(MsgUnPacker));
		IList o = UITools.MsgToSkillList(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int BeginFight(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		UITools.BeginFight();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int EndFightAndGoMainCity(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		UITools.EndFightAndGoMainCity();
		return 0;
	}
}

