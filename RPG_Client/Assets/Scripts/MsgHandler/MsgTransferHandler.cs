using UnityEngine;
using System.Collections;

public class MsgTransferHandler : IMsgHandler
{

    public void HandleMsg(MsgHandlerMgr ctx, MsgUnPacker unpacker)
    {
        UITools.log("MsgTransferHandler");
        if (unpacker.Receiver != null)
            unpacker.Receiver.ReceiveData(unpacker);
    }
}
