using UnityEngine;
using System.Collections;

public class CommonUtils {

	public static byte[] SerializerMsg(MsgEntity msg)
    {
        return new byte[1024];
    }

    public static MsgEntity DeserializerMsg(byte[] buff)
    {
        MsgEntity msg = new MsgEntity();
        return msg;
    }

}
