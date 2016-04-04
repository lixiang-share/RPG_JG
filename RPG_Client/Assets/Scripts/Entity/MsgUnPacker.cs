using UnityEngine;
using System.Collections;
using System.IO;
using MsgPack;
using System.Collections.Generic;

public class MsgUnPacker{

    private byte[] buff;
    private MemoryStream stream ;
    private Unpacker unpacker;
    private IReceiveData _receiver;
    private int msgType;
    private MsgHandler recvHandler;

    public MsgHandler RecvHandler
    {
        get { return recvHandler; }
        set { recvHandler = value; }
    }

    public int MsgType
    {
        get { return msgType; }
        set { msgType = value; }
    }

    public MsgUnPacker(byte[] buff){
        this.buff = buff;
        stream = new MemoryStream(buff);
        unpacker = Unpacker.Create(stream);
    }
    public IReceiveData Receiver
    {
        get { return _receiver; }
        set { _receiver = value; }
    }

    public static MsgUnPacker Create(byte[] buff)
    {
        if (buff == null) return null;
        return new MsgUnPacker(buff);
    }

    public void Close()
    {
        unpacker.Dispose();
        if (stream.CanRead) stream.Close();
    }

    public void skip(int steps) {
        for (int i = 0; i < steps; i++)
        {
            unpacker.Skip();
        }
    }

    public MsgUnPacker Reset()
    {
        this.Close();
        MsgUnPacker unpacker = new MsgUnPacker(this.buff);
        unpacker.Receiver = this.Receiver;
        unpacker.MsgType = this.MsgType;
        return unpacker;
    }

    #region 泛型方法，似乎与java端不能互通，暂时无法使用
    public T Pop<T>()
    {
        T t = unpacker.Unpack<T>();
        return t;
    }


    public T[] PopArray<T>(){
        long len = 0;
        if(!unpacker.ReadArrayLength(out len)){
            return null;
        }
        T[] ts = new T[len];
        for (int i = 0; i < len; i++)
		{
		   ts[i] = unpacker.Unpack<T>();	 
		}
        return ts;
    }

    public IDictionary<K,V> PopMap<K,V>(){
        long len = 0;
        if(!unpacker.ReadMapLength(out len)){
            return null;
        }
        IDictionary<K,V> dict = new Dictionary<K,V>();
        for (int i = 0; i < len; i++)
		{
		    dict.Add(Pop<K>() , Pop<V>());	 
		}
        return dict;
    }

    public T Get<T>(int index){
       MemoryStream s = new MemoryStream(buff);
        Unpacker _unpacker = Unpacker.Create(s);
        for (int i = 0; i < index; i++)
		{
			 _unpacker.Skip();
		}
        T t =_unpacker.Unpack<T>();
        _unpacker.Dispose();
        s.Close();
       return t;
    }

    public T[] GetArray<T>(int index){
        MemoryStream s = new MemoryStream(buff);
        Unpacker _unpacker = Unpacker.Create(s);
        for (int i = 0; i < index; i++)
		{
			 _unpacker.Skip();
		}

        long len = 0;
        if(!_unpacker.ReadArrayLength(out len)){
            return null;
        }
        T[] ts = new T[len];
        for (int i = 0; i < len; i++)
		{
		   ts[i] = _unpacker.Unpack<T>();	 
		}
        _unpacker.Dispose();
        s.Close();
        return ts;
    }

    public IDictionary<K,V> GetMapt<K,V>(int index){
        MemoryStream s = new MemoryStream(buff);
        Unpacker _unpacker = Unpacker.Create(s);
        for (int i = 0; i < index; i++)
		{
			 _unpacker.Skip();
		}

        long len = 0;
        if(!_unpacker.ReadMapLength(out len)){
            return null;
        }
        IDictionary<K,V> dict = new Dictionary<K,V>();
        for (int i = 0; i < len; i++)
		{
		    dict.Add(_unpacker.Unpack<K>() , _unpacker.Unpack<V>());	 
		}
        _unpacker.Dispose();
        s.Close();
        return dict;
    }

    #endregion




    #region 顺序取值（效率高）
    public MessagePackObject PopObj()
    {
        MessagePackObject mpo;
        if (!unpacker.ReadObject(out mpo)) return 0;
        return mpo;
    }

    public int PopInt()
    {
        int rst = 0;
        unpacker.ReadInt32(out rst);
        return rst;
    }
    public float PopFloat()
    {
        return (float)PopDouble();
    }

    public double PopDouble()
    {
        double rst = 0;
        unpacker.ReadDouble(out rst);
        return rst;
    }
    public string PopString()
    {
        string str = "";
        unpacker.ReadString(out str);
        return str;
    }

    public bool PopBool()
    {
        bool b = false;
        unpacker.ReadBoolean(out b);
        return b;
    }

    public List<int> PopIntList()
    {
        long len;
        if (!unpacker.ReadArrayLength(out len)) return null;
        List<int> list = new List<int>();
        for (int i = 0; i < len; i++)
        {
            list.Add(PopInt());   
        }
        return list;
    }


    public List<double> PopDoubleList()
    {
        long len;
        if (!unpacker.ReadArrayLength(out len)) return null;
        List<double> list = new List<double>();
        for (int i = 0; i < len; i++)
        {
            list.Add(PopDouble());
        }
        return list;
    }

