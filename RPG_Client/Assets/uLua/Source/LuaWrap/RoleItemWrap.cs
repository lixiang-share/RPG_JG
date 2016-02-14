using System;
using LuaInterface;

public class RoleItemWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("New", _CreateRoleItem),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("Role_id", get_Role_id, set_Role_id),
			new LuaField("Name", get_Name, set_Name),
			new LuaField("Level", get_Level, set_Level),
			new LuaField("Gender", get_Gender, set_Gender),
			new LuaField("RealGender", get_RealGender, set_RealGender),
			new LuaField("Id", get_Id, set_Id),
			new LuaField("OwnerId", get_OwnerId, set_OwnerId),
		};

		LuaScriptMgr.RegisterLib(L, "RoleItem", typeof(RoleItem), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateRoleItem(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			RoleItem obj = new RoleItem();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: RoleItem.New");
		}

		return 0;
	}

	static Type classType = typeof(RoleItem);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Role_id(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RoleItem obj = (RoleItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Role_id");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Role_id on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Role_id);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Name(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RoleItem obj = (RoleItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Name");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Name on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Name);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Level(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RoleItem obj = (RoleItem)o;

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
	static int get_Gender(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RoleItem obj = (RoleItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Gender");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Gender on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Gender);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_RealGender(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RoleItem obj = (RoleItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name RealGender");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index RealGender on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.RealGender);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Id(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RoleItem obj = (RoleItem)o;

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
	static int get_OwnerId(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RoleItem obj = (RoleItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name OwnerId");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index OwnerId on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.OwnerId);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Role_id(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RoleItem obj = (RoleItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Role_id");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Role_id on a nil value");
			}
		}

		obj.Role_id = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Name(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RoleItem obj = (RoleItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Name");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Name on a nil value");
			}
		}

		obj.Name = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Level(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RoleItem obj = (RoleItem)o;

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
	static int set_Gender(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RoleItem obj = (RoleItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Gender");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Gender on a nil value");
			}
		}

		obj.Gender = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_RealGender(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RoleItem obj = (RoleItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name RealGender");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index RealGender on a nil value");
			}
		}

		obj.RealGender = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Id(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RoleItem obj = (RoleItem)o;

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
	static int set_OwnerId(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RoleItem obj = (RoleItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name OwnerId");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index OwnerId on a nil value");
			}
		}

		obj.OwnerId = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}
}

