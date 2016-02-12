using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Collections.Generic;
using System;

public class StateObj{
    #region 字段
    private Socket _client;
    private IReceiveData _receiver;
    private bool _isNeedRecv;
    private byte[] _sendBuff;


    private byte[] _recvTempBuff;
    private ByteBuffer _recvBuff;

    private int _recvLen;
    private int _errorCode;
    private int _msgType;

    public int MsgType
    {
        get { return _msgType; }
        set { _msgType = value; }
    }


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


    public byte[] SendBuff
    {
        get { return _sendBuff; }
        set { _sendBuff = value; }
    }

    public bool IsNeedRecv
    {
        get { return _isNeedRecv; }
        set { _isNeedRecv = value; }
    }

    public ByteBuffer RecvBuff
    {
        get {
            if (_recvBuff == null) _recvBuff = new ByteBuffer();
            return _recvBuff;
        }
        set { _recvBuff = value; }
    }




    public IReceiveData Receiver
    {
        get { return _receiver; }
        set {
            _receiver = value; 
        }
    }



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
    public byte[] RecvTempBuff
    {
        get
        {
            if (_recvTempBuff == null) _recvTempBuff = new byte[AppConst.Max_Msg_Len];
            return _recvTempBuff;
        }

        set
        {
            _recvTempBuff = value;
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
