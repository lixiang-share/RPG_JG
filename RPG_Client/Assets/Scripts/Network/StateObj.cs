using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Collections.Generic;
using System;

public class StateObj{
    #region 字段
    private Socket _client;
    private MsgEntity _msg;
    private byte[] _buff;
    private List<byte> _recvBuff;
    private int _recvLen;
    private int _errorCode;


    private NetEventCallBack _OnSend;
    private NetEventCallBack _OnReceive;
    private NetEventCallBack _OnConnect;
    private NetEventCallBack _OnDisConnect;

    private NetEventCallBack _OnSendError;
    private NetEventCallBack _OnRecvError;
    private NetEventCallBack _OnCloseError;
    private NetEventCallBack _OnConnectError;

    #endregion

    #region 数据信息
    public int ErrorCode
    {
        get
        {
            return _errorCode;
        }

        set
        {
            _errorCode = value;
        }
    }
    public int RecvLen
    {
        get
        {
            return _recvLen;
        }

        set
        {
            _recvLen = value;
        }
    }
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

    public List<byte> RecvBuff
    {
        get
        {
            if (_recvBuff == null) _recvBuff = new List<byte>();
            return _recvBuff;
        }

        set
        {
            _recvBuff = value;
        }
    }


    #endregion

    #region 回调信息
    public NetEventCallBack OnSend
    {
        get
        {
            if (_OnSend == null) _OnSend = (StateObj state) => { };
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
            if (_OnReceive == null) _OnReceive = (StateObj state) => { };
            return _OnReceive;
        }

        set
        {
            _OnReceive = value;
        }
    }

    public NetEventCallBack OnSendError
    {
        get
        {
            if (_OnSendError == null) _OnSendError = (StateObj state) => { };
            return _OnSendError;
        }

        set
        {
            _OnSendError = value;
        }
    }

    public NetEventCallBack OnRecvError
    {
        get
        {
            if (_OnRecvError == null) _OnRecvError = (StateObj state) => { };
            return _OnRecvError;
        }

        set
        {
            _OnRecvError = value;
        }
    }

    public NetEventCallBack OnConnect
    {
        get
        {
            if (_OnConnect == null) _OnConnect = (StateObj state) => { };
            return _OnConnect;
        }

        set
        {
            _OnConnect = value;
        }
    }

    public NetEventCallBack OnDisConnect
    {
        get
        {
            if (_OnDisConnect == null) _OnDisConnect = (StateObj state) => { };
            return _OnDisConnect;
        }

        set
        {
            _OnDisConnect = value;
        }
    }

    public NetEventCallBack OnCloseError
    {
        get
        {
            if (_OnCloseError == null) _OnCloseError = (StateObj state) => { };
            return _OnCloseError;

        }
        set
        {
            _OnCloseError = value;
        }
    }

    public NetEventCallBack OnConnectError
    {
        get
        {
            return _OnConnectError;
        }

        set
        {
            _OnConnectError = value;
        }
    }




    #endregion


}
