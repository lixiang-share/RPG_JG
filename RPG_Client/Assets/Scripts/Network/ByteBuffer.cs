using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class ByteBuffer {

   // private MemoryStream stream;
    private int _length;
    private List<byte> listBuf;
    public int Length
    {
       // get { return (int)stream.Length; }
        get { return listBuf.Count; }
    }

    //public ByteBuffer(byte[] buff)
    //{
    //    if (buff != null)
    //    {
    //        stream = new MemoryStream(buff);
    //    }
    //    else
    //    {
    //        stream = new MemoryStream();
    //    }
    //}
    public ByteBuffer()
    {
        listBuf = new List<byte>();
        //stream = new MemoryStream();
    }
    public void Write(byte b)
    {
        listBuf.Add(b);
      //  stream.WriteByte(b);
    }

    public void Write(byte[] buff)
    {
        listBuf.AddRange(buff);
       // Write(buff, 0, buff.Length);
    }

    public void Write(byte[] buff, int offset, int len)
    {
        if (buff == null || offset < 0 || len < 0 || offset>=buff.Length || offset + len > buff.Length)
        {
            UITools.log("params is wrong and can not write to ByteBuffer!!");
        }
        else
        {
            listBuf.addRange<byte>(buff, offset, len);
           // stream.Write(buff, 0,len);
        }
    }

    public byte[] ToByteArray()
    {
       // stream.Flush();
      //  stream.GetBuffer();
     //   return stream.GetBuffer();
        return listBuf.ToArray();
    }
}
