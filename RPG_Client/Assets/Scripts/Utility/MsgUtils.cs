using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public class MsgUtils {

	public static byte[] SerializerMsg(MsgEntity msg)
    {
        string content = msg.Content + AppConst.MsgTerminator;
        byte[] bs = String2Byte(content);
        return bs;
    }

    public static byte[] String2Byte(string msg)
    {
        return AppConst.DefEncoding.GetBytes(msg);
    }
    public static string Byte2String(byte[] buff)
    {
        return AppConst.DefEncoding.GetString(buff);
    }
    public static MsgEntity DeserializerMsg(byte[] buff)
    {
        UITools.log("DeserializerMsg len : " + buff.Length);
      //  string content = Byte2String(buff);
      //  UITools.log("Msg Content : " + content);
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
