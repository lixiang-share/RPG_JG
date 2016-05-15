using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

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
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("AUDIO_BTN", get_AUDIO_BTN, null),
			new LuaField("Man_Comm_Attack01", get_Man_Comm_Attack01, null),
			new LuaField("Man_Comm_Attack02", get_Man_Comm_Attack02, null),
			new LuaField("Man_Comm_Attack03", get_Man_Comm_Attack03, null),
			new LuaField("Hurt", get_Hurt, null),
			new LuaField("SKill_One", get_SKill_One, null),
			new LuaField("Skill_Two", get_Skill_Two, null),
			new LuaField("instance", get_instance, set_instance),
			new LuaField("Instance", get_Instance, null),
		};

		LuaScriptMgr.RegisterLib(L, "AudioManager", typeof(AudioManager), regs, fields, typeof(MonoBehaviour));
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
	static int get_Man_Comm_Attack01(IntPtr L)
	{
		LuaScriptMgr.Push(L, AudioManager.Man_Comm_Attack01);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Man_Comm_Attack02(IntPtr L)
	{
		LuaScriptMgr.Push(L, AudioManager.Man_Comm_Attack02);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Man_Comm_Attack03(IntPtr L)
	{
		LuaScriptMgr.Push(L, AudioManager.Man_Comm_Attack03);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Hurt(IntPtr L)
	{
		LuaScriptMgr.Push(L, AudioManager.Hurt);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_SKill_One(IntPtr L)
	{
		LuaScriptMgr.Push(L, AudioManager.SKill_One);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Skill_Two(IntPtr L)
	{
		LuaScriptMgr.Push(L, AudioManager.Skill_Two);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_instance(IntPtr L)
	{
		LuaScriptMgr.Push(L, AudioManager.instance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Instance(IntPtr L)
	{
		LuaScriptMgr.Push(L, AudioManager.Instance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_instance(IntPtr L)
	{
		AudioManager.instance = (AudioManager)LuaScriptMgr.GetUnityObject(L, 3, typeof(AudioManager));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Init(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		AudioManager obj = (AudioManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AudioManager");
		obj.Init();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadAllAudio(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		AudioManager obj = (AudioManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AudioManager");
		obj.LoadAllAudio();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayAudio(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		AudioManager obj = (AudioManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AudioManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.PlayAudio(arg0);
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

