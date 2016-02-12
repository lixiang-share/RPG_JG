using System;
using System.Collections;
using LuaInterface;

public class ArrayListWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("get_Item", get_Item),
			new LuaMethod("set_Item", set_Item),
			new LuaMethod("Add", Add),
			new LuaMethod("Clear", Clear),
			new LuaMethod("Contains", Contains),
			new LuaMethod("IndexOf", IndexOf),
			new LuaMethod("LastIndexOf", LastIndexOf),
			new LuaMethod("Insert", Insert),
			new LuaMethod("InsertRange", InsertRange),
			new LuaMethod("Remove", Remove),
			new LuaMethod("RemoveAt", RemoveAt),
			new LuaMethod("RemoveRange", RemoveRange),
			new LuaMethod("Reverse", Reverse),
			new LuaMethod("CopyTo", CopyTo),
			new LuaMethod("GetEnumerator", GetEnumerator),
			new LuaMethod("AddRange", AddRange),
			new LuaMethod("BinarySearch", BinarySearch),
			new LuaMethod("GetRange", GetRange),
			new LuaMethod("SetRange", SetRange),
			new LuaMethod("TrimToSize", TrimToSize),
			new LuaMethod("Sort", Sort),
			new LuaMethod("ToArray", ToArray),
			new LuaMethod("Clone", Clone),
			new LuaMethod("Adapter", Adapter),
			new LuaMethod("Synchronized", Synchronized),
			new LuaMethod("ReadOnly", ReadOnly),
			new LuaMethod("FixedSize", FixedSize),
			new LuaMethod("Repeat", Repeat),
			new LuaMethod("New", _CreateArrayList),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("Count", get_Count, null),
			new LuaField("Capacity", get_Capacity, set_Capacity),
			new LuaField("IsFixedSize", get_IsFixedSize, null),
			new LuaField("IsReadOnly", get_IsReadOnly, null),
			new LuaField("IsSynchronized", get_IsSynchronized, null),
			new LuaField("SyncRoot", get_SyncRoot, null),
		};

		LuaScriptMgr.RegisterLib(L, "System.Collections.ArrayList", typeof(ArrayList), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateArrayList(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			ArrayList obj = new ArrayList();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(int)))
		{
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
			ArrayList obj = new ArrayList(arg0);
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(ICollection)))
		{
			ICollection arg0 = (ICollection)LuaScriptMgr.GetNetObject(L, 1, typeof(ICollection));
			ArrayList obj = new ArrayList(arg0);
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: ArrayList.New");
		}

		return 0;
	}

	static Type classType = typeof(ArrayList);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Count(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ArrayList obj = (ArrayList)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Count");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Count on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Count);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Capacity(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ArrayList obj = (ArrayList)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Capacity");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Capacity on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Capacity);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsFixedSize(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ArrayList obj = (ArrayList)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsFixedSize");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsFixedSize on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsFixedSize);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsReadOnly(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ArrayList obj = (ArrayList)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsReadOnly");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsReadOnly on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsReadOnly);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsSynchronized(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ArrayList obj = (ArrayList)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsSynchronized");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsSynchronized on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsSynchronized);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_SyncRoot(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ArrayList obj = (ArrayList)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name SyncRoot");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index SyncRoot on a nil value");
			}
		}

		LuaScriptMgr.PushVarObject(L, obj.SyncRoot);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Capacity(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ArrayList obj = (ArrayList)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Capacity");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Capacity on a nil value");
			}
		}

		obj.Capacity = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Item(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		object o = obj[arg0];
		LuaScriptMgr.PushVarObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Item(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		object arg1 = LuaScriptMgr.GetVarObject(L, 3);
		obj[arg0] = arg1;
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Add(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
		object arg0 = LuaScriptMgr.GetVarObject(L, 2);
		int o = obj.Add(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Clear(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
		obj.Clear();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Contains(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
		object arg0 = LuaScriptMgr.GetVarObject(L, 2);
		bool o = obj.Contains(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IndexOf(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
			object arg0 = LuaScriptMgr.GetVarObject(L, 2);
			int o = obj.IndexOf(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 3)
		{
			ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
			object arg0 = LuaScriptMgr.GetVarObject(L, 2);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			int o = obj.IndexOf(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 4)
		{
			ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
			object arg0 = LuaScriptMgr.GetVarObject(L, 2);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			int arg2 = (int)LuaScriptMgr.GetNumber(L, 4);
			int o = obj.IndexOf(arg0,arg1,arg2);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: ArrayList.IndexOf");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LastIndexOf(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
			object arg0 = LuaScriptMgr.GetVarObject(L, 2);
			int o = obj.LastIndexOf(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 3)
		{
			ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
			object arg0 = LuaScriptMgr.GetVarObject(L, 2);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			int o = obj.LastIndexOf(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 4)
		{
			ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
			object arg0 = LuaScriptMgr.GetVarObject(L, 2);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			int arg2 = (int)LuaScriptMgr.GetNumber(L, 4);
			int o = obj.LastIndexOf(arg0,arg1,arg2);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: ArrayList.LastIndexOf");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Insert(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		object arg1 = LuaScriptMgr.GetVarObject(L, 3);
		obj.Insert(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InsertRange(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		ICollection arg1 = (ICollection)LuaScriptMgr.GetNetObject(L, 3, typeof(ICollection));
		obj.InsertRange(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Remove(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
		object arg0 = LuaScriptMgr.GetVarObject(L, 2);
		obj.Remove(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveAt(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		obj.RemoveAt(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveRange(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
		obj.RemoveRange(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Reverse(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
			obj.Reverse();
			return 0;
		}
		else if (count == 3)
		{
			ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			obj.Reverse(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: ArrayList.Reverse");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CopyTo(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
			Array arg0 = (Array)LuaScriptMgr.GetNetObject(L, 2, typeof(Array));
			obj.CopyTo(arg0);
			return 0;
		}
		else if (count == 3)
		{
			ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
			Array arg0 = (Array)LuaScriptMgr.GetNetObject(L, 2, typeof(Array));
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			obj.CopyTo(arg0,arg1);
			return 0;
		}
		else if (count == 5)
		{
			ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
			Array arg1 = (Array)LuaScriptMgr.GetNetObject(L, 3, typeof(Array));
			int arg2 = (int)LuaScriptMgr.GetNumber(L, 4);
			int arg3 = (int)LuaScriptMgr.GetNumber(L, 5);
			obj.CopyTo(arg0,arg1,arg2,arg3);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: ArrayList.CopyTo");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetEnumerator(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
			IEnumerator o = obj.GetEnumerator();
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 3)
		{
			ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			IEnumerator o = obj.GetEnumerator(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: ArrayList.GetEnumerator");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddRange(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
		ICollection arg0 = (ICollection)LuaScriptMgr.GetNetObject(L, 2, typeof(ICollection));
		obj.AddRange(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int BinarySearch(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
			object arg0 = LuaScriptMgr.GetVarObject(L, 2);
			int o = obj.BinarySearch(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 3)
		{
			ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
			object arg0 = LuaScriptMgr.GetVarObject(L, 2);
			IComparer arg1 = (IComparer)LuaScriptMgr.GetNetObject(L, 3, typeof(IComparer));
			int o = obj.BinarySearch(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 5)
		{
			ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			object arg2 = LuaScriptMgr.GetVarObject(L, 4);
			IComparer arg3 = (IComparer)LuaScriptMgr.GetNetObject(L, 5, typeof(IComparer));
			int o = obj.BinarySearch(arg0,arg1,arg2,arg3);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: ArrayList.BinarySearch");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetRange(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
		ArrayList o = obj.GetRange(arg0,arg1);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetRange(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		ICollection arg1 = (ICollection)LuaScriptMgr.GetNetObject(L, 3, typeof(ICollection));
		obj.SetRange(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TrimToSize(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
		obj.TrimToSize();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Sort(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
			obj.Sort();
			return 0;
		}
		else if (count == 2)
		{
			ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
			IComparer arg0 = (IComparer)LuaScriptMgr.GetNetObject(L, 2, typeof(IComparer));
			obj.Sort(arg0);
			return 0;
		}
		else if (count == 4)
		{
			ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			IComparer arg2 = (IComparer)LuaScriptMgr.GetNetObject(L, 4, typeof(IComparer));
			obj.Sort(arg0,arg1,arg2);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: ArrayList.Sort");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ToArray(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
			object[] o = obj.ToArray();
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else if (count == 2)
		{
			ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
			Type arg0 = LuaScriptMgr.GetTypeObject(L, 2);
			Array o = obj.ToArray(arg0);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: ArrayList.ToArray");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Clone(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		ArrayList obj = (ArrayList)LuaScriptMgr.GetNetObjectSelf(L, 1, "ArrayList");
		object o = obj.Clone();
		LuaScriptMgr.PushVarObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Adapter(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		IList arg0 = (IList)LuaScriptMgr.GetNetObject(L, 1, typeof(IList));
		ArrayList o = ArrayList.Adapter(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Synchronized(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(IList)))
		{
			IList arg0 = (IList)LuaScriptMgr.GetLuaObject(L, 1);
			IList o = ArrayList.Synchronized(arg0);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(ArrayList)))
		{
			ArrayList arg0 = (ArrayList)LuaScriptMgr.GetLuaObject(L, 1);
			ArrayList o = ArrayList.Synchronized(arg0);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: ArrayList.Synchronized");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReadOnly(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(IList)))
		{
			IList arg0 = (IList)LuaScriptMgr.GetLuaObject(L, 1);
			IList o = ArrayList.ReadOnly(arg0);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(ArrayList)))
		{
			ArrayList arg0 = (ArrayList)LuaScriptMgr.GetLuaObject(L, 1);
			ArrayList o = ArrayList.ReadOnly(arg0);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: ArrayList.ReadOnly");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FixedSize(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(IList)))
		{
			IList arg0 = (IList)LuaScriptMgr.GetLuaObject(L, 1);
			IList o = ArrayList.FixedSize(arg0);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(ArrayList)))
		{
			ArrayList arg0 = (ArrayList)LuaScriptMgr.GetLuaObject(L, 1);
			ArrayList o = ArrayList.FixedSize(arg0);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: ArrayList.FixedSize");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Repeat(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		object arg0 = LuaScriptMgr.GetVarObject(L, 1);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		ArrayList o = ArrayList.Repeat(arg0,arg1);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}
}

