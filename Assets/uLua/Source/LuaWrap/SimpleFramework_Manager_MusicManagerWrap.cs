using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class SimpleFramework_Manager_MusicManagerWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("LoadAudioClip", LoadAudioClip),
			new LuaMethod("CanPlayBackSound", CanPlayBackSound),
			new LuaMethod("PlayBacksound", PlayBacksound),
			new LuaMethod("CanPlaySoundEffect", CanPlaySoundEffect),
			new LuaMethod("Play", Play),
			new LuaMethod("New", _CreateSimpleFramework_Manager_MusicManager),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "SimpleFramework.Manager.MusicManager", typeof(SimpleFramework.Manager.MusicManager), regs, fields, typeof(View));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSimpleFramework_Manager_MusicManager(IntPtr L)
	{
		LuaDLL.luaL_error(L, "SimpleFramework.Manager.MusicManager class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(SimpleFramework.Manager.MusicManager);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadAudioClip(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.MusicManager obj = (SimpleFramework.Manager.MusicManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.MusicManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		AudioClip o = obj.LoadAudioClip(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CanPlayBackSound(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SimpleFramework.Manager.MusicManager obj = (SimpleFramework.Manager.MusicManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.MusicManager");
		bool o = obj.CanPlayBackSound();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayBacksound(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		SimpleFramework.Manager.MusicManager obj = (SimpleFramework.Manager.MusicManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.MusicManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
		obj.PlayBacksound(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CanPlaySoundEffect(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SimpleFramework.Manager.MusicManager obj = (SimpleFramework.Manager.MusicManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.MusicManager");
		bool o = obj.CanPlaySoundEffect();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Play(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		SimpleFramework.Manager.MusicManager obj = (SimpleFramework.Manager.MusicManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.MusicManager");
		AudioClip arg0 = (AudioClip)LuaScriptMgr.GetUnityObject(L, 2, typeof(AudioClip));
		Vector3 arg1 = LuaScriptMgr.GetVector3(L, 3);
		obj.Play(arg0,arg1);
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

