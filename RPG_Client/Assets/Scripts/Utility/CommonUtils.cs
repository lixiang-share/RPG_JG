using UnityEngine;
using System.Collections;

public class CommonUtils {

	public static byte[] SerializerMsg(MsgEntity msg)
    {
        return new byte[1024];
    }

    public static MsgEntity DeserializerMsg(byte[] buff)
    {
        UITools.log("DeserializerMsg len : " + buff.Length);
        MsgEntity msg = new MsgEntity();
        return msg;
    }

    public static int DecodeMsgRealLen(byte[] buff , int starIndex , int len)
    {
        int endIndex = starIndex + len - 1;
        if(endIndex >= buff.Length || starIndex < 0)
        {
            UITools.logError("DecodeMsgRealLen : index out range");
            return 0;
        }
        int rst = 0;
        for (int i = starIndex; i <= endIndex; i++)
        {
            rst = rst + (int)(buff[i] * Mathf.Pow(256, i -starIndex));
        }
        return rst;
    }
    public static int DecodeMsgRealLen(byte[] buff)
    {
        return DecodeMsgRealLen(buff, 0, AppConst.MsgHeadLen);
    }

}
