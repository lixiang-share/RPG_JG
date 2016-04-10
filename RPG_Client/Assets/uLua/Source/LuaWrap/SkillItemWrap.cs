using System;
using LuaInterface;

public class SkillItemWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("New", _CreateSkillItem),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("Man", get_Man, null),
			new LuaField("Woman", get_Woman, null),
			new LuaField("One", get_One, null),
			new LuaField("Two", get_Two, null),
			new LuaField("Three", get_Three, null),
			new LuaField("Basic", get_Basic, null),
			new LuaField("Base", get_Base, null),
			new LuaField("Id", get_Id, set_Id),
			new LuaField("SkillID", get_SkillID, set_SkillID),
			new LuaField("OwnerID", get_OwnerID, set_OwnerID),
			new LuaField("RoleType", get_RoleType, set_RoleType),
			new LuaField("Type", get_Type, set_Type),
			new LuaField("Pos", get_Pos, set_Pos),
			new LuaField("ColdTime", get_ColdTime, set_ColdTime),
			new LuaField("Fc", get_Fc, set_Fc),
			new LuaField("Level", get_Level, set_Level),
			new LuaField("Icon", get_Icon, set_Icon),
			new LuaField("Desc", get_Desc, set_Desc),
			new LuaField("Name", get_Name, set_Name),
		};

		LuaScriptMgr.RegisterLib(L, "SkillItem", typeof(SkillItem), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSkillItem(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			SkillItem obj = new SkillItem();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SkillItem.New");
		}

		return 0;
	}

	static Type classType = typeof(SkillItem);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Man(IntPtr L)
	{
		LuaScriptMgr.Push(L, SkillItem.Man);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Woman(IntPtr L)
	{
		LuaScriptMgr.Push(L, SkillItem.Woman);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_One(IntPtr L)
	{
		LuaScriptMgr.Push(L, SkillItem.One);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Two(IntPtr L)
	{
		LuaScriptMgr.Push(L, SkillItem.Two);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Three(IntPtr L)
	{
		LuaScriptMgr.Push(L, SkillItem.Three);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Basic(IntPtr L)
	{
		LuaScriptMgr.Push(L, SkillItem.Basic);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Base(IntPtr L)
	{
		LuaScriptMgr.Push(L, SkillItem.Base);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Id(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

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
	static int get_SkillID(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name SkillID");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index SkillID on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.SkillID);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_OwnerID(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name OwnerID");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index OwnerID on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.OwnerID);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_RoleType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name RoleType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index RoleType on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.RoleType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Type(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Type");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Type on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Type);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Pos(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Pos");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Pos on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Pos);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ColdTime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ColdTime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ColdTime on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.ColdTime);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Fc(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

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
	static int get_Level(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

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
	static int get_Icon(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Icon");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Icon on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Icon);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Desc(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Desc");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Desc on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Desc);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Name(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

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
	static int set_Id(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

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
	static int set_SkillID(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name SkillID");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index SkillID on a nil value");
			}
		}

		obj.SkillID = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_OwnerID(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name OwnerID");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index OwnerID on a nil value");
			}
		}

		obj.OwnerID = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_RoleType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name RoleType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index RoleType on a nil value");
			}
		}

		obj.RoleType = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Type(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Type");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Type on a nil value");
			}
		}

		obj.Type = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Pos(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Pos");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Pos on a nil value");
			}
		}

		obj.Pos = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_ColdTime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ColdTime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ColdTime on a nil value");
			}
		}

		obj.ColdTime = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Fc(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

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
	static int set_Level(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

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
	static int set_Icon(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Icon");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Icon on a nil value");
			}
		}

		obj.Icon = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Desc(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Desc");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Desc on a nil value");
			}
		}

		obj.Desc = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Name(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkillItem obj = (SkillItem)o;

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
}

