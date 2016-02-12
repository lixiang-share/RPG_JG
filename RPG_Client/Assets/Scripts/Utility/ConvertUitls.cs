using UnityEngine;
using System.Collections;

public static class ConvertUitls {

    public static IList MsgToServerList(MsgUnPacker unpacker)
    {
        IList list = new ArrayList();
        int msgLen = unpacker.PopInt();
        for (int i = 0; i < msgLen; i++)
        {
            ServerItem item = new ServerItem();
            item.Id = unpacker.PopInt();
            item.Name = unpacker.PopString();
            item.Ip = unpacker.PopString();
            item.Count = unpacker.PopInt();
            list.Add(item);
        }
        return list;
    }
}
