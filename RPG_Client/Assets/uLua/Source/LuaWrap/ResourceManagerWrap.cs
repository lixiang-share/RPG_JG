using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class ResourceManagerWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("LoadFightEffect", LoadFightEffect),
			new LuaMethod("LoadPrefab", LoadPrefab),
			new LuaMethod("LoadMesPrefab", LoadMesPrefab),
			new LuaMethod("Init", Init),
			new LuaMethod("New", _CreateResourceManager),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("Instance", get_Instance, null),
		};

		LuaScriptMgr.RegisterLib(L, "ResourceManager", typeof(ResourceManager), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateResourceManager(IntPtr L)
	{
		LuaDLL.luaL_error(L, "ResourceManager class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(ResourceManager);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Instance(IntPtr L)
	{
		LuaScriptMgr.PushObject(L, ResourceManager.Instance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadFightEffect(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		ResourceManager obj = (ResourceManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "ResourceManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		GameObject o = obj.LoadFightEffect(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadPrefab(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		ResourceManager obj = (ResourceManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "ResourceManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		Object o = obj.LoadPrefab(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadMesPrefab(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		ResourceManager obj = (ResourceManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "ResourceManager");
		Object o = obj.LoadMesPrefab();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Init(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		ResourceManager obj = (ResourceManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "ResourceManager");
		obj.Init();
		return 0;
	}
}

