using System;
using LuaInterface;

public class AudioManagerWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Init", Init),
			new LuaMethod("LoadAllAudio", LoadAllAudio),
			new LuaMethod("PlayAudio", PlayAudio),
			new LuaMethod("New", _CreateAudioManager),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("AUDIO_BTN", get_AUDIO_BTN, null),
			new LuaField("Man_Comm_Attack01", get_Man_Comm_Attack01, null),
			new LuaField("instance", get_instance, set_instance),
			new LuaField("Instance", get_Instance, null),
		};

		LuaScriptMgr.RegisterLib(L, "AudioManager", typeof(AudioManager), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateAudioManager(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			AudioManager obj = new AudioManager();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: AudioManager.New");
		}

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
	static int get_Man_Comm_Attack01(IntPtr L)
	{
		LuaScriptMgr.Push(L, AudioManager.Man_Comm_Attack01);
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
	static int Init(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		AudioManager obj = (AudioManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "AudioManager");
		obj.Init();
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
	static int PlayAudio(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		AudioManager obj = (AudioManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "AudioManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.PlayAudio(arg0);
		return 0;
	}
}

