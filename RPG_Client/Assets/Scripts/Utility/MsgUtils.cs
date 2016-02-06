using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using MsgPack;

public class MsgUtils {

	public static byte[] SerializerMsg(MsgPacker msg)
    {
        msg.add<string>(AppConst.MsgTerminator);
        byte[] bs = msg.Serialize();
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
    public static MsgUnPacker DeserializerMsg(byte[] buff)
    {
        UITools.log("DeserializerMsg len : " + buff.Length);
      //  string content = Byte2String(buff);
      //  UITools.log("Msg Content : " + content);
        //MemoryStream stream = new MemoryStream(buff);
        //Unpacker unpacker = Unpacker.Create(stream);
        //int num = 0;
        //unpacker.ReadInt32(out num);
        //double d = 0;
        //unpacker.ReadDouble(out d);
        //string str = "";
        //unpacker.ReadString(out str);
        //UITools.log(num);
        //UITools.log(d);
        //UITools.log(str);
        //unpacker.Dispose();
        for (int i = 0; i < buff.Length; i++)
        {
            UITools.log(buff[i]);
        }
        MsgUnPacker msg = new MsgUnPacker(buff);
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
