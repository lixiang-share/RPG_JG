using System;
using LuaInterface;

public class MsgPackerWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Serialize", Serialize),
			new LuaMethod("close", close),
			new LuaMethod("SetType", SetType),
			new LuaMethod("New", _CreateMsgPacker),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("MsgType", get_MsgType, set_MsgType),
			new LuaField("IsSetType", get_IsSetType, set_IsSetType),
			new LuaField("IsNeedRecv", get_IsNeedRecv, set_IsNeedRecv),
		};

		LuaScriptMgr.RegisterLib(L, "MsgPacker", typeof(MsgPacker), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateMsgPacker(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			MsgPacker obj = new MsgPacker();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: MsgPacker.New");
		}

		return 0;
	}

	static Type classType = typeof(MsgPacker);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_MsgType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		MsgPacker obj = (MsgPacker)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name MsgType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index MsgType on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.MsgType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsSetType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		MsgPacker obj = (MsgPacker)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsSetType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsSetType on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsSetType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsNeedRecv(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		MsgPacker obj = (MsgPacker)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsNeedRecv");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsNeedRecv on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsNeedRecv);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_MsgType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		MsgPacker obj = (MsgPacker)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name MsgType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index MsgType on a nil value");
			}
		}

		obj.MsgType = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_IsSetType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		MsgPacker obj = (MsgPacker)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsSetType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsSetType on a nil value");
			}
		}

		obj.IsSetType = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_IsNeedRecv(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		MsgPacker obj = (MsgPacker)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsNeedRecv");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsNeedRecv on a nil value");
			}
		}

		obj.IsNeedRecv = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Serialize(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MsgPacker obj = (MsgPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgPacker");
		byte[] o = obj.Serialize();
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int close(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MsgPacker obj = (MsgPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgPacker");
		obj.close();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetType(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MsgPacker obj = (MsgPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgPacker");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		MsgPacker o = obj.SetType(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}
}

