using UnityEngine;
using System.Collections;
using System.IO;
using MsgPack;
using System.Collections.Generic;
/// <summary>
/// 消息实体类
/// </summary>
public class MsgPacker {

    private IReceiveData _receiver;
    private bool _isNeedRecv = true;
    private bool _isSetType = false;
    private int _msgType;

    public int MsgType
    {
        get { return _msgType; }
        set { _msgType = value; }
    }
    private MemoryStream stream;
    private Packer packer;

    public MsgPacker()
    {
        stream = new MemoryStream();
        packer = Packer.Create(stream);
    }
    public bool IsSetType
    {
        get { return _isSetType; }
        set { _isSetType = value; }
    }

    internal IReceiveData Receiver
    {
        get
        {
            return _receiver;
        }

        set
        {
            _receiver = value;
        }
    }

    public bool IsNeedRecv
    {
        get
        {
            return _isNeedRecv;
        }
        set
        {
            _isNeedRecv = value;
        }
    }

    public byte[] Serialize(){
        byte[] buff = stream.ToArray();
        return buff;
	}

    public void close(){
		packer.Dispose();
        stream.Close();
	}
    public MsgPacker SetType(int msgType)
    {
        IsSetType = true;
        MsgType = msgType;
        add<string>(AppConst.SessionKey);
        return add(msgType);
    }

    public MsgPacker add<T>(T t)
    {
        if (!IsSetType) return null;
        packer.Pack<T>(t);
        return this;
    }

    public  MsgPacker addArray<T>(T[] arr){
        if (!IsSetType) return null;
		packer.PackArray<T>(arr);
        return this;
	}

    public MsgPacker addMap<K, V>(K k, V v)
    {
        if (!IsSetType) return null;
        IDictionary<K,V> dict = new Dictionary<K,V>();
        dict.Add(k,v);
        return addMap<K, V>(dict);
    }
    public MsgPacker addMap<K, V>(IDictionary<K, V> dict)
    {
        if (!IsSetType) return null;
        packer.PackMap<K, V>(dict);
        return this;
    }

}
