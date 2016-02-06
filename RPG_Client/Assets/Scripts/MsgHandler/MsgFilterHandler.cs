using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MsgFilterHandler : IMsgHandler {


    public void HandleMsg(MsgHandlerMgr ctx, MsgUnPacker unpacker)
    {
        List<int> arr = unpacker.PopIntList();
        for (int i = 0; i < arr.Count; i++)
        {
            UITools.log(arr[i]);
        }
        UITools.log("===>" + unpacker.GetString(1));
        ctx.NextHandler(unpacker);
    }
}
