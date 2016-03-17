using System;
using LuaInterface;

public class EquipItemWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("New", _CreateEquipItem),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("Equip", get_Equip, null),
			new LuaField("Drug", get_Drug, null),
			new LuaField("Helm", get_Helm, null),
			new LuaField("Cloth", get_Cloth, null),
			new LuaField("Weapon", get_Weapon, null),
			new LuaField("Shoes", get_Shoes, null),
			new LuaField("Necklace", get_Necklace, null),
			new LuaField("Bracelet", get_Bracelet, null),
			new LuaField("Ring", get_Ring, null),
			new LuaField("Wing", get_Wing, null),
			new LuaField("Icon", get_Icon, set_Icon),
			new LuaField("Desc", get_Desc, set_Desc),
			new LuaField("Name", get_Name, set_Name),
			new LuaField("Id", get_Id, set_Id),
			new LuaField("OwnerId", get_OwnerId, set_OwnerId),
			new LuaField("EquipId", get_EquipId, set_EquipId),
			new LuaField("Type", get_Type, set_Type),
			new LuaField("EquipType", get_EquipType, set_EquipType),
			new LuaField("Price", get_Price, set_Price),
			new LuaField("Star", get_Star, set_Star),
			new LuaField("Quality", get_Quality, set_Quality),
			new LuaField("Damage", get_Damage, set_Damage),
			new LuaField("Hp", get_Hp, set_Hp),
			new LuaField("Fc", get_Fc, set_Fc),
			new LuaField("EffectType", get_EffectType, set_EffectType),
			new LuaField("EffectValue", get_EffectValue, set_EffectValue),
			new LuaField("Level", get_Level, set_Level),
			new LuaField("Amount", get_Amount, set_Amount),
			new LuaField("IsDress", get_IsDress, set_IsDress),
			new LuaField("IsMan", get_IsMan, set_IsMan),
		};

		LuaScriptMgr.RegisterLib(L, "EquipItem", typeof(EquipItem), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateEquipItem(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			EquipItem obj = new EquipItem();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: EquipItem.New");
		}

		return 0;
	}

	static Type classType = typeof(EquipItem);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Equip(IntPtr L)
	{
		LuaScriptMgr.Push(L, EquipItem.Equip);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Drug(IntPtr L)
	{
		LuaScriptMgr.Push(L, EquipItem.Drug);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Helm(IntPtr L)
	{
		LuaScriptMgr.Push(L, EquipItem.Helm);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Cloth(IntPtr L)
	{
		LuaScriptMgr.Push(L, EquipItem.Cloth);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Weapon(IntPtr L)
	{
		LuaScriptMgr.Push(L, EquipItem.Weapon);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Shoes(IntPtr L)
	{
		LuaScriptMgr.Push(L, EquipItem.Shoes);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Necklace(IntPtr L)
	{
		LuaScriptMgr.Push(L, EquipItem.Necklace);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Bracelet(IntPtr L)
	{
		LuaScriptMgr.Push(L, EquipItem.Bracelet);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Ring(IntPtr L)
	{
		LuaScriptMgr.Push(L, EquipItem.Ring);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Wing(IntPtr L)
	{
		LuaScriptMgr.Push(L, EquipItem.Wing);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Icon(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

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
		EquipItem obj = (EquipItem)o;

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
		EquipItem obj = (EquipItem)o;

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
	static int get_Id(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

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
		EquipItem obj = (EquipItem)o;

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
	static int get_EquipId(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name EquipId");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index EquipId on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.EquipId);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Type(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

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
	static int get_EquipType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name EquipType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index EquipType on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.EquipType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Price(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Price");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Price on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Price);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Star(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Star");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Star on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Star);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Quality(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Quality");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Quality on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Quality);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Damage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

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
	static int get_Hp(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

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
	static int get_Fc(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

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
	static int get_EffectType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name EffectType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index EffectType on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.EffectType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_EffectValue(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name EffectValue");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index EffectValue on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.EffectValue);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Level(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

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
	static int get_Amount(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Amount");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Amount on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Amount);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsDress(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsDress");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsDress on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsDress);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsMan(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsMan");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsMan on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsMan);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Icon(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

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
		EquipItem obj = (EquipItem)o;

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
		EquipItem obj = (EquipItem)o;

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
	static int set_Id(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

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
		EquipItem obj = (EquipItem)o;

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

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_EquipId(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name EquipId");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index EquipId on a nil value");
			}
		}

		obj.EquipId = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Type(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

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
	static int set_EquipType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name EquipType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index EquipType on a nil value");
			}
		}

		obj.EquipType = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Price(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Price");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Price on a nil value");
			}
		}

		obj.Price = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Star(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Star");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Star on a nil value");
			}
		}

		obj.Star = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Quality(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Quality");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Quality on a nil value");
			}
		}

		obj.Quality = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Damage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

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
	static int set_Hp(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

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
	static int set_Fc(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

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
	static int set_EffectType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name EffectType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index EffectType on a nil value");
			}
		}

		obj.EffectType = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_EffectValue(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name EffectValue");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index EffectValue on a nil value");
			}
		}

		obj.EffectValue = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Level(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

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
	static int set_Amount(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Amount");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Amount on a nil value");
			}
		}

		obj.Amount = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_IsDress(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsDress");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsDress on a nil value");
			}
		}

		obj.IsDress = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_IsMan(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EquipItem obj = (EquipItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsMan");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsMan on a nil value");
			}
		}

		obj.IsMan = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}
}

