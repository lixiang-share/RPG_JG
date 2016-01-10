using System;
using UnityEngine;
using LuaInterface;

public class AudioManagerWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("LoadAllAudio", LoadAllAudio),
			new LuaMethod("LoadAudioClip", LoadAudioClip),
			new LuaMethod("PlayBtnSounds", PlayBtnSounds),
			new LuaMethod("New", _CreateAudioManager),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("AUDIO_BTN", get_AUDIO_BTN, null),
			new LuaField("instance", get_instance, set_instance),
			new LuaField("Instance", get_Instance, null),
		};

		LuaScriptMgr.RegisterLib(L, "AudioManager", typeof(AudioManager), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateAudioManager(IntPtr L)
	{
		LuaDLL.luaL_error(L, "AudioManager class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(AudioManager);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AUDIO_BTN(IntPtr L)
	{
		LuaScriptMgr.Push(L, AudioManager.AUDIO_BTN);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_instance(IntPtr L)
	{
		LuaScriptMgr.PushObject(L, AudioManager.instance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Instance(IntPtr L)
	{
		LuaScriptMgr.PushObject(L, AudioManager.Instance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_instance(IntPtr L)
	{
		AudioManager.instance = (AudioManager)LuaScriptMgr.GetNetObject(L, 3, typeof(AudioManager));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadAllAudio(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		AudioManager obj = (AudioManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "AudioManager");
		obj.LoadAllAudio();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadAudioClip(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		AudioManager obj = (AudioManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "AudioManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		AudioClip o = obj.LoadAudioClip(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayBtnSounds(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		AudioManager obj = (AudioManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "AudioManager");
		obj.PlayBtnSounds();
		return 0;
	}
}

