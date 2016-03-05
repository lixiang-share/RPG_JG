using System;
using LuaInterface;

public class PlayerWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("New", _CreatePlayer),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("CurNeedExp", get_CurNeedExp, null),
			new LuaField("TotalToughen", get_TotalToughen, set_TotalToughen),
			new LuaField("TotalVit", get_TotalVit, set_TotalVit),
			new LuaField("Vip", get_Vip, set_Vip),
			new LuaField("Id", get_Id, set_Id),
			new LuaField("Username", get_Username, set_Username),
			new LuaField("PhoneNum", get_PhoneNum, set_PhoneNum),
			new LuaField("Level", get_Level, set_Level),
			new LuaField("Fc", get_Fc, set_Fc),
			new LuaField("Exp", get_Exp, set_Exp),
			new LuaField("DiamondCount", get_DiamondCount, set_DiamondCount),
			new LuaField("GoldCount", get_GoldCount, set_GoldCount),
			new LuaField("Vit", get_Vit, set_Vit),
			new LuaField("Toughen", get_Toughen, set_Toughen),
			new LuaField("Hp", get_Hp, set_Hp),
			new LuaField("Damage", get_Damage, set_Damage),
			new LuaField("Role", get_Role, set_Role),
		};

		LuaScriptMgr.RegisterLib(L, "Player", typeof(Player), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreatePlayer(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			Player obj = new Player();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Player.New");
		}

		return 0;
	}

	static Type classType = typeof(Player);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_CurNeedExp(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name CurNeedExp");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index CurNeedExp on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.CurNeedExp);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TotalToughen(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name TotalToughen");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index TotalToughen on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.TotalToughen);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TotalVit(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name TotalVit");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index TotalVit on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.TotalVit);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Vip(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Vip");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Vip on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Vip);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Id(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Id");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Id on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Id);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Username(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Username");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Username on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Username);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_PhoneNum(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name PhoneNum");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index PhoneNum on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.PhoneNum);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Level(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Level");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Level on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Level);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Fc(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Fc");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Fc on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Fc);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Exp(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Exp");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Exp on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Exp);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DiamondCount(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name DiamondCount");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index DiamondCount on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.DiamondCount);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_GoldCount(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name GoldCount");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index GoldCount on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.GoldCount);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Vit(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Vit");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Vit on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Vit);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Toughen(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Toughen");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Toughen on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Toughen);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Hp(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Hp");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Hp on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Hp);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Damage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Damage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Damage on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Damage);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Role(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

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
	static int set_TotalToughen(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name TotalToughen");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index TotalToughen on a nil value");
			}
		}

		obj.TotalToughen = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_TotalVit(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name TotalVit");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index TotalVit on a nil value");
			}
		}

		obj.TotalVit = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Vip(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Vip");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Vip on a nil value");
			}
		}

		obj.Vip = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Id(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Id");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Id on a nil value");
			}
		}

		obj.Id = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Username(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Username");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Username on a nil value");
			}
		}

		obj.Username = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_PhoneNum(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name PhoneNum");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index PhoneNum on a nil value");
			}
		}

		obj.PhoneNum = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Level(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Level");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Level on a nil value");
			}
		}

		obj.Level = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Fc(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Fc");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Fc on a nil value");
			}
		}

		obj.Fc = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Exp(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Exp");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Exp on a nil value");
			}
		}

		obj.Exp = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_DiamondCount(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name DiamondCount");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index DiamondCount on a nil value");
			}
		}

		obj.DiamondCount = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_GoldCount(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name GoldCount");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index GoldCount on a nil value");
			}
		}

		obj.GoldCount = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Vit(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Vit");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Vit on a nil value");
			}
		}

		obj.Vit = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Toughen(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Toughen");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Toughen on a nil value");
			}
		}

		obj.Toughen = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Hp(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Hp");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Hp on a nil value");
			}
		}

		obj.Hp = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Damage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Damage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Damage on a nil value");
			}
		}

		obj.Damage = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Role(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Player obj = (Player)o;

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
}

