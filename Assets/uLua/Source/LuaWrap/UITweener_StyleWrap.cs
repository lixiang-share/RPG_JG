using System;
using LuaInterface;

public class UITweener_StyleWrap
{
	static LuaMethod[] enums = new LuaMethod[]
	{
		new LuaMethod("Once", GetOnce),
		new LuaMethod("Loop", GetLoop),
		new LuaMethod("PingPong", GetPingPong),
		new LuaMethod("IntToEnum", IntToEnum),
	};

	public static void Register(IntPtr L)
	{
		LuaScriptMgr.RegisterLib(L, "UITweener.Style", typeof(UITweener.Style), enums);
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetOnce(IntPtr L)
	{
		LuaScriptMgr.Push(L, UITweener.Style.Once);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetLoop(IntPtr L)
	{
		LuaScriptMgr.Push(L, UITweener.Style.Loop);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPingPong(IntPtr L)
	{
		LuaScriptMgr.Push(L, UITweener.Style.PingPong);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IntToEnum(IntPtr L)
	{
		int arg0 = (int)LuaDLL.lua_tonumber(L, 1);
		UITweener.Style o = (UITweener.Style)arg0;
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

