using UnityEngine;
using System.Collections;
using System.Net.Sockets;

public class StateObj{
    #region field
    private Socket _client;
    private MsgEntity _msg;
    private NetEventCallBack _OnSend;
    private NetEventCallBack _OnReceive;
    private NetEventCallBack _OnConnect;
    private byte[] _buff;
    public Socket Client
    {
        get
        {
            return _client;
        }

        set
        {
            _client = value;
        }
    }

    public MsgEntity Msg
    {
        get
        {
            return _msg;
        }

        set
        {
            _msg = value;
        }
    }

    public NetEventCallBack OnSend
    {
        get
        {
            return _OnSend;
        }

        set
        {
            _OnSend = value;
        }
    }

    public NetEventCallBack OnReceive
    {
        get
        {
            return _OnReceive;
        }

        set
        {
            _OnReceive = value;
        }
    }

    public NetEventCallBack OnConnect
    {
        get
        {
            return _OnConnect;
        }

        set
        {
            _OnConnect = value;
        }
    }

    public byte[] Buff
    {
        get
        {
            if (_buff == null) _buff = new byte[1024];
            return _buff;
        }

        set
        {
            _buff = value;
        }
    }


    #endregion



}
