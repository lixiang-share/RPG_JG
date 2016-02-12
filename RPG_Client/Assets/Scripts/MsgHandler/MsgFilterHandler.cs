using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MsgFilterHandler : IMsgHandler {


    public void HandleMsg(MsgHandlerMgr ctx, MsgUnPacker unpacker)
    {
        UITools.log("MsgFilterHandler");
        int status = unpacker.PopInt();
        if (status == MsgProtocol.Error)
        {
            UITools.log("MsgFilterHandler === > Error");
            string msg = unpacker.PopString();
            unpacker.Close();
            UITools.log(msg);
            UITools.ShowMsg(msg);
        }
        else if (status == MsgProtocol.Success)
        {
            ctx.NextHandler(unpacker);
        }
    }
}
