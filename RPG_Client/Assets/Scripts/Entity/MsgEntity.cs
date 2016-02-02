using UnityEngine;
using System.Collections;
/// <summary>
/// 消息实体类
/// </summary>
public class MsgEntity {

    private int _msgLen;
    private byte[] _msgContent;
    private IReceiveData _receiver;

    public int MsgLen
    {
        get
        {
            return _msgLen;
        }

        set
        {
            _msgLen = value;
        }
    }

    public byte[] MsgContent
    {
        get
        {
            return _msgContent;
        }

        set
        {
            _msgContent = value;
        }
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
}
