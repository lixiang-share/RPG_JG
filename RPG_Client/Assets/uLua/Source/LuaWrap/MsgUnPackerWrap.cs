using System;
using System.Collections.Generic;
using LuaInterface;

public class MsgUnPackerWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Create", Create),
			new LuaMethod("Close", Close),
			new LuaMethod("skip", skip),
			new LuaMethod("Reset", Reset),
			new LuaMethod("PopObj", PopObj),
			new LuaMethod("PopInt", PopInt),
			new LuaMethod("PopFloat", PopFloat),
			new LuaMethod("PopDouble", PopDouble),
			new LuaMethod("PopString", PopString),
			new LuaMethod("PopIntList", PopIntList),
			new LuaMethod("PopDoubleList", PopDoubleList),
			new LuaMethod("PopFloatList", PopFloatList),
			new LuaMethod("PopStringList", PopStringList),
			new LuaMethod("PopKStringVIntMap", PopKStringVIntMap),
			new LuaMethod("PopKStringVFloatMap", PopKStringVFloatMap),
			new LuaMethod("PopKStringVDoubleMap", PopKStringVDoubleMap),
			new LuaMethod("PopKStringVStringMap", PopKStringVStringMap),
			new LuaMethod("GetInt", GetInt),
			new LuaMethod("GetFloat", GetFloat),
			new LuaMethod("GetDouble", GetDouble),
			new LuaMethod("GetString", GetString),
			new LuaMethod("GetIntList", GetIntList),
			new LuaMethod("GetFloatList", GetFloatList),
			new LuaMethod("GetDoubleList", GetDoubleList),
			new LuaMethod("GetStringList", GetStringList),
			new LuaMethod("GetKStringVInt", GetKStringVInt),
			new LuaMethod("GetKStringVFloat", GetKStringVFloat),
			new LuaMethod("GetKStringVDouble", GetKStringVDouble),
			new LuaMethod("GetKStringVString", GetKStringVString),
			new LuaMethod("New", _CreateMsgUnPacker),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("MsgType", get_MsgType, set_MsgType),
			new LuaField("Receiver", get_Receiver, set_Receiver),
		};

		LuaScriptMgr.RegisterLib(L, "MsgUnPacker", typeof(MsgUnPacker), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateMsgUnPacker(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			byte[] objs0 = LuaScriptMgr.GetArrayNumber<byte>(L, 1);
			MsgUnPacker obj = new MsgUnPacker(objs0);
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: MsgUnPacker.New");
		}

		return 0;
	}

	static Type classType = typeof(MsgUnPacker);

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
		MsgUnPacker obj = (MsgUnPacker)o;

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
	static int get_Receiver(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		MsgUnPacker obj = (MsgUnPacker)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Receiver");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Receiver on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.Receiver);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_MsgType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		MsgUnPacker obj = (MsgUnPacker)o;

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
	static int set_Receiver(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		MsgUnPacker obj = (MsgUnPacker)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Receiver");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Receiver on a nil value");
			}
		}

		obj.Receiver = (IReceiveData)LuaScriptMgr.GetNetObject(L, 3, typeof(IReceiveData));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Create(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		byte[] objs0 = LuaScriptMgr.GetArrayNumber<byte>(L, 1);
		MsgUnPacker o = MsgUnPacker.Create(objs0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Close(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		obj.Close();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int skip(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		obj.skip(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Reset(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		MsgUnPacker o = obj.Reset();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PopObj(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		MsgPack.MessagePackObject o = obj.PopObj();
		LuaScriptMgr.PushValue(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PopInt(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		int o = obj.PopInt();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PopFloat(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		float o = obj.PopFloat();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PopDouble(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		double o = obj.PopDouble();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PopString(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		string o = obj.PopString();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PopIntList(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		List<int> o = obj.PopIntList();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PopDoubleList(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		List<double> o = obj.PopDoubleList();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PopFloatList(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		List<float> o = obj.PopFloatList();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PopStringList(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		List<string> o = obj.PopStringList();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PopKStringVIntMap(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		Dictionary<string,int> o = obj.PopKStringVIntMap();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PopKStringVFloatMap(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		Dictionary<string,float> o = obj.PopKStringVFloatMap();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PopKStringVDoubleMap(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		Dictionary<string,double> o = obj.PopKStringVDoubleMap();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PopKStringVStringMap(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		Dictionary<string,string> o = obj.PopKStringVStringMap();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetInt(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		int o = obj.GetInt(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetFloat(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		float o = obj.GetFloat(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetDouble(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		double o = obj.GetDouble(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetString(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		string o = obj.GetString(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetIntList(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		List<int> o = obj.GetIntList(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetFloatList(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		List<float> o = obj.GetFloatList(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetDoubleList(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		List<double> o = obj.GetDoubleList(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetStringList(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		List<string> o = obj.GetStringList(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetKStringVInt(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		Dictionary<string,int> o = obj.GetKStringVInt(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetKStringVFloat(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		Dictionary<string,float> o = obj.GetKStringVFloat(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetKStringVDouble(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		Dictionary<string,double> o = obj.GetKStringVDouble(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetKStringVString(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MsgUnPacker obj = (MsgUnPacker)LuaScriptMgr.GetNetObjectSelf(L, 1, "MsgUnPacker");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		Dictionary<string,string> o = obj.GetKStringVString(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}
}