    public List<float> PopFloatList()
    {
        long len;
        if (!unpacker.ReadArrayLength(out len)) return null;
        List<float> list = new List<float>();
        for (int i = 0; i < len; i++)
        {
            list.Add(PopFloat());
        }
        return list;
    }


    public List<string> PopStringList()
    {
        long len;
        if (!unpacker.ReadArrayLength(out len)) return null;
        List<string> list = new List<string>();
        for (int i = 0; i < len; i++)
        {
            list.Add(PopString());
        }
        return list;
    }

    public Dictionary<string, int> PopKStringVIntMap()
    {
        long len = 0;
        if (!unpacker.ReadMapLength(out len)) return null;
        Dictionary<string, int> dict = new Dictionary<string, int>();
        for (int i = 0; i < len; i++)
        {
            dict.Add(PopString(), PopInt());
        }
        return dict;
    }

    public Dictionary<string, float> PopKStringVFloatMap()
    {
        long len = 0;
        if (!unpacker.ReadMapLength(out len)) return null;
        Dictionary<string, float> dict = new Dictionary<string, float>();
        for (int i = 0; i < len; i++)
        {
            dict.Add(PopString(), PopFloat());
        }
        return dict;
    }

    public Dictionary<string, double> PopKStringVDoubleMap()
    {
        long len = 0;
        if (!unpacker.ReadMapLength(out len)) return null;
        Dictionary<string, double> dict = new Dictionary<string, double>();
        for (int i = 0; i < len; i++)
        {
            dict.Add(PopString(), PopDouble());
        }
        return dict;
    }
    public Dictionary<string, string> PopKStringVStringMap()
    {
        long len = 0;
        if (!unpacker.ReadMapLength(out len)) return null;
        Dictionary<string, string> dict = new Dictionary<string, string>();
        for (int i = 0; i < len; i++)
        {
            dict.Add(PopString(),PopString());
        }
        return dict;
    }
    #endregion


    #region 根据索引取值（效率较低）
    public int GetInt(int index)
    {
        MsgUnPacker _unpacker = new MsgUnPacker(this.buff);
        _unpacker.skip(index);
        int rst =  _unpacker.PopInt();
        _unpacker.Close();
        return rst;
    }

    public float GetFloat(int index)
    {
        MsgUnPacker _unpacker = new MsgUnPacker(this.buff);
        _unpacker.skip(index);
        float rst = _unpacker.PopFloat();
        _unpacker.Close();
        return rst;
    }


    public double GetDouble(int index)
    {
        MsgUnPacker _unpacker = new MsgUnPacker(this.buff);
        _unpacker.skip(index);
        double rst = _unpacker.PopDouble();
        _unpacker.Close();
        return rst;
    }

    public string GetString(int index)
    {
        MsgUnPacker _unpacker = new MsgUnPacker(this.buff);
        _unpacker.skip(index);
        string rst = _unpacker.PopString();
        _unpacker.Close();
        return rst;
    }



    public List<int> GetIntList(int index)
    {
        MsgUnPacker _unpacker = new MsgUnPacker(this.buff);
        _unpacker.skip(index);
        List<int> rst = _unpacker.PopIntList();
        _unpacker.Close();
        return rst;
    }

    public List<float> GetFloatList(int index)
    {
        MsgUnPacker _unpacker = new MsgUnPacker(this.buff);
        _unpacker.skip(index);
        List<float> rst = _unpacker.PopFloatList();
        _unpacker.Close();
        return rst;
    }


    public List<double> GetDoubleList(int index)
    {
        MsgUnPacker _unpacker = new MsgUnPacker(this.buff);
        _unpacker.skip(index);
        List<double> rst = _unpacker.PopDoubleList();
        _unpacker.Close();
        return rst;
    }

    public List<string> GetStringList(int index)
    {
        MsgUnPacker _unpacker = new MsgUnPacker(this.buff);
        _unpacker.skip(index);
        List<string> rst = _unpacker.PopStringList();
        _unpacker.Close();
        return rst;
    }


    public Dictionary<string , int> GetKStringVInt(int index)
    {
        MsgUnPacker _unpacker = new MsgUnPacker(this.buff);
        _unpacker.skip(index);
        Dictionary<string, int> rst = _unpacker.PopKStringVIntMap();
        _unpacker.Close();
        return rst;
    }


    public Dictionary<string, float> GetKStringVFloat(int index)
    {
        MsgUnPacker _unpacker = new MsgUnPacker(this.buff);
        _unpacker.skip(index);
        Dictionary<string, float> rst = _unpacker.PopKStringVFloatMap();
        _unpacker.Close();
        return rst;
    }

    public Dictionary<string, double> GetKStringVDouble(int index)
    {
        MsgUnPacker _unpacker = new MsgUnPacker(this.buff);
        _unpacker.skip(index);
        Dictionary<string, double> rst = _unpacker.PopKStringVDoubleMap();
        _unpacker.Close();
        return rst;
    }


    public Dictionary<string, string> GetKStringVString(int index)
    {
        MsgUnPacker _unpacker = new MsgUnPacker(this.buff);
        _unpacker.skip(index);
        Dictionary<string, string> rst = _unpacker.PopKStringVStringMap();
        _unpacker.Close();
        return rst;
    }
    #endregion
}
