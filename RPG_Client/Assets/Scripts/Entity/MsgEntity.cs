using UnityEngine;
using System.Collections;
/// <summary>
/// 消息实体类
/// </summary>
public class MsgEntity {

    private int _type;
    private string _content;
    private IReceiveData _receiver;
    private bool isNeedRecv;

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

    public int Type
    {
        get
        {
            return _type;
        }

        set
        {
            _type = value;
        }
    }

    public string Content
    {
        get
        {
            return _content;
        }

        set
        {
            _content = value;
        }
    }

    public bool IsNeedRecv
    {
        get
        {
            return isNeedRecv;
        }
        set
        {
            isNeedRecv = value;
        }
    }
}
