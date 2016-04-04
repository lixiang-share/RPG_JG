using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class PlayerManagerWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("UpdatePlayerinfo", UpdatePlayerinfo),
			new LuaMethod("New", _CreatePlayerManager),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("Server", get_Server, set_Server),
			new LuaField("Role", get_Role, set_Role),
			new LuaField("Inst", get_Inst, null),
			new LuaField("Player", get_Player, set_Player),
		};

		LuaScriptMgr.RegisterLib(L, "PlayerManager", typeof(PlayerManager), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreatePlayerManager(IntPtr L)
	{
		LuaDLL.luaL_error(L, "PlayerManager class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(PlayerManager);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Server(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PlayerManager obj = (PlayerManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Server");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Server on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.Server);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Role(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PlayerManager obj = (PlayerManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Role");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Role on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.Role);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Inst(IntPtr L)
	{
		LuaScriptMgr.Push(L, PlayerManager.Inst);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Player(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PlayerManager obj = (PlayerManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Player");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Player on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.Player);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Server(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PlayerManager obj = (PlayerManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Server");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Server on a nil value");
			}
		}

		obj.Server = (ServerItem)LuaScriptMgr.GetNetObject(L, 3, typeof(ServerItem));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Role(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PlayerManager obj = (PlayerManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Role");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Role on a nil value");
			}
		}

		obj.Role = (RoleItem)LuaScriptMgr.GetNetObject(L, 3, typeof(RoleItem));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Player(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PlayerManager obj = (PlayerManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Player");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Player on a nil value");
			}
		}

		obj.Player = (Player)LuaScriptMgr.GetNetObject(L, 3, typeof(Player));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UpdatePlayerinfo(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			PlayerManager obj = (PlayerManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "PlayerManager");
			obj.UpdatePlayerinfo();
			return 0;
		}
		else if (count == 2)
		{
			PlayerManager obj = (PlayerManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "PlayerManager");
			MsgUnPacker arg0 = (MsgUnPacker)LuaScriptMgr.GetNetObject(L, 2, typeof(MsgUnPacker));
			obj.UpdatePlayerinfo(arg0);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: PlayerManager.UpdatePlayerinfo");
		}

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

